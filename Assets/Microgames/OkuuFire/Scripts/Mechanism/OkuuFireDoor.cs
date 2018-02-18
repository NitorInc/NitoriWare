using UnityEngine;

public class OkuuFireDoor : MonoBehaviour, IOkuuFireMechanism
{
    // How far the door can open.
    public float reach;

    private Vector3 startPosition;
    private Vector3 endPosition;
    
    void Awake()
    {
        this.startPosition = transform.localPosition;

        this.endPosition = this.startPosition;
        this.endPosition.x += this.reach;
    }
	
    public void Move(float completion)
    {
        float move = Mathf.Abs(this.reach * completion);

        Vector3 newPosition = Vector3.MoveTowards(this.startPosition, this.endPosition, move);
        this.transform.localPosition = newPosition;
    }
}
