using UnityEngine;

public class KyoukoEchoKyouko : MonoBehaviour
{
    [SerializeField]
    int echoCount;

    [SerializeField]
    int missLimit = 1;
    int misses;
    int hits;
    
    bool willEcho = true;

    Rigidbody2D rigidBody;
    Animator animator;

    KyoukoEchoFlyZone flyZone;

    void Awake()
    {
        this.animator = GetComponent<Animator>();
        this.flyZone = FindObjectOfType<KyoukoEchoFlyZone>();
    }
    
    void LateUpdate()
    {
        Vector3 cursorPosition = CameraHelper.getCursorPosition();
        this.transform.position = new Vector2(this.transform.position.x, cursorPosition.y);
    }

    public void Hit(string partName)
    {
        // Body parts share names with animations
        this.animator.SetTrigger(partName);

        this.hits += 1;
        bool win = (this.hits + this.misses) >= echoCount;

        if (win)
        {
            // Win
            this.willEcho = false;
            MicrogameController.instance.setVictory(true, true);
        }
    }

    public void Miss()
    {
        this.misses += 1;
        bool lose = this.misses >= missLimit;

        if (lose)
        {
            // Lose
            this.willEcho = false;
            MicrogameController.instance.setVictory(false, true);

            this.animator.SetBool("Lose", true);
        }
    }

    public bool WillEcho
    {
        get { return this.willEcho; }
    }

    public float UpperBoundY
    {
        get { return this.flyZone.UpperBoundY; }
    }

    public float LowerBoundY
    {
        get { return this.flyZone.LowerBoundY; }
    }

}
