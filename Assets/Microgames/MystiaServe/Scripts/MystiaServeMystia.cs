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
    private MystiaServeCustomerManager customerManager;

    [SerializeField]
    private Sprite debugFoodSprite;

    private float speed;
    bool launched;
    bool flipped = false;
    private Queue<MystiaServeCustomer> activeCustomers;
    private int customersLeft;
    private MystiaServeFoodManager foodManager;

    void Start()
    {
        var launchDelay = MathHelper.randomRangeFromVector(launchDelayRange);
        Invoke("launch", launchDelay);
        Invoke("showIcon", launchDelay - notifyIconDuration);
        speed = MathHelper.randomRangeFromVector(speedRange);
        customersLeft = customerManager.Customers.Length;
        activeCustomers = new Queue<MystiaServeCustomer>();

        if (canSwitchSides && MathHelper.randomBool())
        {
            flipped = true;
            flipSide(transform);
            flipSide(notifyIcon.transform);
            speed = -speed;
            customerManager.reverseCustomerPositions();
        }

        foodManager = GetComponent<MystiaServeFoodManager>();
        foodManager.createFood(customerManager.Customers);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("MicrogameTag1"))
        {
            activeCustomers.Enqueue(collision.GetComponent<MystiaServeCustomer>());
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
                    foodManager.serveNextCustomer();
                    
                    var customer = activeCustomers.Dequeue();
                    customer.GetComponent<Collider2D>().enabled = false;
                    customer.gameObject.SetActive(false);   //Debug
                    rigAnimator.SetTrigger("Serve");

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
