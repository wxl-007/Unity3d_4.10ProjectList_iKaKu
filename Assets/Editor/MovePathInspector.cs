using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomEditor(typeof(MovePath))]
public class MovePathInspector : Editor{
    MovePath movePath;
    int nodeToInsert;
    int nodeToRemove;
    int nodeToSW_1;
    int nodeToSW_2;
    float heightToAlign = 0;
    GUIStyle style;
    void OnEnable() {
        movePath = target as MovePath;
        if (movePath.pathNode == null) { 
            movePath.pathNode = new Vector3[0];
        }
        style = new GUIStyle();
        style.fontStyle = FontStyle.Normal;
        style.fontSize = 35;
        style.normal.textColor = Color.white;
        orgStyle = opstyle;
        Debug.Log(PrefabUtility.GetPrefabType(Selection.activeGameObject));
        if (PrefabUtility.GetPrefabType(movePath) == PrefabType.PrefabInstance)
            EditorUtility.DisplayDialog("警告!", "对路径做完改动之后请不要忘记[Apply],避免丢失数据!", "确定");
    }

    void OnDisable() {
        if (!movePath)
            return;
        if (PrefabUtility.GetPrefabType(movePath) == PrefabType.PrefabInstance){
            if (EditorUtility.DisplayDialog("警告!", "若未将更改应用到预设,运行时或者保存时会丢失更改!确定以应用更改到预设!", "Apply", "Don't Save")){
                GameObject curSelect = Selection.activeGameObject;
                Selection.activeGameObject = movePath.gameObject;
                EditorApplication.ExecuteMenuItem("GameObject/Apply Changes To Prefab");
                Selection.activeGameObject = curSelect;
            }
        }
    }

    public override void OnInspectorGUI(){
        GUILayout.BeginHorizontal();
        nodeToInsert = EditorGUILayout.IntField(nodeToInsert);
        if (GUILayout.Button("添加转折点")) {
            AddNode();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        nodeToRemove = EditorGUILayout.IntField(nodeToRemove);
        if (GUILayout.Button("移除折点")) {
            RemoveNode();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        nodeToSW_1 = EditorGUILayout.IntField(nodeToSW_1);
        nodeToSW_2 = EditorGUILayout.IntField(nodeToSW_2);
        if (GUILayout.Button("交换折点")) {
            SwitchNode();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        type = (Loop)EditorGUILayout.EnumPopup("对接点", type);//首:将终点对接到起点 / 尾:将起点对接到终点 / 中:将起点和终点对接到两者中心位置
        if (GUILayout.Button("首尾相接")) {
            LoopSpline();
        }
        GUILayout.EndHorizontal();

        opstyle = (OperatorStyle)EditorGUILayout.EnumPopup("可操作方式", opstyle);
        if (opstyle != orgStyle) {
            orgStyle = opstyle;
            switch (opstyle) { 
                case OperatorStyle.scale:
                    orgState = movePath.transform.localScale;
                    break;
                case OperatorStyle.rotate:
                    orgState = movePath.transform.forward;
                    break;
                case OperatorStyle.move:
                    orgState = movePath.transform.position;
                    break;
            }
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("对齐高度到")) {
            for (int i = 0; i < movePath.pathNode.Length; i++) {
                movePath.pathNode[i].y = heightToAlign;
            }
        }
        heightToAlign = EditorGUILayout.FloatField(heightToAlign);
        GUILayout.EndHorizontal();
        base.OnInspectorGUI();
    }

    Vector3 orgState;
    OperatorStyle orgStyle;
    void OnSceneGUI(){
        for (int i = 0; i < movePath.pathNode.Length; i++) {
            Undo.SetSnapshotTarget(movePath, "Move Path Nodes = " + i);
            movePath.pathNode[i] = Handles.PositionHandle(movePath.pathNode[i], movePath.transform.rotation);
            Handles.Label(movePath.pathNode[i], i.ToString(), style);
        }
        switch (opstyle) {
            case OperatorStyle.move:
                Handles.Label(movePath.transform.position, "Move");
                if (movePath.transform.position != orgState) {
                    Vector3 deltaPos = movePath.transform.position - orgState;
                    //Undo.SetSnapshotTarget(movePath, "Move Path Nodes");
                    //Undo.RegisterSceneUndo("Move path nodes");
                    for (int i = 0; i < movePath.pathNode.Length; i++) {
                        movePath.pathNode[i] += deltaPos;
                    }
                    orgState = movePath.transform.position;
                }
                break;
            case OperatorStyle.rotate:
                Handles.Label(movePath.transform.position, "Rotate");
                if (movePath.transform.forward != orgState) {
                    Quaternion deltaQ = Quaternion.FromToRotation(orgState, movePath.transform.forward);
                    //Undo.RegisterSceneUndo("Rotate path nodes");
                    //Undo.SetSnapshotTarget(movePath, "Rotate Path Nodes");
                    for (int i = 0; i < movePath.pathNode.Length; i++) {
                        movePath.pathNode[i] = deltaQ * (movePath.pathNode[i] - movePath.transform.position) + movePath.transform.position;
                    }
                    orgState = movePath.transform.forward;
                }
                break;
            case OperatorStyle.scale:
                Handles.Label(movePath.transform.position, "Scale");
                if (movePath.transform.localScale != orgState) {
                    Vector3 deltaScale = new Vector3(movePath.transform.localScale.x / orgState.x, movePath.transform.localScale.y / orgState.y, movePath.transform.localScale.z / orgState.z);
                    //Undo.SetSnapshotTarget(movePath, "Scale Path Nodes");
                    Vector3 delpos;
                    for (int i = 0; i < movePath.pathNode.Length; i++) {
                        delpos = movePath.pathNode[i] - movePath.transform.position;
                        movePath.pathNode[i] = new Vector3(delpos.x * deltaScale.x, delpos.y * deltaScale.y, delpos.z * deltaScale.z) + movePath.transform.position;
                    }
                    orgState = movePath.transform.localScale;
                }
                break;
            case OperatorStyle.none:
                Handles.Label(movePath.transform.position, "None");
                break;
        }
        if ((Event.current.modifiers & (EventModifiers.Alt | EventModifiers.Control | EventModifiers.Shift)) == 0) {
            if (Event.current.keyCode == KeyCode.W) {
                orgState = movePath.transform.position;
                opstyle = OperatorStyle.move;
            }else if (Event.current.keyCode == KeyCode.Q) {
                opstyle = OperatorStyle.none;
            }else if (Event.current.keyCode == KeyCode.R) {
                orgState = movePath.transform.localScale;
                opstyle = OperatorStyle.scale;
            }else if (Event.current.keyCode == KeyCode.E) {
                orgState = movePath.transform.forward;
                opstyle = OperatorStyle.rotate;
            }
        }
        if (Event.current.type == EventType.KeyDown){
            if (Event.current.keyCode == KeyCode.KeypadPlus){
                nodeToInsert = movePath.pathNode.Length;
                AddNode();
            }else if (Event.current.keyCode == KeyCode.KeypadMinus){
                nodeToRemove = movePath.pathNode.Length - 1;
                RemoveNode();
            }
        }
    }

    void AddNode() {
        if (nodeToInsert < 0 || nodeToInsert > movePath.pathNode.Length) {
            EditorUtility.DisplayDialog("错误", "插入节点下标不可" + (nodeToInsert < 0 ? "小于0!" : "大于转折点数组长度"), "确定");
            return;
        }
        Undo.RegisterUndo(movePath, "Insert Node");
        Vector3 insertPos;
        if (movePath.pathNode == null || movePath.pathNode.Length == 0) {
            movePath.pathNode = new Vector3[] { Vector3.zero };
            return;
        }else if (movePath.pathNode.Length == nodeToInsert) {
            Vector3[] _tmp = new Vector3[movePath.pathNode.Length + 1];
            Array.Copy(movePath.pathNode, _tmp, nodeToInsert);
            movePath.pathNode = _tmp;
            movePath.pathNode[nodeToInsert] = Vector3.zero;
            return;
        }
        insertPos = (movePath.pathNode[nodeToInsert - 1 < 0 ? movePath.pathNode.Length - 1 : nodeToInsert - 1] + movePath.pathNode[nodeToInsert]) * .5f;
        if (insertPos == Vector3.zero){
            insertPos = movePath.transform.position;
        }
        Vector3[] tmp = new Vector3[movePath.pathNode.Length + 1];
        for (int i = 0; i < tmp.Length; i++) {
            if (i < nodeToInsert){
                tmp[i] = movePath.pathNode[i];
            }
            else if (i > nodeToInsert){
                tmp[i] = movePath.pathNode[i - 1];
            }
            else {
                tmp[i] = insertPos;
            }
        }
        movePath.pathNode = tmp;
    }

    void RemoveNode() {
        if (nodeToRemove >= movePath.pathNode.Length || nodeToRemove < 0) {
            EditorUtility.DisplayDialog("错误", "移除下标不可" + (nodeToRemove < 0 ? "小于0!" : "大于转折点数组长度"), "确定");
            return;
        }
        Undo.RegisterUndo(movePath, "Remove Node");
        Vector3[] pathNode = new Vector3[movePath.pathNode.Length - 1];
        for (int i = 0; i < pathNode.Length; i++) {
            if (i < nodeToRemove){
                pathNode[i] = movePath.pathNode[i];
            }
            else {
                pathNode[i] = movePath.pathNode[i + 1];
            }
        }
        movePath.pathNode = pathNode;
    }

    void SwitchNode() {
        if (nodeToSW_1 == nodeToSW_2) {
            EditorUtility.DisplayDialog("错误", "输入的两个交换点相同,误操作!", "确定");
        }
        else if (nodeToSW_1 >= movePath.pathNode.Length || nodeToSW_1 < 0) { 
            EditorUtility.DisplayDialog("错误", "第一个转折点下标" + (nodeToSW_1 < 0 ? "不能小于0" : "不能大于转折点数组总长度"), "确定");
        }
        else if (nodeToSW_2 < 0 || nodeToSW_2 >= movePath.pathNode.Length)
        {
            EditorUtility.DisplayDialog("错误", "第二个转折点下标" + (nodeToSW_2 < 0 ? "不能小于0" : "不能大于转折点数组总长度"), "确定");
        }
        else {
            Undo.RegisterUndo(movePath, "Exchange Node");
            Vector3 tmp = movePath.pathNode[nodeToSW_1];
            movePath.pathNode[nodeToSW_1] = movePath.pathNode[nodeToSW_2];
            movePath.pathNode[nodeToSW_2] = tmp;
        }
    }

    Loop type = Loop.首;
    OperatorStyle opstyle = OperatorStyle.none;
    void LoopSpline() {
        Undo.RegisterUndo(movePath, "首尾相接");
        switch (type) { 
            case Loop.首:
                movePath.pathNode[movePath.pathNode.Length - 1] = movePath.pathNode[0];
                break;
            case Loop.尾:
                movePath.pathNode[0] = movePath.pathNode[movePath.pathNode.Length - 1];
                break;
            case Loop.中:
                Vector3 tmp = (movePath.pathNode[0] + movePath.pathNode[movePath.pathNode.Length - 1]) * .5f;
                movePath.pathNode[0] = movePath.pathNode[movePath.pathNode.Length - 1] = tmp;
                break;
        }
    }
    enum Loop { 
        首,
        尾,
        中
    }

    enum OperatorStyle {
        scale,
        move,
        rotate,
        none
    }
}
