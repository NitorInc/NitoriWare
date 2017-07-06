using UnityEngine;

public class OkuuFireCrank : MonoBehaviour, IOkuuFireMechanism
{
    // Number of complete rotations possible.
    public float rotations;
    
    private float reach;

    void Start()
    {
        this.reach = 360 * this.rotations;
    }
    
    public void Move(float completion)
    {
        float angle = this.reach * completion;
        
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
