using UnityEngine;

public class OkuuFireCrank : MonoBehaviour, IOkuuFireMechanism
{
    // Number of complete rotations possible.
    public float rotations;
    
    private float maxAngle;
    
    void Start()
    {
        this.maxAngle = this.rotations * 360F;
    }
    
    public void Move(float completion)
    {
        float angle = this.maxAngle * completion;
        
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
