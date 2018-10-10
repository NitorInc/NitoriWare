using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MystiaServeMystia : MonoBehaviour
{
    [SerializeField]
    private Vector2 launchDelayRange;
    [SerializeField]
    private Vector2 speedRange;
    [SerializeField]
    private bool canSwitchSides;
    [SerializeField]
    private float notifyIconDuration;

    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private GameObject notifyIcon;
    [SerializeField]
    private Transform customerContainer;

    private float speed;
    bool launched;
    private Queue<Collider2D> activeCustomers;
    private int customersLeft;

    void Start()
    {
        var launchDelay = MathHelper.randomRangeFromVector(launchDelayRange);
        Invoke("launch", launchDelay);
        Invoke("showIcon", launchDelay - notifyIconDuration);
        speed = MathHelper.randomRangeFromVector(speedRange);
        activeCustomers = new Queue<Collider2D>();
        customersLeft = customerContainer.childCount;

        if (canSwitchSides && MathHelper.randomBool())
        {
            flipSide(transform);
            flipSide(notifyIcon.transform);
            speed = -speed;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("MicrogameTag1"))
        {
            activeCustomers.Enqueue(collision);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("MicrogameTag1") && activeCustomers.Any() && collision.enabled)
        {
            setVictory(false);
        }
    }
    void Update()
    {
        if (launched)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && !MicrogameController.instance.getVictoryDetermined())
            {
                if (activeCustomers.Any())
                {
                    var customer = activeCustomers.Dequeue();
                    customer.enabled = false;
                    customer.gameObject.SetActive(false);

                    customersLeft--;
                    if (customersLeft <= 0)
                        setVictory(true);
                }
                else
                setVictory(false);
            }
        }
    }

    void setVictory(bool victory)
    {
        MicrogameController.instance.setVictory(victory);
        if (!victory)
        {
            rigAnimator.SetTrigger("Fail");
            enabled = false;
        }
    }


    void launch()
    {
        launched = true;
        notifyIcon.SetActive(false);
    }

    void showIcon()
    {
        notifyIcon.SetActive(true);
    }

    void flipSide(Transform transform)
    {
        transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}
