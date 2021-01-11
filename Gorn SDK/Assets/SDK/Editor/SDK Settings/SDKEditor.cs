using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[InitializeOnLoad]
public class SDKEditor : EditorWindow
{
    private static SDKEditor instance;

    private static string settingsFile => Path.Combine(Application.dataPath, "SDK", "Editor", "SDK Settings", "Settings.sdk");

    public static Settings settings;

    public static string CustomArmorFolder
    {
        get
        {
            string path = Path.Combine(settings.gornFolder ?? "", "Mods/Custom Armor Framework/Custom Armor");

            if (!Directory.Exists(path))
                return null;

            return path;
        }
    }
    public static string CustomWeaponFolder
    {
        get
        {
            string path = Path.Combine(settings.gornFolder ?? "", "Mods/MemesWeaponExtender/Custom Weapons");

            if (!Directory.Exists(path))
                return null;

            return path;
        }
    }
    public static string CustomLevelFolder
    {
        get
        {
            string path = Path.Combine(settings.gornFolder ?? "", "Mods/MemesLevelExtender/Custom Levels");

            if (!Directory.Exists(path))
                return null;

            return path;
        }
    }

    [MenuItem("GORN SDK/Settings")]
    public static void GetWindow()
    {
        SDKEditor sDKEditor = GetWindow<SDKEditor>("SDK Settings");
        sDKEditor.Initialize();
        sDKEditor.Show();

        instance = sDKEditor;
    }

    static SDKEditor()
    {
        if (!File.Exists(settingsFile))
        {
            GetWindow();
        }
        else
        {
            Load();

            if (!Directory.Exists(settings.gornFolder))
            {
                GetWindow();
            }
        }
    }

    private void Initialize()
    {
        if (!File.Exists(settingsFile))
        {
            File.Create(settingsFile).Close();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void OnGUI()
    {
        if (settings == null)
        {
            Load();
            LabelField("Loading...");
        }
        else
        {
            BeginVertical(EditorStyles.helpBox);

            BeginHorizontal();
            settings.gornFolder = TextField("GORN Folder:", settings.gornFolder);
            if (GUILayout.Button("Select", GUILayout.Width(100)))
            {
                string selected = EditorUtility.OpenFolderPanel("CAF", "", "");
                settings.gornFolder = !string.IsNullOrEmpty(selected) ? selected : settings.gornFolder;
            }

            EndHorizontal();

            Space();
            Space();
            Space();
            Space();

            if (GUILayout.Button("Save And Close"))
            {
                Save();
                Close();
            }

            EndVertical();
        }
    }

    private static void Load()
    {
        settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFile)) ?? new Settings();
    }

    private static void Save()
    {
        File.WriteAllText(settingsFile, JsonConvert.SerializeObject(settings, Formatting.Indented));
    }

    public class Settings
    {
        public string gornFolder;
    }
}