using UnityEngine;
using UnityEngine.Events;

public class HecShapesHolder : MonoBehaviour
{

    public UnityEvent onFill;
    public Transform snapPoint;

    public HecShapesSlottable.Shape SlotShape { get; set; }

    HecShapesSlottable.Shape shapeInSlot;
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

    public Vector2 SnapPosition => snapPoint.transform.position;
    public bool Filled => shapeInSlot != HecShapesSlottable.Shape.none;
    public bool Valid => shapeInSlot == SlotShape; 

}
