using UnityEngine;

public class Handle: MonoBehaviour
{
    [Header("Handle Options")]
    public bool HideHandOnGrab = true;
    public bool SetRotationOnGrab = true;
    public bool SetPositionOnGrab = true;
    public bool CanGrab = true;
    [Space]
    [Header("Grab Orientation")]
    public Transform OrientationReference;

    public HandleGroup GetAsHandleGroup()
    {
        return new HandleGroup
        {
            TransformName = transform.name,
            HideHandOnGrab = HideHandOnGrab,
            SetRotationOnGrab = SetRotationOnGrab,
            SetPositionOnGrab = SetPositionOnGrab,
            CanGrab = CanGrab,
            OrientationReferenceTransformName = (OrientationReference == null) ? "" : OrientationReference.name
        };
    }
}

public class HandleGroup
{
    public string TransformName;

    public bool HideHandOnGrab = true;
    public bool SetRotationOnGrab = true;
    public bool SetPositionOnGrab = true;
    public bool CanGrab = true;
    public string OrientationReferenceTransformName;
}