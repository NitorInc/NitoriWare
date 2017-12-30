using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperSatoriPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator rigAnimator;
    
	void Start ()
    {
		
	}
	
    public void startGame()
    {
        rigAnimator.enabled = true;
    }
}
