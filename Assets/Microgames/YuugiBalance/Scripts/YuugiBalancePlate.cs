using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuugiBalancePlate : MonoBehaviour
{

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private SpriteRenderer background;
    private SpriteRenderer rend;
    [SerializeField]
    private Sprite failure_spr;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private YuugiBalancePlayer player_script;
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private new AudioSource audio;
    private float z = 0,
        target_x = 0,
        sin = Mathf.PI / 2f,
        sine_speed = 2f,
        max_rotation = 80,
        fail_distance = 4,
        fall = 0,
        distance = 0,
        mad = 0,
        start_time;
    [SerializeField]
    private float delay = 0, //delay in seconds before start
        difficulty = 2,
        beats_left_to_win = 0.5f;
    private Vector3 deathposition, bgdefpos;
    private bool success = false;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        bgdefpos = background.transform.position;
        //randomize if plate will start rotating left or right
        if(Random.value > 0.5f)
            sin = Mathf.PI * 3 / 2f;
        start_time = Time.time;
    }

    void LateUpdate()
    {
        //run the failed update if failed
        if(!MicrogameController.instance.getVictory())
        {
            Failed();
            return;
        }

        if(MicrogameController.instance.session.BeatsRemaining <= beats_left_to_win)
        {
            Succeed();
            return;
        }

        //update where the plate should be in X (affected by sine function and position difference from player)
        //the delay is used to prevent the plate from falling too fast at the start
        target_x += (Mathf.Sin(sin += Time.deltaTime * sine_speed) / 5f - distance)
            * Time.deltaTime * difficulty;
        if (delay > 0f)
            target_x *= Mathf.Clamp01((Time.time - start_time) / delay);

        //calculate distance and rotate plate
        distance = player.position.x - target_x;
        z = distance * max_rotation / fail_distance;

        //set the animator value
        anim.SetFloat("emotion", Mathf.Abs(distance / fail_distance));

        //set position and rotation
        transform.position = player.position + offset;
        transform.eulerAngles = Vector3.forward * z;

        //set bg paralax position
        background.transform.position = bgdefpos + Vector3.right * distance * -0.1f;

        //save the variable for less abs calls
        float abs = Mathf.Abs(distance);

        //check for failure
        if(abs > fail_distance)
        {
            MicrogameController.instance.setVictory(false, true);

            //death animation
            anim.SetBool("rip", true);

            //change sprite if available
            if(failure_spr != null)
                rend.sprite = failure_spr;

            //set death pos
            deathposition = player.position;

            //remove controls
            Destroy(player.GetComponent<YuugiBalancePlayer>());
        }

        //play beeping noise when nearing failure
        if(abs > 0.2F)
        {
            audio.loop = true;
            if(!audio.isPlaying)
                audio.Play();
            audio.pitch = (1 + abs);
            audio.panStereo = AudioHelper.getAudioPan(transform.position.x);
        }
        audio.loop = false;
    }

    void Succeed()
    {
        if(!success)
        {
            success = true;
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

        //shake
        player.position = deathposition
            + new Vector3(Random.value * 2 - 1, Random.value * -1)
            * Mathf.Clamp((mad += Time.deltaTime * 30) - 0.5f, 0, 1) * 0.2f;
        background.color = Color.Lerp(Color.white, Color.red, mad / 30);
    }
}
