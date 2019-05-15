//This class is to help trigger certain animation events

using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{

	private Animator animator;

	public UnityEvent[] unityEvents;
	public ParticleSystem[] particleSystems;
    public AudioSource[] audioSources;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void triggerEvent(int index)
	{
		unityEvents[index].Invoke();
	}

	public void playParticles(int index)
	{
		particleSystems[index].Stop();
		particleSystems[index].Play();
	}

	public void killParticles(int index)
	{
		particleSystems[index].Stop();
		particleSystems[index].SetParticles(new ParticleSystem.Particle[0], 0);
	}

	public void stopParticles(int index)
	{
		particleSystems[index].Stop();
	}

    public void playAudio(int index)
    {
        audioSources[index].Play();
    }

    public void stopAudio(int index)
    {
        audioSources[index].Stop();
    }

    public void pauseAudio(int index)
    {
        audioSources[index].Pause();
    }

    public void unPauseAudio(int index)
    {
        audioSources[index].UnPause();
    }

    /// <summary>
    /// Object MUST have a FadingMusic component
    /// </summary>
    public void fadeAudio(int index)
    {
        audioSources[index].GetComponent<FadingMusic>().startFade();
    }

	public void setShakeSpeed(float shakeSpeed)
	{
		CameraShake.instance.shakeSpeed = shakeSpeed;
	}

	public void setScreenShake(float shake)
	{
		CameraShake.instance.setScreenShake(shake);
	}

	public void setXshake(float shake)
	{
		CameraShake.instance.xShake = shake;
	}

	public void setYShake(float shake)
	{
		CameraShake.instance.yShake = shake;
	}

	public void addScreenShake(float shake)
	{
		CameraShake.instance.addScreenShake(shake);
	}

	public void addXshake(float shake)
	{
		CameraShake.instance.xShake += shake;
	}

	public void addYShake(float shake)
	{
		CameraShake.instance.yShake += shake;
	}

	public void setShakeCoolRate(float coolRate)
	{
		CameraShake.instance.shakeCoolRate = coolRate;
	}

    //Plays a pitch-scaled sound effect via MicrogameController, asset parameter is the soundClip, float parameter is the audio pan
    public void playMicrogameSFX(AnimationEvent values)
    {
        MicrogameController.instance.playSFX((AudioClip)values.objectReferenceParameter, values.floatParameter);
    }
    //Plays a pitch-scaled sound effect via MicrogameController auto-panned to object's position, asset parameter is the soundClip, float parameter is pan Mult
    public void playPannedMicrogameSFX(AnimationEvent values)
    {
        if (values.floatParameter <= 0f)
            values.floatParameter = 1f;
        MicrogameController.instance.playSFX((AudioClip)values.objectReferenceParameter, AudioHelper.getAudioPan(transform.position.x) * values.floatParameter);
    }

    //Use string paramater for name and float paramater for value
    public void setFloat(AnimationEvent values)
	{
		animator.SetFloat(values.stringParameter, values.floatParameter);
	}

	//Use string paramater for name and int paramater for value
	public void setInteger(AnimationEvent values)
	{
		animator.SetInteger(values.stringParameter, values.intParameter);
	}

	//Use string paramater for name and int paramater for value (1 for true, 0 for false)
	public void setBool(AnimationEvent values)
	{
		animator.SetBool(values.stringParameter, values.intParameter > 0);
	}

}
