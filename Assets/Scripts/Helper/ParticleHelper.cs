using UnityEngine;
using System.Collections;

public class ParticleHelper
{
	/// <summary>
	/// Sets the emission rate (single float only)
	/// </summary>
	/// <param name="particleSystem"></param>
	/// <param name="rate"></param>
	public static void setEmissionRate(ParticleSystem particleSystem, float rate)
	{
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		var newRate = new ParticleSystem.MinMaxCurve();
		newRate.constantMax = rate;
		emission.rateOverTime = newRate;
	}

	/// <summary>
	/// Gets the emission rate (single float only)
	/// </summary>
	/// <param name="particleSystem"></param>
	/// <param name="rate"></param>
	public static float getEmissionRate(ParticleSystem particleSystem)
	{
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		return emission.rateOverTime.constantMax;
	}

	/// <summary>
	/// Enable or disable particle emission
	/// </summary>
	/// <param name="particleSystem"></param>
	/// <param name="rate"></param>
	public static void setEmissionEnabled(ParticleSystem particleSystem, bool enabled)
	{
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		emission.enabled = Input.GetMouseButton(0);
	}

	/// <summary>
	/// Return whether particle emission is enabled
	/// </summary>
	/// <param name="particleSystem"></param>
	/// <param name="rate"></param>
	public static bool getEmissionEnabled(ParticleSystem particleSystem)
	{
		return particleSystem.emission.enabled;
	}
}
