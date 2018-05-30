using UnityEngine;

public class KyoukoEchoKyouko : MonoBehaviour
{
    [SerializeField]
    int missLimit = 1;

    int misses;
    int hits;
    int echoCount;
    bool willEcho = true;

    Rigidbody2D rigidBody;
    Animator animator;

    KyoukoEchoFlyZone flyZone;

    void Awake()
    {
        animator = GetComponent<Animator>();
        flyZone = FindObjectOfType<KyoukoEchoFlyZone>();
    }
    
    void LateUpdate()
    {
        Vector3 cursorPosition = CameraHelper.getCursorPosition();
        transform.position = new Vector2(transform.position.x, cursorPosition.y);
    }

    public void Hit(string partName)
    {
        // Body parts share names with animations
        animator.SetTrigger(partName);

        hits += 1;
        bool win = (hits + misses) >= echoCount;

        if (win)
        {
            // Win
            willEcho = false;
            MicrogameController.instance.setVictory(true, true);

            animator.SetTrigger("Win");
        }
    }

    public void Miss()
    {
        misses += 1;
        bool lose = misses >= missLimit;

        if (lose)
        {
            // Lose
            willEcho = false;
            MicrogameController.instance.setVictory(false, true);

            animator.SetTrigger("Lose");
        }
    }
    
    public void IncrementEchoCount() => echoCount++;
    public bool WillEcho => willEcho;
    public float UpperBoundY => flyZone.UpperBoundY; 
    public float LowerBoundY => flyZone.LowerBoundY; 

}
