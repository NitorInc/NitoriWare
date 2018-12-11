using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaServeFood : MonoBehaviour
{
    [SerializeField]
    private bool isTray;

    [Header("Values for on tray")]
    [SerializeField]
    private float ySeparation;
    [SerializeField]
    private float yLerpSpeed;

    [Header("Values for launch")]
    [SerializeField]
    private Vector2 xSpeedRange;
    [SerializeField]
    private Vector2 ySpeedRange;
    [SerializeField]
    private Vector2 rotSpeedRange;

    private bool onTray = true;
    public bool OnTray => onTray;
    private MystiaServeCustomer customer;
    public MystiaServeCustomer Customer
    {
        get { return customer; }
        set { customer = value; GetComponent<SpriteRenderer>().sprite = customer.Data.FoodSprite; }
    }

    Vector3 GoalPosition => Vector3.up * GoalY;
    float GoalY => ySeparation * transform.GetSiblingIndex();

    Rigidbody2D rigidBoi;

    void Start ()
    {
        transform.localPosition = GoalPosition;
        rigidBoi = GetComponent<Rigidbody2D>();
        if (!isTray)
            transform.position += Vector3.forward * transform.GetSiblingIndex() * .01f;
    }
	
	void Update ()
    {
        if (!isTray && onTray)
        {
            //Snap to goal Y (for when food is taken away)
		    if (!MathHelper.Approximately(transform.localPosition.y, GoalY, .001f))
            {
                transform.moveTowardsLocal2D(GoalPosition, yLerpSpeed);
            }
        }

	}

    public void serve()
    {
        onTray = false;
        transform.parent = null;
        gameObject.SetActive(false);
    }

    public void launch()
    {
        onTray = false;
        var direction = Mathf.Sign(transform.root.localScale.x);
        rigidBoi.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        rigidBoi.velocity = new Vector2(
            MathHelper.randomRangeFromVector(xSpeedRange) * direction,
            MathHelper.randomRangeFromVector(ySpeedRange));
        rigidBoi.angularVelocity = MathHelper.randomRangeFromVector(rotSpeedRange * direction);
        if (isTray)
            GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTray)
        {
            enabled = false;
            return;
        }

        var floorTag = "MicrogameTag2";
        if (collision.collider.tag.Equals(floorTag)
            || collision.otherCollider.tag.Equals(floorTag))
        {
            rigidBoi.bodyType = RigidbodyType2D.Static;
            enabled = false;
        }
    }
}
