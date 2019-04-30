using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPlanePaperPlane : MonoBehaviour
{
    [SerializeField]
    float speed = 1f;
    [SerializeField]
    float turningSpeed = 0.1f;
    [SerializeField]
    float maxTurningSpeed = 0.5f;
    [SerializeField]
    float spriteChangeThreshold = 0.5f;
    [SerializeField]
    float delay = 0.5f;
    [SerializeField]
    Vector2 upperBounds = new Vector2(-7, 7);
    [SerializeField]
    Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    bool Control;
    Vector2 velocity;

    void Start()
    {
        Control = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.gameObject.SetActive(false);
        velocity = Vector2.zero;
        Invoke("setVelocity", delay);
	}
	
    void setVelocity()
    {
        spriteRenderer.gameObject.SetActive(true);
        velocity = new Vector2(speed, 0);
        spriteRenderer.sprite = sprites[1];
        Control = true;
    }

    private void AddVelocity(bool up)
    {
        if(up)
            velocity.y += turningSpeed * Time.deltaTime;
        else
            velocity.y += -turningSpeed * Time.deltaTime;
        clampSpeed();

    }

    void Update()
    {
        if (Control)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                AddVelocity(false);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                AddVelocity(true);
            }
            else
            {
                if (velocity.y < -1f)
                {
                    AddVelocity(true);
                }
                else if (velocity.y > 1f)
                {
                    AddVelocity(false);
                }
                else velocity.y = 0;
            }
            }
        transform.position += new Vector3(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0);
        if (transform.position.y < upperBounds.x) transform.position = new Vector3(transform.position.x, upperBounds.x, transform.position.z);
        else if (transform.position.y > upperBounds.y) transform.position = new Vector3(transform.position.x, upperBounds.y, transform.position.z);


        if (velocity.y > spriteChangeThreshold)
            spriteRenderer.sprite = sprites[0];

        else if (velocity.y < -spriteChangeThreshold)
            spriteRenderer.sprite = sprites[2];

        else
            spriteRenderer.sprite = sprites[1];

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "MicrogameTag1":
                MicrogameController.instance.setVictory(false, true);
                velocity = Vector2.zero;
                Control = false;
                break;
            case "MicrogameTag2":
                MicrogameController.instance.setVictory(true, true);
                velocity = Vector2.zero;
                Control = false;
                break;
            case "MicrogameTag3":
                MicrogameController.instance.setVictory(true, true);
                Control = false;
                StartCoroutine("WinTarget");
                break;
            default:
                break;
        }
    }

    public BoxCollider2D target;
    IEnumerable WinTarget()
    {
        float boundUp = target.transform.position.y + (target.size.y / 2);
        float boundDown = target.transform.position.y - (target.size.y / 2);
        while (velocity != Vector2.zero)
        {
            if (boundUp < transform.position.y)
            {
                AddVelocity(false);

            }
            else if (boundDown > transform.position.y)
            {
                AddVelocity(true);
            }
            else
            {
                if (velocity.y < -1f)
                {
                    AddVelocity(true);
                }
                else if (velocity.y > 1f)
                {
                    AddVelocity(false);
                }
                else velocity.y = 0;
            }
                yield return null;
        }
    }

    void clampSpeed()
    {
        if (velocity.y > maxTurningSpeed)
            velocity.y = maxTurningSpeed;
        if (velocity.y < -maxTurningSpeed)
            velocity.y = -maxTurningSpeed;
    }
}
