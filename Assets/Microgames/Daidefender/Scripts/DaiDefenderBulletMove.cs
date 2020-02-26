using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaiDefenderBulletMove : MonoBehaviour {

    [Header("The thing to fly towards")]
    [SerializeField]
    private GameObject target;

    [Header("BulletSpeed")]
    [SerializeField]
    private float speed;

    [Header("Firing delay in seconds")]
    [SerializeField]
    private float delay;

    [SerializeField]
    private float deflectAngleRange = 30f;
    [SerializeField]
    private float deflectSpeedMult = 1.5f;

    private Vector2 trajectory;
    bool hit = false;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("Dai");
        Invoke("SetTrajectory", delay);
       // var projectile = GameObject.FindGameObjectsWithTag("Bullet");
        /* foreach (var bullet in projectile)
         {
             if (bullet.GetComponent<Collider> == GetComponent<collider>));
                 Physics.IgnoreCollision(bullet.GetComponent<collider>, GetComponent<collider>);
         } */
        Physics2D.IgnoreCollision(this.gameObject.GetComponent<CircleCollider2D>(), this.gameObject.GetComponent<CircleCollider2D>());
        //this works only for prefab, the bullets are probably identified as seperate gameobjects. Needs something to tie all bullets together.

    }
    //http://forum.brackeys.com/thread/ignoring-collision-between-colliders/

    // Update is called once per frame
    void Update () {

        if (trajectory != null)
        {
            //Move the bullet a certain distance based on trajectory speed and time
            Vector2 newPosistion = (Vector2)transform.position + (trajectory * speed * Time.deltaTime);
            transform.position = newPosistion;
        }
        



    }
    void SetTrajectory()
    {
        // Calculate a trajectory towards the target
        trajectory = (target.transform.position - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit bullet detected");
        // Destroy(collision.gameObject);
        //this.gameObject.SetActive(false);
        if (collision.name.ToLower().Contains("cirno"))
        {
            var addAngle = 180f + (Random.Range(-deflectAngleRange, deflectAngleRange));
            trajectory = MathHelper.getVector2FromAngle(
                trajectory.getAngle() + addAngle,
                trajectory.magnitude);
            speed *= deflectSpeedMult;
            transform.eulerAngles += Vector3.forward * addAngle;
        }
        else
            gameObject.SetActive(false);
    }
    }

/*Previous setting
 * 0.5 sec spacing
 * speed 9
 * distance 8
 * 
 * Current setting
 * speed 10
 * every bullet the repeat same spot to 2 sq
 * delay -0.5
 * 
 * Game Last for 7 second
 * animation should last no more than 1-2 seconds.*/

