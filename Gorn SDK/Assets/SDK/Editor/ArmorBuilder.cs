using CustomArmorFramework;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

[CustomEditor(typeof(ArmorDefenition))]
public class ArmorBuilder : Editor
{
    private ArmorDefenition def;
    private GUIStyle headerLabel;

    private SlotReference dummy;

    private ArmorSlot lastSlot;

    private bool canBuild = true;

    private bool IsShowingBoundries { get => dummy.GetSlot(def.slot).slotObj.GetChild(0).gameObject.activeInHierarchy; }

    public override void OnInspectorGUI()
    {
        headerLabel = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 12
        };

        def = (ArmorDefenition)target;
        lastSlot = def.slot;

        GetOrLoadDummy();
        AlignPieceToMesh();

        BeginVertical(EditorStyles.helpBox);

        DrawLine("Name Your Armor");
        def.name = TextField(new GUIContent("Name", "The uh...Name of you armor piece?"), def.name);
        def.armorSetName = TextField(new GUIContent("Armor Set Name", "This is used to group all armor pieces with this name into the same folder for exporting."), def.armorSetName);

        DrawLine("Options");
        DrawOptions();

        DrawLine("Statistics");
        if (def.type != ArmorType.Cosmetic)
        {
            DrawStats();
        }
        def.spawnChance = IntSlider(new GUIContent("Spawn Chance", "How likely is this piece of armor to spawn?"), def.spawnChance, 1, 100);

        DrawLine();
        Space();
        Space();
        Space();
        Space();
        Space();

        DrawLine("Export");

        BeginHorizontal(EditorStyles.toolbar);
        DrawExport();
        EndHorizontal();

        EndVertical();

        HelpBox("Hover over the settings to see what they do!", MessageType.Info);

        Space();
        Space();
        Space();
        DrawLine();

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
    }

    private void DrawExport()
    {
        if (GUILayout.Button(!canBuild ? "Export (FIX ERRORS)" : "Export", EditorStyles.toolbarButton))
        {
            if (canBuild)
            {
                SavePrefab();

                BundleUtilities.ExportSelectedBundles(out string output, "armor", PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(target));
                def.BuildManifest().ExportManifest(Path.Combine(output, $"{def.name.ToLowerInvariant().Replace(" ", "")}.manifest"));

                if (string.IsNullOrEmpty(def.armorSetName))
                    BundleUtilities.GroupFilesToFolder(output, Path.Combine(SDKEditor.CustomArmorFolder, def.name), def.name.Replace(" ", ""));
                else
                {
                    BundleUtilities.GroupFilesToFolder(output, Path.Combine(SDKEditor.CustomArmorFolder, def.armorSetName, def.name), def.name.Replace(" ", ""));
                }

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
        if (GUILayout.Button(IsShowingBoundries ? "Hide Boundries" : "Show Boundries", EditorStyles.toolbarButton))
        {
            if (IsShowingBoundries)
            {
                SetBoundries(false, def.slot);
            }
            else
            {
                SetBoundries(true, def.slot);
            }
        }
    }

    private void OnDestroy()
    {
        SetBoundries(false, def.slot);
    }

    private void SetBoundries(bool state, ArmorSlot slot)
    {
        Transform slotObj = dummy.GetSlot(slot).slotObj;

        for (int i = 0; i < slotObj.childCount; i++)
        {
            slotObj.GetChild(i).gameObject.SetActive(state);
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

        if (lastSlot != def.slot)
        {
            SetBoundries(false, lastSlot);
        }

        DrawLine("Log");
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

        if (def.name == "Armor Piece")
        {
            HelpBox("Your piece of armor will be named the default 'Armor Piece' is this intentional?", MessageType.Warning);
            messageCount++;
        }

        if (string.IsNullOrEmpty(SDKEditor.settings.gornFolder))
        {
            HelpBox("You haven't selected a valid GORN folder.", MessageType.Error);
            canBuild = false;
            messageCount++;
        }

        if (string.IsNullOrEmpty(SDKEditor.CustomArmorFolder))
        {
            HelpBox("You haven't installed the custom armor framework.", MessageType.Error);
            canBuild = false;
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