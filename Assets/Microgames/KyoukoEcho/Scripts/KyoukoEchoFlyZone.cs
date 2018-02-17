using UnityEngine;

public class KyoukoEchoFlyZone : MonoBehaviour
{

    [SerializeField]
    Transform upperBound;
    [SerializeField]
    Transform lowerBound;
    
    public float UpperBoundY
    {
        get { return this.upperBound.position.y; }
    }

    public float LowerBoundY
    {
        get { return this.lowerBound.position.y; }
    }

}
