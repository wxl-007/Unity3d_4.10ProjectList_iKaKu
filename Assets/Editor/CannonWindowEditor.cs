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
        GUILayout.Label("ѡ���ڻ�");
        cannonBase = EditorGUILayout.ObjectField(cannonBase, typeof(GameObject)) as GameObject;
        if (Selection.activeGameObject){
            if (GUILayout.Button("��ǰѡ��")){
                cannonBase = Selection.activeGameObject;
            }
        }
        GUILayout.EndHorizontal();


        GUILayout.BeginHorizontal();
        GUILayout.Label("ѡ����Ӧ�ڹ�");
        cannonBarrel = EditorGUILayout.ObjectField(cannonBarrel, typeof(GameObject)) as GameObject;
        if (Selection.activeGameObject){
            if (GUILayout.Button("��ǰѡ��")){
                cannonBarrel = Selection.activeGameObject;
            }
        }
        GUILayout.EndHorizontal();

        side = (CannonSide)EditorGUILayout.EnumPopup("ѡ������Դ��ĳ���", side);
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
            case CannonSide.���:
                CreateChild(cannonBarrel.transform, Vector3.back, "CannonBarrel").transform.parent = CreateChild(cannonBase.transform, Vector3.back, "Cannon").transform;
                break;
            case CannonSide.��ǰ:
                CreateChild(cannonBarrel.transform, Vector3.forward, "CannonBarrel").transform.parent = CreateChild(cannonBase.transform, Vector3.forward, "Cannon").transform;
                break;
            case CannonSide.����:
                CreateChild(cannonBarrel.transform, Vector3.left, "CannonBarrel").transform.parent = CreateChild(cannonBase.transform, Vector3.left, "Cannon").transform;
                break;
            case CannonSide.����:
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
        //���ĸ��ӹ�ϵ
        if (parent.parent){
            tmp.transform.parent = parent.parent;
        }else{
            tmp.transform.parent = null;
        }
        parent.parent = tmp.transform;
        //���ش����õ�����
        return tmp;
    }

    enum CannonSide { 
        ����   = 0,
        ����   = 1,
        ��ǰ   = 2,
        ���   = 3
    }
}