using UnityEngine;

public class OkuuFireCrank : MonoBehaviour, IOkuuFireMechanism
{
    // Number of complete rotations possible.
    [Header("Number of times the crank can be rotated 360 degrees")]
    public float rotations;
    
    private float reach;

    void Start()
    {
        reach = 360 * rotations;
    }
    
    public void Move(float completion)
    {
        float angle = reach * completion;
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
