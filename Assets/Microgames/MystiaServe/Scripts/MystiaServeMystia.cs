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
    private AudioSource rollerbladeSource;
    [SerializeField]
    private AudioClip serveClip;

    [SerializeField]
    private Animator rigAnimator;
    [SerializeField]
    private GameObject notifyIcon;
    [SerializeField]
    private MystiaServeCustomerManager customerManager;

    [SerializeField]
    private Sprite debugFoodSprite;
    [SerializeField]
    private float earlyServeFailDelay = .1f;

    private float speed;
    bool launched;
    bool flipped = false;
    private Queue<MystiaServeCustomer> activeCustomers;
    private int customersLeft;
    private MystiaServeFoodManager foodManager;
    private bool servedEarly;

    void Start()
    {
        servedEarly = false;
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
        if (collision.tag.Equals("MicrogameTag1") && activeCustomers.Any() && collision.enabled && !servedEarly)
        {
            setVictory(false);
        }
    }
    void Update()
    {
        if (launched)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && !MicrogameController.instance.getVictoryDetermined() && !servedEarly)
            {
                if (activeCustomers.Any())
                {
                    foodManager.serveNextCustomer();
                    
                    var customer = activeCustomers.Dequeue();
                    customer.GetComponent<Collider2D>().enabled = false;
                    rigAnimator.SetTrigger("Serve");
                    MicrogameController.instance.playSFX(serveClip, panStereo: AudioHelper.getAudioPan(transform.position.x));

                    customersLeft--;
                    if (customersLeft <= 0)
                        setVictory(true);
                }
                else
                {
                    servedEarly = true;
                    rigAnimator.SetTrigger("Serve");
                    Invoke("setFail", earlyServeFailDelay);
                }
            }

            if (rollerbladeSource.isPlaying && CameraHelper.isObjectOffscreen(transform, 2f))
                rollerbladeSource.Stop();
        }
    }

    void setFail()
    {
        setVictory(false);
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
        rollerbladeSource.Play();
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
