using UnityEngine;
using UnityEditor;
using System.Collections;

//[CustomEditor(typeof(BoxCollider))]
public class BoxColliderInspectorEditor : Editor {
    BoxCollider colider;
    Vector3 center;
    Vector3 centerOrg;
    Vector3 size;
    Vector3 sizeOrg;
    public static EditType type;
    void OnEnable() {
        colider = target as BoxCollider;
        centerOrg = center = colider.center;
        sizeOrg = size = colider.size;

    }

    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        type = (EditType)EditorGUILayout.EnumPopup("ѡ��༭����", type);
        if (type == EditType.none) {
            GUILayout.Label("��ǰѡ��ģʽΪ:��ͨ(UnityĬ����ײ�б༭״̬).");
        }
        else if (type == EditType.center) {
            GUILayout.Label("��ǰѡ��ģʽΪ:�������ĵ�.����϶�Scene����λ�ÿ��Ʊ�,������ײ��λ��.");
        }
        else if (type == EditType.size) {
            GUILayout.Label("��ǰѡ��ģʽΪ:����.��������Scene�������ſ��Ʊ�,������ײ�д�С.");
        }
    }


    void OnSceneGUI() {
        if ((Event.current.modifiers & (EventModifiers.Control | EventModifiers.Shift | EventModifiers.Alt)) == 0){
            if (Event.current.keyCode == KeyCode.C) {
                centerOrg = center = colider.center;
                type = EditType.center;
                Event.current.Use();
            }else if (Event.current.keyCode == KeyCode.S) {
                sizeOrg = size = colider.size;
                type = EditType.size;
                Event.current.Use();
            }else if (Event.current.keyCode == KeyCode.Q) {
                type = EditType.none;
                Event.current.Use();
            }
        }
        if (type == EditType.center){
            center = Handles.PositionHandle(center + colider.transform.position, colider.transform.rotation) - colider.transform.position;
            if (center != centerOrg) {
                colider.center += (center - centerOrg) * .1f;
                centerOrg = center;
            }
            Handles.Label(colider.transform.position + colider.center, "Center Mode");
        }else if(type == EditType.size){
            size = Handles.ScaleHandle(size, colider.transform.position + colider.center, colider.transform.rotation, 15);
            if (size != sizeOrg) {
                colider.size += (size - sizeOrg) * .1f;
                sizeOrg = size;
            }
            Handles.Label(colider.transform.position + colider.center, "Size Mode");
        }
    }
    public enum EditType { 
        center,
        size,
        none
    }
}