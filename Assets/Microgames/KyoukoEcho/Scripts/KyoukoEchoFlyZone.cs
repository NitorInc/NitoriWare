using UnityEngine;

public class KyoukoEchoFlyZone : MonoBehaviour
{

    [SerializeField]
    Transform upperBound;
    [SerializeField]
    Transform lowerBound;
    
    public float UpperBoundY
    {
        get { return upperBound.position.y; }
    }

    public float LowerBoundY
    {
        get { return lowerBound.position.y; }
    }

}
