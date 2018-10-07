using UnityEngine;
using System.Collections;
using UnityEditor;

public class MeshEditorWindow : ScriptableWizard{
    [MenuItem("Plugins/Open Mesh Editor #m")]
    static void Init() {
        ScriptableWizard.DisplayWizard("mesheditor", typeof(MeshEditorWindow), "Close");
    }
    public int number = 0;
    void OnWizardUpdate()
    {
        helpString = "Set The number to 5";
        if (number != 5)
            errorString = "The number has to be set to 5!";
        else
            errorString = "";
    }
}