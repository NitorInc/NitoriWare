using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RumiaRescueRumiaController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float restrictMoveRangX = 5;
    [SerializeField]
    private float restrictMoveRangY = 4;
    [SerializeField]
    private RumiaRescueRescuerController rescuerController;


    private float moveX;
    private float moveY;
    private float moveDiff;
    private bool isFinished;
    private Animator ani;
    void Start() {
        isFinished = false;
        ani = GetComponent<Animator>();
        Assert.IsTrue(restrictMoveRangX > 0);
        Assert.IsTrue(restrictMoveRangY > 0);
    }


    private void FixedUpdate() {
        if (isFinished)
            return;

        moveX = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        moveY = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
    }

    private void Update() {
        if (isFinished)
            return;

        if (moveX != 0f || moveY != 0f) {
            Vector2 moveDirection = new Vector2(moveX, moveY);
            moveDiff = moveSpeed * Time.deltaTime * Time.timeScale;
            Vector3 newPosition = transform.position + (Vector3)(moveDirection * moveDiff);
            if (Mathf.Abs(newPosition.x) <= restrictMoveRangX && Mathf.Abs(newPosition.y) <= restrictMoveRangY) { 
                transform.position = newPosition;
                print(newPosition);
            }
        }
    }

    public void FinishRumiaMovement() {
        isFinished = true;
    }

    public void WhenGameVictory() {
        FinishRumiaMovement();
        ani.SetTrigger("Victory");
    }

    public void WhenRumiaHitTree() {
        rescuerController.ShutDownBlackBall();
        FinishRumiaMovement();
        ani.SetTrigger("Hit");
    }
}
