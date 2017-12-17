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
        set { this.slotShape = value; }
    }

    public HecShapesSlottable.Shape ShapeInSlot
    {
        get { return this.shapeInSlot; }

        set
        {
            this.shapeInSlot = value;
            if (value != HecShapesSlottable.Shape.none)
                onFill.Invoke();
        }
    }

    public Vector2 SnapPosition
    {
        get { return this.snapPoint.transform.position; }
    }

    public bool Filled
    {
        get { return this.shapeInSlot != HecShapesSlottable.Shape.none; }
    }

    public bool Valid
    {
        get { return this.shapeInSlot == this.slotShape; }
    }

}
