using UnityEngine;
using System;
using System.Collections.Generic;
using static HinaGrab;
public class HinaSpinMain : MonoBehaviour
{
	public static float lastPositionX;
    public static double Score;
    public static double ScoreRounded;
    public static float lastPositionY;
	public static float deltaX;
	public static float deltaY;
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
	