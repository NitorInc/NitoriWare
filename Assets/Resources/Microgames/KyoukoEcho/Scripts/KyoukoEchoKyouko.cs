using System.Collections.Generic;
using UnityEngine;

public class KyoukoEchoKyouko : MonoBehaviour
{
    
    [SerializeField]
    float boundTop;
    [SerializeField]
    float boundBottom;

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
    }

    public void Miss()
    {
        this.animator.SetTrigger("Miss");
    }

    public float BoundTop
    {
        get
        {
            return this.boundTop;
        }
    }

    public float BoundBottom
    {
        get
        {
            return this.boundBottom;
        }
    }

}
