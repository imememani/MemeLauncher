namespace CustomArmorFramework
{
    public enum MeshSupport
    {
        Gornie,
        Goliath,
        Mitch,
        Custom
    }

    public enum ArmorSide
    {
        Both,
        Left,
        Right,
        None
    }

    public enum ArmorSlot
    {
        /// <summary>
        /// This sits on the face of the gornie.
        /// </summary>
        Helmat,
        /// <summary>
        /// This sits on the chest of the gornie.
        /// </summary>
        Chestplate,
        /// <summary>
        /// This sits upon the waist of the gornie.
        /// </summary>
        Belt,
        /// <summary>
        /// This sits upon the left foot.
        /// </summary>
        LeftFoot,
        /// <summary>
        /// This sits upon the left shin.
        /// </summary>
        LeftShin,
        /// <summary>
        /// This sits upon the left thigh.
        /// </summary>
        LeftThigh,
        /// <summary>
        /// This sits upon the left hand.
        /// </summary>
        LeftHand,
        /// <summary>
        /// This sits upon the forearm.
        /// </summary>
        LeftLowerBracer,
        /// <summary>
        /// This sits upon the upper arm.
        /// </summary>
        LeftUpperBracer,
        /// <summary>
        /// This sits upon the left foot.
        /// </summary>
        RightFoot,
        /// <summary>
        /// This sits upon the Right shin.
        /// </summary>
        RightShin,
        /// <summary>
        /// This sits upon the Right thigh.
        /// </summary>
        RightThigh,
        /// <summary>
        /// This sits upon the Right hand.
        /// </summary>
        RightHand,
        /// <summary>
        /// This sits upon the forearm.
        /// </summary>
        RightLowerBracer,
        /// <summary>
        /// This sits upon the upper arm.
        /// </summary>
        RightUpperBracer,
    }

    public enum ArmorType
    {
        /// <summary>
        /// This means the armor piece will actually act like armor on the gornie.
        /// </summary>
        NoneCosmetic,
        /// <summary>
        /// This means the armor piece is purely cosmetic and will do nothing when hit.
        /// </summary>
        Cosmetic
    }
}