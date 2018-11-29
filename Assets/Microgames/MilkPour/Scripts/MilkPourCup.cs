using UnityEngine;

public class MilkPourCup : MonoBehaviour
{
    public virtual void AddFill (float amount) { }

    // Is the fill too large for the cup?
    public virtual bool IsFillMaxed ()
    { 
        return false;
    }

    // Is the fill above the max fill line?
    public virtual bool IsOverfilled()
    {
        return false;
    }

    // Is the fill between the min and max fill lines?
    public virtual bool IsFillReqMet ()
    { 
        return false;
    }

    // Stop the cup from growing.
    public virtual void Stop () { }
}
