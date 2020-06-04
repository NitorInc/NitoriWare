using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudPunchPiece : MonoBehaviour
{
    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private float animationDetectionCooldownTime = .15f;
    [SerializeField]
    private float heightPerPiece = 1.5f;
    [SerializeField]
    private bool isHead;
    [SerializeField]
    private float fallSpeed = 50f;
    [SerializeField]
    private float victorySoundDelay = .25f;

    public static bool awaitingPunch;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collide(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collide(collision);
    }

    void collide(Collider2D collision)
    {
        if (enabled
            && collision.tag.Equals("MicrogameTag1")
            && awaitingPunch)
        {
            // Ensure we're the closest piece vertically to the fist hitbox
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                var child = transform.parent.GetChild(i);
                if (child != transform
                    && !child.name.ToLower().Contains("head"))
                {
                    var distance = Mathf.Abs(transform.position.y - collision.transform.position.y);
                    var otherPieceDistance = Mathf.Abs(child.transform.position.y - collision.transform.position.y);
                    if (otherPieceDistance < distance)
                        return;
                }
            }


            // Swap animation movement offset of above pieces
            for (int i = transform.GetSiblingIndex() + 1; i < transform.parent.childCount; i++)
            {
                var otherPiece = transform.parent.GetChild(i).GetComponent<CloudPunchPiece>();
                if (!otherPiece.isHead)
                    otherPiece.rigAnimator.SetTrigger("Swap");
            }
            if (isHead)
            {
                MicrogameController.instance.setVictory(true);
                AudioHelper.playScheduled(GetComponent<AudioSource>(), victorySoundDelay);
            }

            rigAnimator.SetTrigger("Knock");
            awaitingPunch = false;
            //GetComponentInChildren<Collider2D>().enabled = false;
            transform.SetParent(null);
            enabled = false;
        }
    }

    private void Update()
    {
        var goalY = transform.GetSiblingIndex() * heightPerPiece;
        if (transform.localPosition.y != goalY)
        {
            var newY = Mathf.MoveTowards(transform.localPosition.y, goalY, Time.deltaTime * fallSpeed);
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }

    }
}
