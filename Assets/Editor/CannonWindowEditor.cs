using UnityEngine;
using System.Collections;
using UnityEditor;

public class CannonWindowEditor : EditorWindow{
    [MenuItem("Plugins/Adjust Cannon #c")]
    static void CannonWindow() {
        EditorWindow.GetWindow(typeof(CannonWindowEditor));
    }
    GameObject cannonBase;
    GameObject cannonBarrel;
    CannonSide side;
    void OnGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label("选择炮基");
        cannonBase = EditorGUILayout.ObjectField(cannonBase, typeof(GameObject)) as GameObject;
        if (Selection.activeGameObject){
            if (GUILayout.Button("当前选中")){
                cannonBase = Selection.activeGameObject;
            }
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("选中相应炮管");
        cannonBarrel = EditorGUILayout.ObjectField(cannonBarrel, typeof(GameObject)) as GameObject;
        if (Selection.activeGameObject){
            if (GUILayout.Button("当前选中")){
                cannonBarrel = Selection.activeGameObject;
            }
        }
        GUILayout.EndHorizontal();

        side = (CannonSide)EditorGUILayout.EnumPopup("选择炮相对船的朝向", side);
        if (cannonBase && cannonBarrel) {
            if (GUILayout.Button("Adjust")) {
                Adjust();
            }
        }
        if (Event.current.type == EventType.keyDown) {
            if (Event.current.keyCode == KeyCode.Escape) {
                this.Close();
            }
        }
    }

    void OnSelectionChange() {
        this.Repaint();
    }

    void Adjust() {
        Undo.RegisterSceneUndo("Adjust Cannon");
        switch (side) { 
            case CannonSide.向后:
                CreateChild(cannonBarrel.transform, Vector3.back, "CannonBarrel").transform.parent = CreateChild(cannonBase.transform, Vector3.back, "Cannon").transform;
                break;
            case CannonSide.向前:
                CreateChild(cannonBarrel.transform, Vector3.forward, "CannonBarrel").transform.parent = CreateChild(cannonBase.transform, Vector3.forward, "Cannon").transform;
                break;
            case CannonSide.向左:
                CreateChild(cannonBarrel.transform, Vector3.left, "CannonBarrel").transform.parent = CreateChild(cannonBase.transform, Vector3.left, "Cannon").transform;
                break;
            case CannonSide.向右:
                CreateChild(cannonBarrel.transform, Vector3.right, "CannonBarrel").transform.parent = CreateChild(cannonBase.transform, Vector3.right, "Cannon").transform;
                break;
        }
        cannonBase = null;
        cannonBarrel = null;
    }

    GameObject CreateChild(Transform parent,Vector3 forward, string name) {
        GameObject tmp = new GameObject(name);
        tmp.transform.parent = parent;
        tmp.transform.localPosition = Vector3.zero;
        tmp.transform.forward = forward;
        //更改父子关系
        if (parent.parent){
            tmp.transform.parent = parent.parent;
        }else{
            tmp.transform.parent = null;
        }
        parent.parent = tmp.transform;
        //返回创建好的物体
        return tmp;
    }

    enum CannonSide { 
        向左   = 0,
        向右   = 1,
        向前   = 2,
        向后   = 3
    }
}