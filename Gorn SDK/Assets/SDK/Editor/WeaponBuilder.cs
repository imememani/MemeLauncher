using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

public class WeaponBuilder: EditorWindow
{
    [MenuItem("GORN SDK/Weapon Builder")]
    private static void GetWindow()
    {
        WeaponBuilder window = GetWindow<WeaponBuilder>("WEAPON BUILDER");
        window.Show();
    }

    public WeaponDefinition weaponDefinition;

    private void OnGUI()
    {
        weaponDefinition = (WeaponDefinition)ObjectField("Weapon Definition: ", weaponDefinition, typeof(WeaponDefinition), true);

        if (GUILayout.Button("BUILD MANIFEST"))
        {
            string bundleDir = $"{Application.dataPath}\\AssetBundles";

            if (!Directory.Exists(bundleDir))
                Directory.CreateDirectory(bundleDir);
            else
            {
                Directory.Delete(bundleDir, true);
                Directory.CreateDirectory(bundleDir);
            }

            File.WriteAllText($"{Application.dataPath}\\AssetBundles\\{weaponDefinition.ID}.weaponmanifest", JsonConvert.SerializeObject(weaponDefinition.BuildManifest(), Formatting.Indented));

            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("BUILD ALL"))
        {
            Build();
        }
    }

    private void Build()
    {
        string bundleDir = $"{Application.dataPath}\\AssetBundles";

        Debug.Log(bundleDir);

        if (!Directory.Exists(bundleDir))
            Directory.CreateDirectory(bundleDir);
        else
        {
            Directory.Delete(bundleDir, true);
            Directory.CreateDirectory(bundleDir);
        }

        BuildPipeline.BuildAssetBundles(bundleDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }
}