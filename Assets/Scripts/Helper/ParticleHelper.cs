using UnityEngine;
using System.Collections;

public class ParticleHelper
{

	public static void setEmissionRate(ParticleSystem particleSystem, float rate)
	{
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		var newRate = new ParticleSystem.MinMaxCurve();
		newRate.constantMax = rate;
		emission.rate = newRate;
	}

	public static float getEmissionRate(ParticleSystem particleSystem)
	{
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		return emission.rate.constantMax;
	}
}
