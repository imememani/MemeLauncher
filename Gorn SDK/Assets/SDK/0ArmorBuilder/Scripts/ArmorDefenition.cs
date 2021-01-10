using Newtonsoft.Json;
using UnityEngine;

namespace CustomArmorFramework
{
    public class ArmorDefenition : MonoBehaviour
    {
        public MeshSupport suppportedMesh;

        public ArmorSlot slot;
        public ArmorSide side;
        public ArmorSide createdSide = ArmorSide.Left;
        public ArmorType type;
        public float armorHealth = 5;
        public float weight = 1;
        public int spawnChance = 35;

        public string BuildManifest()
        {
            ArmorInformation info = new ArmorInformation(transform.name, slot, type, side, createdSide, armorHealth, weight, spawnChance);

            return JsonConvert.SerializeObject(info, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}