using CustomArmorFramework;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class MenuItems
{
    [MenuItem("GORN SDK/Open/GORN Folder", validate = true)]
    public static bool GORNInstalled() => SDKEditor.IsGORNFolderValid;

    [MenuItem("GORN SDK/Open/GORN Folder")]
    public static void OpenGORNFolder()
    {
        Process.Start(SDKEditor.GORNFolder);
    }

    [MenuItem("GORN SDK/Open/Armor Folder", validate = true)]
    public static bool ArmorInstalled() => SDKEditor.IsArmorExtenderInstalled;

    [MenuItem("GORN SDK/Open/Armor Folder")]
    public static void OpenArmorFolder()
    {
        Process.Start(SDKEditor.CustomArmorFolder);
    }

    [MenuItem("GORN SDK/Open/Weapons Folder", validate = true)]
    public static bool WeaponInstalled() => SDKEditor.IsWeaponExtenderInstalled;

    [MenuItem("GORN SDK/Open/Weapons Folder")]
    public static void OpenWeaponFolder()
    {
        Process.Start(SDKEditor.CustomWeaponFolder);
    }

    [MenuItem("GORN SDK/Open/Levels Folder", validate = true)]
    public static bool LevelsInstalled() => SDKEditor.IsLevelExtenderInstalled;

    [MenuItem("GORN SDK/Open/Levels Folder")]
    public static void OpenLevelsFolder()
    {
        Process.Start(SDKEditor.CustomLevelFolder);
    }

    [MenuItem("GameObject/Armor Framework/Create Armor Piece", false, -1)]
    public static void CreateNewPiece()
    {
        Selection.activeObject = new GameObject("Armor Piece").AddComponent<ArmorDefenition>();
    }

    [MenuItem("GameObject/GUI/New Window", false, -1)]
    public static void CreateNewWindow()
    {
        GameObject window = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine("Assets/SDK/0GUIBuilder/Data/Prefabs/Window Templates/New Window.prefab")));
        window.transform.SetParent(GameObject.Find("Design Canvas")?.transform ?? Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine("Assets/SDK/0GUIBuilder/Data/Prefabs/Design Canvas.prefab")).transform));

        window.name = "New Window";

        window.transform.localPosition = Vector3.zero;
        window.transform.localRotation = Quaternion.identity;
        window.transform.localScale = Vector3.one;

        Selection.activeObject = window;
    }

    [MenuItem("GameObject/GUI/New Settings Window", false, -1)]
    public static void CreateNewSettingsWindow()
    {
        GameObject window = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine("Assets/SDK/0GUIBuilder/Data/Prefabs/Window Templates/New Settings Window.prefab")));
        window.transform.SetParent(GameObject.Find("Design Canvas")?.transform ?? Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(Path.Combine("Assets/SDK/0GUIBuilder/Data/Prefabs/Design Canvas.prefab")).transform));

        window.name = "New Window";

        window.transform.localPosition = Vector3.zero;
        window.transform.localRotation = Quaternion.identity;
        window.transform.localScale = Vector3.one;

        Selection.activeObject = window;
    }
}