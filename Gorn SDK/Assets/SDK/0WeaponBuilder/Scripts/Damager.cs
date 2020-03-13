using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Damager: MonoBehaviour
{
    [Header("Damage Type")]
    public DamageType DamageType = DamageType.Cut;
    [Space]
    [Header("Damage Values")]
    public float MinumumDamageVelocity = 0.2f;
    [Space]
    public float DamageScale = 1;
    public float ArmorDamageBonus = 2;
    public float BonusVelocity = 0;
    [Space]
    [Header("Damage Options")]
    public bool CanDoDamage = true;
    public bool CanSlice = false;
    public bool IsHeavy = false;
    public bool IsFloorDamager = false;
    [Space]
    [Header("Stabbing Options")]
    public float ImpaleBreakForce = 500;
    public float ImapleDepth = 1;
    public bool StickOnDamage = false;
    public Transform HeartStabPoint;

    public DamagerGroup GetAsDamagerGroup()
    {
        return new DamagerGroup
        {
            TransformName = transform.name,
            DamageType = DamageType,
            MinumumDamageVelocity = MinumumDamageVelocity,
            DamageScale = DamageScale,
            ArmorDamageBonus = ArmorDamageBonus,
            BonusVelocity = BonusVelocity,
            CanDoDamage = CanDoDamage,
            CanSlice = CanSlice,
            IsHeavy = IsHeavy,
            IsFloorDamager = IsFloorDamager,
            ImpaleBreakForce = ImpaleBreakForce,
            ImapleDepth = ImapleDepth,
            StickOnDamage = StickOnDamage,
            HeartStabPointTransformName = (HeartStabPoint == null) ? "" : HeartStabPoint.transform.name
        };
    }
}

public class DamagerGroup
{
    public string TransformName;

    public DamageType DamageType = DamageType.Cut;
    public float MinumumDamageVelocity = 0.2f;
    public float DamageScale = 1;
    public float ArmorDamageBonus = 2;
    public float BonusVelocity = 0;
    public bool CanDoDamage = true;
    public bool CanSlice = false;
    public bool IsHeavy = false;
    public bool IsFloorDamager = false;
    public float ImpaleBreakForce = 500;
    public float ImapleDepth = 1;
    public bool StickOnDamage = false;
    public string HeartStabPointTransformName;
}

public enum DamageType
{
    Fist,
    Blunt,
    Arrow,
    Cut,
    Stab,
    Bleed,
    HeartRip,
    Bite
}