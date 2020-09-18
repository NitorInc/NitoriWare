using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SpeedController : MonoBehaviour
{
    public const int MAX_SPEED = 10;

    [SerializeField]
    private AudioMixer gameplayMixer;

    [Range(1, MAX_SPEED)]
    [SerializeField]
    private int speed = 1;
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField]
    private float timeScalePerSpeedUp = .125f;

    private void Awake()
    {
        ApplySpeed();
    }


    public void ApplySpeed()
    {
        Time.timeScale = GetSpeedTimeScaleMult();
        gameplayMixer.SetFloat("MasterPitch", Time.timeScale);
    }

    public float GetSpeedTimeScaleMult()
	{
		return GetSpeedTimeScaleMult(speed);
	}

	public float GetSpeedTimeScaleMult(int speed)
	{
		return 1f + ((float)(speed - 1) * timeScalePerSpeedUp);
	}

    private void OnDestroy()
    {
        speed = 1;
        ApplySpeed();
    }
}
