using Newtonsoft.Json;
using UnityEngine;

namespace CustomArmorFramework
{
    public class ArmorDefenition : MonoBehaviour
    {
        public string armorSetName = "";
        public MeshSupport suppportedMesh = MeshSupport.Gornie;

        public ArmorSlot slot = ArmorSlot.Helmat;
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