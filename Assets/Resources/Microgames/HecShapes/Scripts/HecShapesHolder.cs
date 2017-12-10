using UnityEngine;

public class HecShapesHolder : MonoBehaviour
{

    [SerializeField]
    HecShapesHecatia hecatia;

    public HecShapesSlottable.Shape slotShape;
    public bool filled = false;

    public void FillSlot(HecShapesSlottable.Shape shape)
    {
        if (!this.filled)
        {
            this.filled = true;
            if (shape == this.slotShape)
                this.hecatia.Win();
            else
                this.hecatia.Lose();
        }
    }

    public void SetShape(HecShapesSlottable.Shape shape)
    {
        this.slotShape = shape;
    }

}
