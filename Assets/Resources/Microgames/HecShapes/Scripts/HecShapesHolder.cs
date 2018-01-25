using UnityEngine;
using UnityEngine.Events;

public class HecShapesHolder : MonoBehaviour
{

    public UnityEvent onFill;

    public Transform snapPoint;

    HecShapesSlottable.Shape slotShape;
    HecShapesSlottable.Shape shapeInSlot;

    public HecShapesSlottable.Shape SlotShape
    {
        set { slotShape = value; }
    }

    public HecShapesSlottable.Shape ShapeInSlot
    {
        get { return shapeInSlot; }

        set
        {
            shapeInSlot = value;
            if (value != HecShapesSlottable.Shape.none)
                onFill.Invoke();
        }
    }

    public Vector2 SnapPosition
    {
        get { return snapPoint.transform.position; }
    }

    public bool Filled
    {
        get { return shapeInSlot != HecShapesSlottable.Shape.none; }
    }

    public bool Valid
    {
        get { return shapeInSlot == slotShape; }
    }

}
