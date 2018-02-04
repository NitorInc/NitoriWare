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
    private YuugiBalancePlayer player_script;
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
        mad = 0;
    [SerializeField]
    private float delay = 0, //delay in seconds before start
        difficulty = 2;
    private Vector3 deathposition;
    private bool success = false;
    [SerializeField]
    private ParticleSystem[] confetti;
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

        if (MicrogameTimer.instance.beatsLeft <= 0)
        {
            Succeed();
            return;
        }

        //update where the plate should be in X (affected by sine function and position difference from player)
        //the delay is used to prevent the plate from falling too fast at the start
        target_x += (Mathf.Sin(sin += Time.deltaTime * sine_speed) / 5f - distance)
            * Time.deltaTime * difficulty * Mathf.Clamp01(-(delay -= Time.deltaTime) - 1);

        //push if close to edge
        float max = player_script.max_horizontal - 1;
        if (Mathf.Abs(player.position.x) > max)
            target_x -= (player.position.x - max
                * Mathf.Sign(player.position.x)) * 0.3f;

        //calculate distance and rotate plate
        distance = player.position.x - target_x;
        z = distance * max_rotation / fail_distance;

        //set the animator value
        anim.SetFloat("emotion", Mathf.Abs(distance / fail_distance));

        //set position and rotation
        transform.position = player.position + offset;
        transform.eulerAngles = Vector3.forward * z;

        //set bg paralax position
        background.transform.position = Vector3.right * distance * -0.1f;

        //check for failure
        if (Mathf.Abs(distance) > fail_distance)
        {
            MicrogameController.instance.setVictory(false, true);

            //death animation
            anim.SetBool("rip", true);

            //set death pos
            deathposition = player.position;

            //remove controls
            Destroy(player.GetComponent<YuugiBalancePlayer>());
        }
    }

    void Succeed()
    {
        if (!success)
        {
            success = true;
            //confetti
            for (int i = 0; i < confetti.Length; i++)
                confetti[i].Emit(30);
            //happy animation
            anim.SetBool("yay", true);
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
        transform.position += Vector3.down * Time.deltaTime * (fall += Time.deltaTime * 70)
            - Vector3.right * distance * Time.deltaTime;

        //zoom
        //Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, deathposition - Vector3.forward * 10, Time.deltaTime);
        //Camera.main.orthographicSize = 5 - Mathf.Clamp((mad += Time.deltaTime) - 1, 0, 3);

        //shake
        player.position = deathposition
            + new Vector3(Random.value * 2 - 1, Random.value * -1)
            * Mathf.Clamp((mad += Time.deltaTime * 30) - 0.5f, 0, 1) * 0.2f;
        background.color = Color.Lerp(Color.white, Color.red, mad / 30);
    }
}
