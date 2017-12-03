using UnityEngine;

public class HecShapesHolder : MonoBehaviour
{

    [SerializeField]
    HecShapesHecatia hecatia;

    public bool filled = false;

    public void FillSlot(bool correct)
    {
        if (!this.filled)
        {
            this.filled = true;
            if (correct)
                this.hecatia.Win();
            else
                this.hecatia.Lose();
        }
    }

}
