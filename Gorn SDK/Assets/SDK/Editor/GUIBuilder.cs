using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;

public class GUIBuilder: EditorWindow
{
    [MenuItem("GORN SDK/GUI Builder")]
    private static void GetWindow()
    {
        GUIBuilder window = GetWindow<GUIBuilder>("GUI BUILDER");
        window.Show();
    }

    private void OnGUI()
    {
        LabelField("This feature is not yet complete.");
    }
}