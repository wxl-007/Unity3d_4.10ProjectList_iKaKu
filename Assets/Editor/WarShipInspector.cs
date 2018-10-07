using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(WarShip))]
[CanEditMultipleObjects]
public class WarShipInspector : Editor {

    WarShip warShip;

    void OnEnable() {
        warShip = target as WarShip;
        if (warShip.cannons == null || warShip.cannons.Length == 0) {
            Debug.Log("给炮组赋值");
            int length = 0;
            for (int i = 0; i < warShip.transform.childCount; i++) {
                if (warShip.transform.GetChild(i).name.Equals("Cannon")){
                    length++;
                }
            }
            warShip.cannons = new Cannon[length];
            length = 0;
            for (int i = 0; i < warShip.transform.childCount; i++) {
                if (warShip.transform.GetChild(i).name.Equals("Cannon")) {
                    warShip.cannons[length] = warShip.transform.GetChild(i).GetComponent<Cannon>();
                    length++;
                }
            }
        }
    }

    public override void OnInspectorGUI(){
        if (warShip.shipData != null){
            GUILayout.Label("移动速度: " + warShip.shipData.moveSpeed);
        }else {
            GUILayout.Label("当前战船数据未赋值");
        }

        if(GUILayout.Button("Get Externts Info")){
            GetExternts();
        }else if (GUILayout.Button("Get All Cannons")) {
            Undo.RegisterUndo(warShip, "Get All Cannons");
            canonTmp.Clear();
            GetAllCannons();
        }
        base.OnInspectorGUI();
    }

    ArrayList canonTmp = new ArrayList();
    void GetAllCannons() {
        checkCannon(warShip.transform);
        warShip.cannons = new Cannon[canonTmp.Count];
        for (int i = 0; i < canonTmp.Count; i++) {
            warShip.cannons[i] = (Cannon)canonTmp[i];
        }
    }
    void checkCannon(Transform root) {
        if (root.GetComponent<Cannon>()){
            canonTmp.Add(root.GetComponent<Cannon>());
        }else {
            for (int i = 0; i < root.childCount; i++) {
                checkCannon(root.GetChild(i));
            }
        }
    }

    void GetExternts() { 
        Quaternion tmp = warShip.transform.rotation;
        warShip.transform.rotation = Quaternion.identity;
        warShip.externts = warShip.collider.bounds.extents;
        warShip.transform.rotation = tmp;
        //EditorUtility.SetDirty(warShip);
    }
}
