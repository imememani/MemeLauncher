using Newtonsoft.Json;

namespace CustomArmorFramework
{
    /// <summary>
    /// Stores information about a piece of armor.
    /// </summary>
    public readonly struct ArmorInformation
    {
        public string AssetID { get; }

        /// <summary>
        /// Which position on the body does this slot fill?
        /// </summary>
        public ArmorSlot Slot { get; }

        /// <summary>
        /// What cosmetic type is this piece?
        /// </summary>
        public ArmorType Type { get; }

        /// <summary>
        /// Which side this piece resides on?
        /// </summary>
        public ArmorSide Side { get; }

        /// <summary>
        /// Which side was this piece created on?
        /// </summary>
        public ArmorSide CreatedSide { get; }

        /// <summary>
        /// How much health this piece currently has.
        /// </summary>
        public float ArmorHealth { get; }

        /// <summary>
        /// How heavy this armor piece is.
        /// </summary>
        public float Weight { get; }

        public int SpawnChance { get; }

        [JsonConstructor]
        public ArmorInformation(string assetID, ArmorSlot slot, ArmorType type, ArmorSide side, ArmorSide createdSide, float armorHealth, float weight, int spawnChance)
        {
            AssetID = assetID;

            Slot = slot;
            Type = type;
            Side = side;
            CreatedSide = createdSide;

            ArmorHealth = armorHealth;
            SpawnChance = spawnChance;
            Weight = weight;
        }
    }
}