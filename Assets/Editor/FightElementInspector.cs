using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

[CustomEditor(typeof(FightElement))]
public class FightElementInspector : Editor {

    FightElement fightElement;

    void OnEnable() {
        fightElement = target as FightElement;
    }

    SerializedProperty ser_Pro;
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
    }
}