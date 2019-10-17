using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomRaceMarisa : MonoBehaviour {

	[SerializeField]
	float moveSpeed;
	[SerializeField]
	float yBound;
    [SerializeField]
    private int ringsRequired = 2;
    [SerializeField]
    private Animator rigAnimator;

    private int rings = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("MicrogameTag1"))
        {
            collision.GetComponent<BroomRaceRing>().activate();
            rings++;

            rigAnimator.SetInteger("Rings", rings);
            rigAnimator.SetTrigger("Ring");
            if (rings == ringsRequired)
            {
                rigAnimator.SetTrigger("Victory");
                MicrogameController.instance.setVictory(true);
                enabled = false;
            }

            collision.enabled = false;
        }
    }
    
    void Update ()
    {
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			transform.position += Vector3.up * moveSpeed * Time.deltaTime;
			if (transform.position.y > yBound) 
				transform.position = new Vector3 (transform.position.x, yBound, transform.position.z);
		}

		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			transform.position += Vector3.down * moveSpeed * Time.deltaTime;

			if (transform.position.y < -yBound ) 
				transform.position = new Vector3 (transform.position.x, -yBound, transform.position.z);
		}



	}
}
