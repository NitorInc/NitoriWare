using UnityEngine;

public class MilkPourCup : MonoBehaviour
{
    public virtual void AddFill (float amount) { }

    public virtual bool IsOverfilled ()
    { 
        return false;
    }

    public virtual bool IsFillReqMet ()
    { 
        return false;
    }

    public virtual void Stop () { }
}
