using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class KogasaLandBoatScript : MonoBehaviour
{
    [SerializeField]
    [Header("Speed of the boat")]
    public float speed = 0f;

    [SerializeField]
    [Header("Where the left and right borders of the screen are")]
    public float border = 0f;

    [SerializeField]
    [Header("Room for error")]
    public float leniency = 0f;

    //Vector of Kogasa
    Vector2 _position;

    //X and Y values of the platform
    float _x;
    float _y;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Update position value
        _position = transform.position;

        //X and Y positions of Kogasa
        _x = _position.x;
        _y = _position.y;

        //If the boat is on or behind the left or right border of the screen, then reverse the direction of the platform
        speed = _x >= border || _x <= -border ? speed * -1 : speed;

        //If the microgame is not finished, then move the platform based on the Speed and time
        transform.position = !MicrogameController.instance.getVictoryDetermined()
            ? new Vector2(_x + speed * Time.deltaTime, _y)
            : _position;
    }

    //Activates if the platform touches another GameObject
    void OnTriggerEnter2D(Collider2D other)
    {
        //Get the bottom side of Kogasa's collider
        float kogasaBottom = other.transform.position.y - other.GetComponent<BoxCollider2D>().size.y / 2;

        //Get the top side of the platform's collider
        float platformTop = _y + GetComponent<BoxCollider2D>().size.y / 2;

        //If Kogasa touches the platform while above it or equally level, then victory is achieved!
        MicrogameController.instance.setVictory(kogasaBottom >= platformTop - leniency, true);
    }
}