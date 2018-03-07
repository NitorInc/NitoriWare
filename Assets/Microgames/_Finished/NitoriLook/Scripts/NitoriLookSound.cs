using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookSound : MonoBehaviour
{
    public Transform nitori;
    public Transform player;

    //private AudioSource propSource;
	
	void LateUpdate ()
    {
        var playerToNitori = nitori.position - player.position;
        transform.position = player.position + (playerToNitori * -1f);

        //float playerAngle = MathHelper.trueMod(-player.rotation.eulerAngles.y + 90f, 360f);
        //float angleDiff = 
    }
}
