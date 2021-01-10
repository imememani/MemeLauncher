using CustomArmorFramework;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(ArmorDefenition))]
public class ArmorBuilder : Editor
{
    [MenuItem("GameObject/Armor Framework/Create Armor Piece", false, -1)]
    public static void CreateNewPiece()
    {
        Selection.activeObject = new GameObject("Armor Piece").AddComponent<ArmorDefenition>();
    }

    private ArmorDefenition def;
    private GUIStyle headerLabel;

    private SlotReference dummy;

    public override void OnInspectorGUI()
    {
        headerLabel = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 14
        };

        def = (ArmorDefenition)target;

        GetOrLoadDummy();
        AlignPieceToMesh();

        BeginVertical(EditorStyles.helpBox);

        DrawLine("Options");
        DrawOptions();

        if (def.type != ArmorType.Cosmetic)
        {
            DrawLine();

            DrawLine("Statistics");
            DrawStats();
        }

        DrawLine();

        DrawLine("Export");

        BeginHorizontal(EditorStyles.toolbar);
        DrawExport();
        EndHorizontal();

        EndVertical();

        HelpBox("Hover over the settings to see what they do!", MessageType.Info);
        EnsureSettingsDontConflict();
    }

    private void DrawOptions()
    {
        def.suppportedMesh = (MeshSupport)EnumPopup(new GUIContent("Supported Mesh", "Which type of enemy does this support?"), def.suppportedMesh);

        Space();

        def.slot = (ArmorSlot)EnumPopup(new GUIContent("Mesh Slot", "Which part of the enemy mesh does this piece of armor sit upon?"), def.slot);
        def.type = (ArmorType)EnumPopup(new GUIContent("Armor Type", "Is this armor piece cosmetic (damagable/acts like armor) or nonecosmetic (just for looks)?"), def.type);

        Space();

        def.side = (ArmorSide)EnumPopup(new GUIContent("Armor Side", "Which side is this armor for?"), def.side);

        if (def.side == ArmorSide.Both)
        {
            def.createdSide = (ArmorSide)EnumPopup(new GUIContent("Original Side", "The side this piece of armor was originally created on."), def.createdSide);

            if (def.createdSide == ArmorSide.Both)
            {
                def.createdSide = ArmorSide.Left;
            }
        }
    }

    private void DrawStats()
    {
        def.armorHealth = FloatField(new GUIContent("Health", "How much health this piece has before it breaks."), def.armorHealth);
        def.weight = Slider(new GUIContent("Weight", "How heavy is this piece of armor?"), def.weight, 0.1f, 250);
        def.spawnChance = IntSlider(new GUIContent("Spawn Chance", "How likely is this piece of armor to spawn?"), def.spawnChance, 1, 100);
    }

    private void DrawExport()
    {
        if (GUILayout.Button("Export", EditorStyles.toolbarButton))
        {
            SavePrefab();

            BundleUtilities.ExportSelectedBundles(out string output, "armor", PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(target));
            def.BuildManifest().ExportManifest(Path.Combine(output, $"{def.name.ToLowerInvariant().Replace(" ", "")}.manifest"));
            BundleUtilities.GroupFilesToFolder(output, Path.Combine(output, def.name), def.name.Replace(" ", ""));
        }
        if (GUILayout.Button("Save Prefab", EditorStyles.toolbarButton))
        {
            SavePrefab();
        }
    }

    private void DrawLine(string header = "")
    {
        BeginVertical(EditorStyles.helpBox);
        if (!string.IsNullOrEmpty(header)) LabelField(header, headerLabel);
        EndVertical();
    }

    private void GetOrLoadDummy()
    {
        if (dummy != null && def.suppportedMesh == dummy.meshType)
        {
            return;
        }

        dummy = FindObjectsOfType<SlotReference>().First(t => t.meshType == def.suppportedMesh);

        if (dummy == null)
        {
            switch (def.suppportedMesh)
            {
                case MeshSupport.Gornie:
                    dummy = Instantiate(AssetDatabase.LoadAssetAtPath<Transform>("SDK/0ArmorBuilder/Data/Gornie.prefab")).GetComponent<SlotReference>();
                    break;

                    /// Others will be here when I or someone else implements them.
            }
        }

        dummy.transform.position = Vector3.zero;
        dummy.transform.rotation = Quaternion.identity;
    }

    private void AlignPieceToMesh()
    {
        Transform slot = dummy.GetSlot(def.slot).slotObj;

        def.transform.position = slot.TransformPoint(Vector3.zero);
        def.transform.rotation = Quaternion.identity * slot.localRotation;
    }

    private void EnsureSettingsDontConflict()
    {
        if ((def.slot == ArmorSlot.Belt || def.slot == ArmorSlot.Helmat || def.slot == ArmorSlot.Chestplate) && def.side != ArmorSide.None)
        {
            def.side = ArmorSide.None;
        }

        def.armorHealth = Mathf.Clamp(def.armorHealth, 0, Mathf.Infinity);

        DrawLine("Messages");
        int messageCount = 0;

        if (def.type == ArmorType.NoneCosmetic && def.GetComponentsInChildren<Collider>().Length == 0)
        {
            HelpBox("Your piece is None-Cosmetic but it contains no colliders, this will make it cosmetic.", MessageType.Warning);
            messageCount++;
        }

        if (def.spawnChance < 10)
        {
            HelpBox("Your armor piece has a very rare spawn chance, is this intentional?", MessageType.Warning);
            messageCount++;
        }

        if (messageCount == 0)
        {
            HelpBox("Everything looks good! No warnings/errors!", MessageType.Info);
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
}