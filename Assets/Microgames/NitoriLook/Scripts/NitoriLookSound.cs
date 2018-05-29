using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NitoriLookSound : MonoBehaviour
{
    public Transform nitori;
    public Transform player;

	void LateUpdate ()
    {
        var playerToNitori = nitori.position - player.position;
        transform.position = player.position + (playerToNitori * -1f);
    }
}
