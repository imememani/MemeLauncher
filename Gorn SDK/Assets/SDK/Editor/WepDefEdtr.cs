using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponDefinition))]
public class WepDefEdtr: Editor
{
    public override void OnInspectorGUI()
    {
        WeaponDefinition wepDef = (WeaponDefinition)target;

        wepDef.name = (string.IsNullOrEmpty(wepDef.ID)) ? "My Weapon" : wepDef.ID;

        base.OnInspectorGUI();
    }
}
