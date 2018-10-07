using UnityEngine;
using UnityEditor;
using System.Collections;

public class SnapToGrid : EditorWindow {
    [MenuItem("Plugins/SnapToWorldGrid &x")]
    static void Snap(){
        Transform[] target = Selection.transforms;
        if (target != null){
            foreach (Transform sl in target){
                Vector3 pos = sl.position;
                Undo.RegisterUndo(sl, "Span To World Grid");
                sl.position = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
            }
        }
    }

    [MenuItem("Plugins/SnapToLocalGrid &z")]
    static void SnapAssistant(){
        Transform[] targets = Selection.transforms;
        foreach (Transform rt in targets){
            Vector3 pos = rt.localPosition;
            Undo.RegisterUndo(rt, "SnapToLocalGrid");
            rt.localPosition = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
        }
    }

    [MenuItem("Plugins/Hide WireFrame &h")]
    static void HideWireFrame() {
        foreach (Transform c in Selection.transforms){
            SetFramShow(c, true);
        }
    }

    [MenuItem("Plugins/Hide WireFrame &s")]
    static void ShowWireFrame(){
        foreach (Transform c in Selection.transforms){
            SetFramShow(c, false);
        }
    }
    static void SetFramShow(Transform selected, bool hide) {
        if (selected.renderer)
            EditorUtility.SetSelectedWireframeHidden(selected.renderer, hide);
        for (int i = 0; i < selected.childCount; i++) {
            SetFramShow(selected.GetChild(i),hide);
        }
    }

    [MenuItem("Plugins/Make Group &g")]
    static void MakeGroup(){
        Undo.RegisterSceneUndo("Make Group");
        GameObject tmp = new GameObject("Group");
        Transform[] targets = Selection.transforms;
        Vector3 center = Vector3.zero;
        for (int i = 0; i < targets.Length; i++) {
            center += targets[i].position;
        }
        center /= targets.Length;
        tmp.transform.position = center;
        foreach (Transform chil in targets) {
            chil.parent = tmp.transform;
        }
        Selection.activeGameObject = tmp;
    }

    [MenuItem("Plugins/创建Path &p")]
    static void MakePath() {
        GameObject tmp = new GameObject("MovePath");
        Selection.activeGameObject = tmp;
        Undo.RegisterCreatedObjectUndo(tmp, "Create MovePath");
        tmp.AddComponent<MovePath>();
    }

    [MenuItem("Plugins/炮管 &b")]
    static void MakeBarrel() {
        if (!Selection.activeTransform) {
            EditorUtility.DisplayDialog("Error", "选择为空", "确定");
            return;
        }
        Undo.RegisterSceneUndo("Set The Cannon Barrel");
        Transform barrel = Selection.activeTransform;
        barrel.name = "CannonBarrel";
        Transform barrelParent = barrel.parent;
        if (!barrelParent){
            EditorUtility.DisplayDialog("Error", "选中物体不可无父物体", "确定");
            return;
        }
        barrel.localPosition = Vector3.zero;    //相对位置归零
        barrel.localScale = Vector3.one;        //相对缩放归一
        if (barrelParent.parent)
            barrel.parent = barrelParent.parent;
        else
            barrel.parent = null;
        barrelParent.parent = barrel;
    }

    [MenuItem("Plugins/炮基 &c")]
    static void MakeCannon() {
        if (!Selection.activeTransform) {
            EditorUtility.DisplayDialog("Error", "选择为空", "确定");
            return;
        }
        Undo.RegisterSceneUndo("Set The Cannon Base");
        Transform Cannon = Selection.activeTransform;
        Cannon.name = "Cannon";
        Transform cannonParent = Cannon.parent;
        if (!cannonParent){
            EditorUtility.DisplayDialog("Error", "选中物体不可无父物体", "确定");
            return;
        }
        Cannon.localPosition = Vector3.zero;    //相对位置归零
        Cannon.localScale = Vector3.one;        //相对缩放归一
        if (cannonParent.parent)
            Cannon.parent = cannonParent.parent;
        else
            Cannon.parent = null;
        cannonParent.parent = Cannon;
    }

    [MenuItem("Plugins/Add Child &d")]
    static void AddChild() {
        if (Selection.activeTransform) {
            string childName = GetDiffName(Selection.activeTransform);
            GameObject child = new GameObject(childName);
            child.transform.parent = Selection.activeTransform;
            child.transform.localPosition = Vector3.zero;
            Undo.RegisterCreatedObjectUndo(child, childName);
        }else{
            EditorUtility.DisplayDialog("Error", "选择不能为空", "OK");
        }
    }
    static string GetDiffName(Transform target) {
        string newName = "child_0";
        string tmp = "";
        for (int i = 0; i < target.childCount; i++) {
            tmp += target.GetChild(i).name;
        }
        for (int i = 0; i < tmp.Length; i++){
            if (!tmp.Contains(newName + i)){
                return newName + i;
            }
        }
        return newName + target.childCount;
    }

    [MenuItem("Plugins/前后朝向调换 &f")]
    static void ChangeDir() {
        Undo.RegisterUndo(Selection.activeTransform, "Change Direction To Back");
        Selection.activeTransform.forward = Vector3.back;
    }

    [MenuItem("Plugins/右侧为前方 &r")]
    static void ChangeDir_R() {
        Undo.RegisterUndo(Selection.activeGameObject, "Change Direction To Right");
        Selection.activeTransform.forward = Vector3.right;
    }

    [MenuItem("Plugins/左侧为前方 &l")]
    static void ChangeDir_L() { 
        Undo.RegisterUndo(Selection.activeGameObject, "Change Direction To Left");
        Selection.activeTransform.forward = Vector3.left;
    }

    [MenuItem("Plugins/Add Box Collider &o")]
    static void AddBoxCollider() {
        Undo.RegisterUndo(Selection.activeGameObject, "Add Box Collider");
        Selection.activeGameObject.AddComponent<BoxCollider>();
    }

    [MenuItem("Plugins/Add Unit To Selected &u")]
    static void AddUnit() {
        GameObject tmp = new GameObject("Unit");
        tmp.transform.parent = Selection.activeTransform;
        tmp.transform.localPosition = Vector3.zero;
        tmp.AddComponent<Unit>();
        Selection.activeGameObject = tmp;
        Undo.RegisterCreatedObjectUndo(tmp, "Add Uni");
    }
}