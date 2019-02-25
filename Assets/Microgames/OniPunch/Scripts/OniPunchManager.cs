using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OniPunchManager : MonoBehaviour 
//	Class must be parented to the Gameplay GameObject.
{
	[SerializeField]private OniPunchTiming timingChild;
	[SerializeField]private OniPunchCharging chargeChild;
	[SerializeField]private Animator gameANI;
	private int microGamePartsWon = 0;

	void Start()
	{
		chargeChild.activate(true);
		timingChild.activate(false);
	}

	public void winMicrogamePart()
	{
		microGamePartsWon++;
		switch(microGamePartsWon)
		{
			case 1:
				timingChild.activate(true);
				chargeChild.activate(false);
			break;
			
			case 2:
				timingChild.activate(false);
				MicrogameController.instance.setVictory(true);
				gameANI.SetTrigger("Success");
			break;
		}
	}
}
