using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedController : MonoBehaviour
{
    public const int MAX_SPEED = 10;

    [Range(1, MAX_SPEED)]
    [SerializeField]
    private int speed;
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    private float timeScalePerSpeedUp = .125f;


    public void ApplyToTimeScale()
    {
        Time.timeScale = GetSpeedTimeScaleMult();
    }

    public float GetSpeedTimeScaleMult()
	{
		return GetSpeedTimeScaleMult(speed);
	}

	public float GetSpeedTimeScaleMult(int speed)
	{
		return 1f + ((float)(speed - 1) * timeScalePerSpeedUp);
	}
}
