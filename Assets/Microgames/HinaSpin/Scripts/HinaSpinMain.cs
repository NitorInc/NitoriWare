using UnityEngine;
using System;
using System.Collections.Generic;
using static HinaGrab;
public class HinaSpinMain : MonoBehaviour
{
	public float lastPositionX;
    public double Score;
    public double ScoreRounded;
    public float lastPositionY;
	public float deltaX;
	public float deltaY;
	 void Start()
    {		
    }

	void Awake ()
	{
		lastPositionX = transform.position.x;
		lastPositionY = transform.position.y;
		deltaX = transform.position.x - lastPositionX;
		deltaY = transform.position.y - lastPositionY;
	}
 
	void Update()
	{
		deltaX = transform.position.x - lastPositionX;
		deltaY = transform.position.y - lastPositionY;
		lastPositionX = transform.position.x;
		lastPositionY = transform.position.y;
        Score = Score + Math.Abs(deltaX) + Math.Abs(deltaY);
        ScoreRounded = Math.Round(Score, 3);
        Debug.Log(ScoreRounded);
	}
}
	