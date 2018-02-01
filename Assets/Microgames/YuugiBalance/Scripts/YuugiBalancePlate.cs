using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuugiBalancePlate : MonoBehaviour
{

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private SpriteRenderer background;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Vector3 offset;
    private float z = 0,
        target_x = 0,
        sin = Mathf.PI / 2f,
        sine_speed = 2f,
        max_rotation = 80,
        fail_distance = 2,
        fall = 0,
        distance = 0,
        delay = 0,
        mad = 0;
    [SerializeField]
    private float difficulty = 1;
    private Vector3 deathposition;
    void Start()
    {
        //randomize if plate will start rotating left or right
        if (Random.value > 0.5f)
            sin = Mathf.PI * 3 / 2f;
    }

    void LateUpdate()
    {
        //run the failed update if failed
        if (!MicrogameController.instance.getVictory())
        {
            Failed();
            return;
        }

        //update where the plate should be in X (affected by sine function and position difference from player)
        //the delay is used to prevent the plate from falling too fast at the start
        delay = Mathf.Clamp(delay + Time.deltaTime * difficulty / 2f, 0, 1);
        target_x += (Mathf.Sin(sin += Time.deltaTime * sine_speed) / 5f
            - distance * delay * difficulty) * Time.deltaTime;

        //calculate distance and rotate plate
        distance = player.position.x - target_x;
        z = distance * max_rotation / fail_distance;

        //set the animator value
        anim.SetFloat("emotion", Mathf.Abs(distance / fail_distance));

        //set position and rotation
        transform.position = player.transform.position + offset;
        transform.eulerAngles = Vector3.forward * z;

        //check for failure
        if (Mathf.Abs(distance) > fail_distance)
        {
            MicrogameController.instance.setVictory(false, true);

            //death animation
            anim.SetBool("rip", true);

            //set death pos
            deathposition = player.transform.position;

            //remove controls
            Destroy(player.GetComponent<YuugiBalancePlayer>());
        }
    }

    void Failed()
    {
        //keep rotating after a fail
        z += Time.deltaTime * distance * 60;

        //assign rotation
        transform.eulerAngles = Vector3.forward * z;

        //falling
        transform.position += Vector3.down * Time.deltaTime * (fall += Time.deltaTime * 10)
            - Vector3.right * distance * Time.deltaTime;

        //zoom
        //Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, deathposition - Vector3.forward * 10, Time.deltaTime);
        //Camera.main.orthographicSize = 5 - Mathf.Clamp((mad += Time.deltaTime) - 1, 0, 3);

        //shake
        player.transform.position = deathposition
            + new Vector3(Random.value * 2 - 1, Random.value * -1)
            * Mathf.Clamp((mad += Time.deltaTime) - 0.5f, 0, 1) * 0.2f;
        background.color = Color.Lerp(Color.white, Color.red, mad);
    }
}
