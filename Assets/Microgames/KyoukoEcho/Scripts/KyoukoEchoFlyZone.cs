using UnityEngine;

public class KyoukoEchoFlyZone : MonoBehaviour
{

    [SerializeField]
    Transform upperBound;
    [SerializeField]
    Transform lowerBound;
    
    public float UpperBoundY => upperBound.position.y;
    public float LowerBoundY => lowerBound.position.y;

}
