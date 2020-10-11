using UnityEngine;
using System.Collections;

public class TraceShapeCursor : MonoBehaviour
{
	public int inBoundsNeeded, outOfBoundsLeft;
    public float particleDisappearStartTime, particleDisappearDuration, particleDisappearResumeTime;
	public Collider2D[] essentialColliders;
	public Animator victoryAnimator;
	private AudioSource _audioSource;
    [SerializeField]
    private Transform inBoundColliders, outBoundColliders;
    [SerializeField]
	private AudioSource resultSoundSource;
	[SerializeField]
	private AudioClip victoryClip, buzzerClip;
    [SerializeField]
    private VictoryColor[] victoryColors;

	private ParticleSystem traceParticles;
    private bool failed;

	void Start ()
	{
		_audioSource = GetComponent<AudioSource>();
		traceParticles = GetComponent<ParticleSystem>();
		Cursor.visible = false;
	}
	
	void Update ()
	{
        updateCursor();
        if (!failed)
		    updateTrace();
	}

    private void LateUpdate()
    {
        updateCursor();
    }

    void updateTrace()
	{
		//Enable emmissions if mouse is held down
		ParticleSystem.EmissionModule emission = traceParticles.emission;
        emission.enabled = Input.GetMouseButton(0);
        _audioSource.volume = Input.GetMouseButton(0) ? 1f : 0f;

        if (!emission.enabled && Input.GetMouseButtonDown(0))
            traceParticles.Emit(1); //When mouse is first pressed, create a particle (otherwise one won't be created before the player moves their cursor

	}

    void updateCursor()
    {
        //Snap to cursor
        Vector3 cursorPosition = CameraHelper.getCursorPosition();
        transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z);
    }

	void collide(Collider2D other)
	{
		//Player must be holding down the mouse button to hit drawing colliders
		if (!traceParticles.emission.enabled)
			return;

		other.gameObject.SetActive(false);

		//Decrement victory/loss values depending on whether the collison is in or out of bounds
		if (other.transform.parent.name == "In Bounds")
		{
			inBoundsNeeded--;
			if (inBoundsNeeded <= 0)
			{
				checkForVictory();
			}
		}
		else if (other.transform.parent.name == "Out Bounds")
		{
			outOfBoundsLeft--;
			if (outOfBoundsLeft <= 0)
			{
				failure();
			}
		}
	}

	void checkForVictory()
	{
		//The player still doesn't win unless all the "essential" colliders have been hit
		for (int i = 0; i < essentialColliders.Length; i++)
		{
			//If one is still there, no victory yet
			if (essentialColliders[i].gameObject.activeInHierarchy)
				return;
		}

		victory();
	}

	void victory()
	{
		MicrogameController.instance.setVictory(true, true);
		resultSoundSource.PlayOneShot(victoryClip);

		victoryAnimator.enabled = true;
		gameObject.SetActive(false);
	}

	void failure()
    {
        var main = traceParticles.main;
        ParticleSystem.Particle[] allParticles = new ParticleSystem.Particle[main.maxParticles];
        int activeParticleCount = traceParticles.GetParticles(allParticles);
        for (int i = 0; i < activeParticleCount; i++)
        {
            allParticles[i].remainingLifetime = particleDisappearStartTime
                + (particleDisappearDuration -(i * (particleDisappearDuration / (float)activeParticleCount)));
        }
        traceParticles.SetParticles(allParticles, activeParticleCount);


        for (int i = 0; i < inBoundColliders.childCount; i++)
        {
            GameObject child = inBoundColliders.GetChild(i).gameObject;
            if (!child.activeInHierarchy)
            {
                inBoundsNeeded++;
                child.SetActive(true);
            }
        }
        for (int i = 0; i < outBoundColliders.childCount; i++)
        {
            GameObject child = outBoundColliders.GetChild(i).gameObject;
            if (!child.activeInHierarchy)
            {
                outOfBoundsLeft++;
                child.SetActive(true);
            }
        }

        var emission = traceParticles.emission;
        MicrogameController.instance.setVictory(false, false);
		emission.enabled = false;
		_audioSource.volume = 0f;
        foreach (VictoryColor vColor in victoryColors)
        {
            vColor.ignoreVictoryDetermined = true;
        }
        MicrogameController.instance.playSFX(buzzerClip);

        failed = true;
        Invoke("tryAgain", particleDisappearResumeTime);
	}

    void tryAgain()
    {
        if (MicrogameController.instance == null)   //TODO better fix for invoke bringing dead objects back
            return;

        failed = false;
        foreach (VictoryColor vColor in victoryColors)
        {
            vColor.ignoreVictoryDetermined = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
	{
		collide(other);
	}

	void OnTriggerStay2D(Collider2D other)
	{
		collide(other);
	}

}
