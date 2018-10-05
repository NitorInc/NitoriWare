using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaServeMystia : MonoBehaviour
{
    [SerializeField]
    private Vector2 launchDelayRange;
    [SerializeField]
    private Vector2 speedRange;
    [SerializeField]
    private bool canSwitchSides;
    [SerializeField]
    private GameObject notifyIcon;
    [SerializeField]
    private float notifyIconDuration;

    private float speed;
    bool launched;

    void Start()
    {
        var launchDelay = MathHelper.randomRangeFromVector(launchDelayRange);
        Invoke("launch", launchDelay);
        Invoke("showIcon", launchDelay - notifyIconDuration);
        speed = MathHelper.randomRangeFromVector(speedRange);

        if (canSwitchSides && MathHelper.randomBool())
        {
            flipSide(transform);
            flipSide(notifyIcon.transform);
            speed = -speed;
        }

    }

    void Update()
    {
        if (launched)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
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
