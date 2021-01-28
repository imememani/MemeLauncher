using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(GUIDefenition))]
public class GUIDefEditor : Editor
{
    private GUIDefenition def;

    private bool canBuild = true;
    private GUIStyle headerLabel;

    private bool hasinit;

    public override void OnInspectorGUI()
    {
        def = (GUIDefenition)target;

        if (!hasinit)
        {
            foreach (Transform trsf in def.GetComponentsInChildren<Transform>())
            {
                trsf.gameObject.layer = 5;
            }

            hasinit = true;
        }

        headerLabel = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 12
        };

        BeginVertical(EditorStyles.helpBox);

        DrawLine("Name Your Window");
        def.name = TextField(new GUIContent("Name", "The uh...Name of your window?"), def.name);
        def.modName = TextField(new GUIContent("Mod Name", "This is used to locate your mod folder to build your gui files to."), def.modName);

        DrawLine("Options");
        def.hasOwnFolder = Toggle(new GUIContent("Has Own Folder", "Does this GUI require its own dedicated folder?"), def.hasOwnFolder);

        DrawLine();
        Space();

        DrawLine("Export");
        BeginHorizontal(EditorStyles.toolbar);
        DrawExport();
        EndHorizontal();
        EndVertical();

        Space();
        Space();
        Space();
        Space();

        EnsureSettingsDontConflict();
    }

    private void DrawExport()
    {
        if (GUILayout.Button(!canBuild ? "Export (FIX ERRORS)" : "Export", EditorStyles.toolbarButton))
        {
            if (canBuild)
            {
                SavePrefab();

                BundleUtilities.ExportSelectedBundles(out string output, "gui", PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(target));

                string path = GetExportPath();

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!Directory.Exists(Path.Combine(path, "Icons")))
                {
                    Directory.CreateDirectory(Path.Combine(path, "Icons"));
                }

                BundleUtilities.GroupFilesToFolder(output, path, def.name.Replace(" ", ""));

                EditorUtility.DisplayDialog("Exported", "Your piece has been exported. You can launch GORN and test.", "Grucci");
            }
            else
            {
                EditorUtility.DisplayDialog("ERROR", "You have errors you need to fix before you can export.", "FUUUUUUUUUU");
            }
        }
        if (GUILayout.Button("Save Prefab", EditorStyles.toolbarButton))
        {
            SavePrefab();
        }
    }

    private string GetExportPath()
    {
        if (Directory.Exists(Path.Combine(SDKEditor.settings.gornFolder, "Mods", def.modName)))
        {
            return Path.Combine(SDKEditor.settings.gornFolder, "Mods", def.modName, "GUI", def.modName, def.hasOwnFolder ? Path.Combine("GUI", def.name) : "GUI");
        }
        else if (Directory.Exists(Path.Combine(SDKEditor.settings.gornFolder, def.modName)))
        {
            return Path.Combine(SDKEditor.settings.gornFolder, def.modName, def.hasOwnFolder ? Path.Combine("GUI", def.name) : "GUI");
        }

        return null;
    }

    private void EnsureSettingsDontConflict()
    {
        DrawLine("Log");
        int messageCount = 0;
        canBuild = true;

        if (def.name == "New Window")
        {
            HelpBox("Your window will be named the default 'New Window' is this intentional?", MessageType.Warning);
            messageCount++;
        }

        if (string.IsNullOrEmpty(def.modName))
        {
            HelpBox("You must specify the name of your mod so the files can be built to your folder.", MessageType.Error);
            canBuild = false;
            messageCount++;
        }

        if (string.IsNullOrEmpty(SDKEditor.settings.gornFolder))
        {
            HelpBox("You haven't selected a valid GORN folder.", MessageType.Error);
            canBuild = false;
            messageCount++;
        }

        foreach (Transform trsf in def.GetComponentsInChildren<Transform>())
        {
            if (trsf.gameObject.layer != 5)
            {
                HelpBox($"{trsf.name} is not using the UI layer, this means it will not show up in the menu environment and will be culled.", MessageType.Warning);
                messageCount++;
            }
        }

        if (messageCount == 0)
        {
            if (GetExportPath() is string export)
            {
                HelpBox("Everything looks good! No warnings/errors!", MessageType.Info);
                HelpBox($"Export: {export}", MessageType.Info);
            }
        }
    }

    private void SavePrefab()
    {
        string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(def.gameObject);

        EditorUtility.SetDirty(def);
        PrefabUtility.SaveAsPrefabAssetAndConnect(def.gameObject, !string.IsNullOrEmpty(path) ? path : $"Assets/{def.name}.prefab", InteractionMode.AutomatedAction);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void DrawLine(string header = "")
    {
        BeginVertical(EditorStyles.helpBox);
        if (!string.IsNullOrEmpty(header)) LabelField(header, headerLabel);
        EndVertical();
    }
}