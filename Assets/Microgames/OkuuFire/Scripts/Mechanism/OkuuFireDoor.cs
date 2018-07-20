using UnityEngine;

public class OkuuFireDoor : MonoBehaviour, IOkuuFireMechanism
{
    // How far the door can open.
    public float reach;

    private Vector3 startPosition;
    private Vector3 endPosition;
    
    void Awake()
    {
        startPosition = transform.localPosition;

        endPosition = startPosition;
        endPosition.x += reach;
    }
	
    public void Move(float completion)
    {
        float move = Mathf.Abs(reach * completion);

        Vector3 newPosition = Vector3.MoveTowards(startPosition, endPosition, move);
        transform.localPosition = newPosition;
    }
}
