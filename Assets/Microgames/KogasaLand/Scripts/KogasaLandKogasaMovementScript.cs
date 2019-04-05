using UnityEngine;
using UnityEngine.Serialization;

public class KogasaLandKogasaMovementScript : MonoBehaviour
{
    [SerializeField]
    [Header("How fast Kogasa falls")]
    public float fallSpeed = 1f;

    [SerializeField]
    [Header("How fast Kogasa can turn")]
    public float turnSpeed = 0.1f;

    //Momentum of Kogasa
    float _momentum = 0f;

    //Position of Kogasa
    Vector2 _position;

    // Use this for initializations
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Update position of Kogasa
        _position = transform.position;

        //Set momentum of Kogasa to TurnSpeed if the right arrow is pressed, -TurnSpeed if the left arrow is pressed
        _momentum = Input.GetKey(KeyCode.RightArrow) ? turnSpeed : Input.GetKey(KeyCode.LeftArrow) ? -turnSpeed : 0;

        //X and Y positions of Kogasa
        float x = _position.x;
        float y = _position.y;

        //If the microgame is not finished, move Kogasa downward based on the time and speed.
        transform.position = !MicrogameController.instance.getVictoryDetermined()
            ? new Vector2(x + _momentum, y - fallSpeed * Time.deltaTime)
            : _position;
    }
}