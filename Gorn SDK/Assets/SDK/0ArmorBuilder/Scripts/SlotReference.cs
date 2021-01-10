using UnityEngine;

namespace CustomArmorFramework
{
    public class SlotReference : MonoBehaviour
    {
        public MeshSupport meshType;
        public SlotGroup[] slots;

        public SlotGroup GetSlot(ArmorSlot slot)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].slot == slot)
                    return slots[i];
            }

            return default;
        }

        [System.Serializable]
        public struct SlotGroup
        {
            public ArmorSlot slot;
            public Transform slotObj;
        }
    }
}