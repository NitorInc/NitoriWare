using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReimuDodge3BulletsBehaviour : MonoBehaviour
{

	public ReimuDodgeBulletBehaviour bulletTemplate;
	public GameObject bulletTarget;

	// Use this for initialization
	void Start ()
	{
		// Deactivate the template so we can activate it after setting properties
		bulletTemplate.gameObject.SetActive (false);

		for (int x = -6; x <= 6; x += 2) {
			CreateBullet (x, 6f);
			CreateBullet (x, -6f);
		}
		for (int y = -3; y <= 3; y += 3) {
			CreateBullet (-7.5f, y);
			CreateBullet (7.5f, y);
		}	
	}

	void CreateBullet (float x, float y)
	{
		ReimuDodgeBulletBehaviour bullet = (ReimuDodgeBulletBehaviour)Instantiate (
			                                   bulletTemplate, 
			                                   new Vector3 (x, y, 0),
			                                   Quaternion.identity
		                                   );

		bullet.SetDelay (Random.value * 4.5f + 0.001f);
		bullet.SetSpeed (5f);
		bullet.SetTarget (bulletTarget);
		bullet.gameObject.SetActive (true);
	}
}
