using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleKappaInteractive : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Collider2D _collider2D;
    [SerializeField]
    private ParticleSystem eatParticles;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float collideTime, lifeTime, clickSpeed;
    [SerializeField]
    private float minSpeed, maxSpeed, slowDownAcc, speedUpAcc;
    [SerializeField]
    private float eatGrowScale, explodeSpeed;
    [SerializeField]
    private Rigidbody2D _rigidBody;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip eatClip, explodeClip;
    [SerializeField]
    private TitleFloatingInteractive floatingInteractive;
#pragma warning restore 0649

    private Vector2 lastMousePosition, flingVelocity;
    private bool grabbed;
    private List<GameObject> eatenCucumbers;

    void Start()
    {
        grabbed = false;
        eatenCucumbers = new List<GameObject>();
    }

    void Update()
    {

        if (!grabbed)
        {
            if (_rigidBody.velocity == Vector2.zero)
                _rigidBody.velocity = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), .01f);

            float speed = _rigidBody.velocity.magnitude, diff = slowDownAcc * Time.deltaTime;
            if (speed > minSpeed)
                _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Max(speed - diff, minSpeed));
            else if (speed < minSpeed)
                _rigidBody.velocity = _rigidBody.velocity.resize(Mathf.Min(speed + diff, minSpeed));
        }
        else
        {
            Vector2 mousePosition = CameraHelper.getCursorPosition();
            if (mousePosition - lastMousePosition != Vector2.zero)
            {
                flingVelocity = (mousePosition - lastMousePosition) / Time.deltaTime;
                flingVelocity = flingVelocity.resize(Mathf.Clamp(flingVelocity.magnitude, minSpeed, maxSpeed));
            }
            lastMousePosition = mousePosition;
        }
        

        if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(_collider2D))
            click();
    }

    void click()
    {
        _rigidBody.velocity = ((Vector2)transform.position - (Vector2)CameraHelper.getCursorPosition()).resize(clickSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enabled && collision.collider.name.Contains("Cucumber"))
        {
            Vector3 holdScale = transform.localScale;
            transform.localScale = Vector3.one;
            var eatenCucumber = collision.collider.gameObject;
            eatenCucumbers.Add(eatenCucumber);
            eatenCucumber.transform.parent = transform;
            eatenCucumber.SetActive(false);
            transform.localScale = holdScale;


            transform.localScale *= eatGrowScale;
            sfxSource.PlayOneShot(eatClip);
            eatParticles.Play();

            animator.SetTrigger("Eat");
            int eatCount = animator.GetInteger("EatCount");
            animator.SetInteger("EatCount", eatCount + 1);
            if (eatCount >= 2)
            {
                _rigidBody.bodyType = RigidbodyType2D.Kinematic;
                _rigidBody.velocity = Vector2.zero;
                enabled = false;
            }
        }
    }

    public void explode()
    {
        Vector3 holdScale = transform.localScale;
        transform.localScale = Vector3.one;
        foreach (GameObject eatenCucumber in eatenCucumbers)
        {
            eatenCucumber.SetActive(true);
            eatenCucumber.transform.parent = transform.parent;
        }
        transform.localScale = holdScale;

        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform childTransform = transform.parent.GetChild(i);
            if (childTransform != transform)
                childTransform.GetComponent<Rigidbody2D>().velocity = ((Vector2)(childTransform.position - transform.position)).resize(explodeSpeed);
        }

        sfxSource.PlayOneShot(explodeClip);
        _collider2D.enabled = false;
        GetComponent<TrailRenderer>().time = 0f;
        Invoke("kill", .5f);
    }

    void kill()
    {
        Destroy(gameObject);
    }
}
