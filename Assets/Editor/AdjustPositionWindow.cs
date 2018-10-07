using UnityEngine;
using System.Collections;
using UnityEditor;

public class AdjustPositionWindow : EditorWindow{
    [MenuItem("Plugins/AdjustPosition #a")]
    static void AdjustWindow() {
        EditorWindow win = EditorWindow.GetWindow((typeof(AdjustPositionWindow)));
    }

    Transform target;
    Vector2 scrollView = Vector2.zero;
    void OnGUI() {
        target = EditorGUILayout.ObjectField("跟随物体", target, typeof(Transform)) as Transform;
        GUILayout.BeginVertical();
        scrollView = GUILayout.BeginScrollView(scrollView);
        GUILayout.Label("当前选中物体:");
        if (Selection.gameObjects.Length != 0)
            foreach (GameObject other in Selection.gameObjects){
                GUILayout.Label(other.name);
            }
        else
            GUILayout.Label("(未选中目标)");
        if (target)
            GUILayout.Label("将跟随 [" + target.name + "] 矫正坐标位置");
        else
            GUILayout.Label("请先选中跟随物体");
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        if (GUILayout.Button("Adjust")) {
            Adjust();
        }
        if (Event.current.type == EventType.keyDown) {
            if (Event.current.keyCode == KeyCode.Escape) {
                EditorWindow.focusedWindow.Close();
            }
        }
    }
    
    void OnSelectionChange() {
        EditorWindow.GetWindow(typeof(AdjustPositionWindow)).Repaint();
    }

    Vector3 delta;
    void Adjust() {
        delta = target.localPosition;
        Undo.RegisterSceneUndo("Adjust Position");
        foreach (Transform other in Selection.transforms) {
            other.transform.position -= delta;
        }
    }
}