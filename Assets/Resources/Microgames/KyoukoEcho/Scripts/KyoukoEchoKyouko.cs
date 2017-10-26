using UnityEngine;

public class KyoukoEchoKyouko : MonoBehaviour
{
    [SerializeField]
    int echoCount;

    [SerializeField]
    int missLimit = 1;
    int misses;
    int hits;

    [SerializeField]
    float boundTop;
    [SerializeField]
    float boundBottom;

    bool willEcho = true;

    Rigidbody2D rigidBody;
    Animator animator;
    
    // Use this for initialization
    void Start()
    {
        this.animator = this.GetComponent<Animator>();
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

    public float BoundTop
    {
        get { return this.boundTop; }
    }

    public float BoundBottom
    {
        get { return this.boundBottom; }
    }

}
