using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WeaponDefinition: MonoBehaviour
{
    public string ID;
    public string AssetBundleName;

    public WeaponManifest BuildManifest()
    {
        Damager[] damagers = GetComponentsInChildren<Damager>();
        Handle[] handles = GetComponentsInChildren<Handle>();

        return new WeaponManifest
        {
            ID = ID,
            BundleID = AssetBundleName,
            Handles = handles.Select(h => h.GetAsHandleGroup()).ToArray(),
            Damagers = damagers.Select(d => d.GetAsDamagerGroup()).ToArray()
        };
    }
}

public class WeaponManifest
{
    public string ID { get; set; }

    public string BundleID { get; set; }

    public HandleGroup[] Handles { get; set; }

    public DamagerGroup[] Damagers { get; set; }
}