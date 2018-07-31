using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPourState : MonoBehaviour {

	private const float FILL_RATE = 75;

	[SerializeField]
	private MilkPourCup _currentCup;

	[SerializeField]
	private mpState _currentState;

	private enum mpState {
		Idle,
		Filling
	}

	void Start () { }

	void Update () {

		if (Input.GetKey (KeyCode.Space))
			_currentState = mpState.Filling;
		else
			_currentState = mpState.Idle;

		switch(_currentState) {
			case mpState.Idle: {
				if (_currentCup.Fill > _currentCup.RequiredFill)
					MicrogameController.instance.setVictory(true, true);
			}
			break;
			case mpState.Filling:
			{
				_currentCup.Fill += FILL_RATE * Time.deltaTime;
				if (_currentCup.Fill > MilkPourCup.MAX_FILL)
					MicrogameController.instance.setVictory(false, true);
			}
			break;
		}
	}
}
