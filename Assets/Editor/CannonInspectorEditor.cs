using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Cannon))]
[CanEditMultipleObjects]
public class CannonInspectorEditor : Editor {
    Cannon cannon;
    Quaternion org;

    void OnEnable() {
        cannon = target as Cannon;
        if (Application.isPlaying)
            return;
        org = cannon.transform.parent.rotation;
        cannon.transform.parent.rotation = Quaternion.identity;
        if (cannon.leftSightAxis == Vector3.zero) {
            cannon.leftSightAxis = -cannon.transform.right;
        }
        if (cannon.righSightAxis == Vector3.zero) {
            cannon.righSightAxis = cannon.transform.right;
        }
    }

    void OnDisable(){
        if (Application.isPlaying)
            return;
        if (cannon)
            cannon.transform.parent.rotation = org;
    }

    void OnSceneGUI() {
        if (Application.isPlaying){
            Handles.Label(cannon.transform.position + cannon.transform.parent.rotation * cannon.leftSightAxis, "Left Sight Axis");
            Handles.Label(cannon.transform.position + cannon.transform.parent.rotation * cannon.righSightAxis, "Right Sight Axis");
        }else{
            Undo.SetSnapshotTarget(cannon, "Adjust Cannon Axis");
            cannon.leftSightAxis = Handles.PositionHandle(cannon.transform.position + cannon.leftSightAxis, Quaternion.identity) - cannon.transform.position;
            Handles.Label(cannon.transform.position + cannon.leftSightAxis, "LeftSight");
            cannon.righSightAxis = Handles.PositionHandle(cannon.transform.position + cannon.righSightAxis, Quaternion.identity) - cannon.transform.position;
            Handles.Label(cannon.transform.position + cannon.righSightAxis, "RightSight");
        }
    }
}