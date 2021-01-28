using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[InitializeOnLoad]
public class SDKEditor : EditorWindow
{
    private static SDKEditor instance;

    private static string SettingsFileLocation { get => Path.Combine(Application.dataPath, "SDK", "Editor", "SDK Settings", "Settings.sdk"); }

    public static Settings settings;

    /// <summary>
    /// ~/GORN
    /// </summary>
    public static string GORNFolder { get => settings.gornFolder; }

    /// <summary>
    /// GORN/Mods/Custom Armor Framework/Custom Armor
    /// </summary>
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

    /// <summary>
    /// GORN/Mods/MemesWeaponExtender/Custom Weapons (Planned to rename to: Custom Weapon Framework)
    /// </summary>
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

    /// <summary>
    /// GORN/Mods/Custom Level Framework/Custom Levels
    /// </summary>
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

    /// <summary>
    /// Is the provided GORN folder valid for use?
    /// </summary>
    public static bool IsGORNFolderValid { get => settings != null && Directory.Exists(settings.gornFolder); }

    /// <summary>
    /// Is the Custom Armor Framework installed?
    /// </summary>
    public static bool IsArmorExtenderInstalled { get => IsGORNFolderValid && Directory.Exists(CustomArmorFolder); }

    /// <summary>
    /// Is the Custom Weapons Framework installed?
    /// </summary>
    public static bool IsWeaponExtenderInstalled { get => IsGORNFolderValid && Directory.Exists(CustomWeaponFolder); }

    /// <summary>
    /// Is the Custom Levels Framework installed?
    /// </summary>
    public static bool IsLevelExtenderInstalled { get => IsGORNFolderValid && Directory.Exists(CustomLevelFolder); }

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
        if (!File.Exists(SettingsFileLocation))
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
        if (!File.Exists(SettingsFileLocation))
        {
            File.Create(SettingsFileLocation).Close();
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
        settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(SettingsFileLocation)) ?? new Settings();
    }

    private static void Save()
    {
        File.WriteAllText(SettingsFileLocation, JsonConvert.SerializeObject(settings, Formatting.Indented));
    }

    public class Settings
    {
        public string gornFolder;
    }
}