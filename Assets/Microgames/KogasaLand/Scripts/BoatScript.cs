using UnityEngine;

public class BoatScript : MonoBehaviour
{
    [SerializeField] 
    [Header("Speed of the boat")]
    public float Speed = 0f;

    [SerializeField] 
    [Header("Where the left and right borders of the screen are")]
    public float Border = 0f;
    
    [SerializeField] 
    [Header("Room for error")]
    public float Leniency  = 0f;

    
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Position of the platform
        Vector2 position = transform.position;
        
        //If the microgame is not finished, then move the platform based on the Speed and time
        if (!MicrogameController.instance.getVictoryDetermined())
            transform.position = new Vector2(transform.position.x + Speed * Time.deltaTime, position.y);

        //If the boat is on or behind the left or right border of the screen, then reverse the direction of the platform
        Speed = position.x >= Border || position.x <= -Border ? Speed * -1 : Speed;
    }
    
    //Activates if the platform touches another GameObject
    void OnTriggerEnter2D(Collider2D other)
    {
        //Get the bottom side of Kogasa's collider
        float kogasaBottom = other.transform.position.y - other.GetComponent<BoxCollider2D>().size.y / 2;
        
        //Get the top side of the platform's collider
        float platformTop = transform.position.y + GetComponent<BoxCollider2D>().size.y / 2;
                       
        //If Kogasa touches the platform while above it or equally level, then victory is achieved!
        MicrogameController.instance.setVictory(kogasaBottom >= platformTop - Leniency, true);
    }
}