using UnityEngine;
 
 public class KogasaMovementScript : MonoBehaviour
 {
 	[SerializeField]
 	[Header("How fast Kogasa falls")]
 	public float FallSpeed = 1f;
 
 	[SerializeField]
 	[Header("How fast Kogasa can turn")]
 	public float TurnSpeed = 0.1f;
 
 	float momentum = 0f;
 
 	// Use this for initializations
 	void Start()
 	{
 	}
 
 	// Update is called once per frame
 	void Update()
 	{	
 		//Set momentum of Kogasa to TurnSpeed if the right arrow is pressed, -TurnSpeed if the left arrow is pressed
 		momentum = Input.GetKey(KeyCode.RightArrow) ? TurnSpeed : Input.GetKey(KeyCode.LeftArrow) ? -TurnSpeed : 0;
 
 		//X and Y positions of Kogasa
 		float x = transform.position.x;
 		float y = transform.position.y;
 
 		//If the microgame is not finished, move Kogasa downward based on the time and speed.
 		if (!MicrogameController.instance.getVictoryDetermined())
			 transform.position = new Vector2(x + momentum * Time.deltaTime, y - FallSpeed * Time.deltaTime);
 	}
 }