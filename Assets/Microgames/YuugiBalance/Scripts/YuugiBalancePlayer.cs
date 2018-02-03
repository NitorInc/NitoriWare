using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YuugiBalancePlayer : MonoBehaviour
{

    private float x = 0, max_horizontal = 5, current_acceleration = 0;
    [SerializeField]
    private float acceleration = 3, speed = 10;

    void Update()
    {
        //assigning input values
        float input = (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0)
        + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);

        //adding acceleration value depending on input
        current_acceleration = Mathf.MoveTowards(current_acceleration,
            input, Time.deltaTime * acceleration);

        //modifiying x value depending on acceleration
        x = Mathf.Clamp(x + current_acceleration * Time.deltaTime * speed
            , -max_horizontal, max_horizontal);

        //resetting acceleration in case of hitting wall
        if (x == max_horizontal && current_acceleration > 0
            || x == -max_horizontal && current_acceleration < 0)
            current_acceleration = 0;

        //applying position
        transform.position = new Vector3(x, 0);
    }
}
