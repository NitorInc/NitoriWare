using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaShakeBottle : MonoBehaviour
{

    public static float flingSoundCooldown;

#pragma warning disable 0649
    [SerializeField]
	private float health, minSpeed, progressMult, sideChance;
	[SerializeField]
	private int healthPerSuika;
    [SerializeField]
    private BoxCollider2D middleCollider, leftCollider, rightCollider;
	[SerializeField]
	private GameObject suikaPrefab;
    [SerializeField]
    private GameObject sparkleGenerator;
    [SerializeField]
    private AudioClip victoryClip;
#pragma warning restore 0649

    private bool _pauseBuffer = false;
	public bool pauseBuffer
	{
		set { _pauseBuffer = value; }
		get { return _pauseBuffer; }
	}

	private SuikaShakeSuika[] suikas;
	private Vector2 lastCursorPosition;

	void Start ()
	{
        flingSoundCooldown = 0f;

		suikas = new SuikaShakeSuika[(int)(health / healthPerSuika)];
		for (int i = 0; i < suikas.Length; i++)
		{
            suikas[i] = Instantiate(suikaPrefab, transform.position, Quaternion.identity).GetComponent<SuikaShakeSuika>();
            if (Random.Range(0f, 1f) < sideChance)
            {
                if (MathHelper.randomBool())
                {
                    suikas[i].generateOffset(leftCollider);
                    suikas[i].setFacing(-1);
                }
                else
                {
                    suikas[i].generateOffset(rightCollider);
                    suikas[i].setFacing(1);
                }
            }
            else
            {
                suikas[i].generateOffset(middleCollider);
                suikas[i].setFacing(0);
            }
			suikas[i].transform.parent = transform;
            //suikas[i].spriteRenderer.sortingOrder = 1 + (suikas.Length - i);
            suikas[i].spriteRenderer.sortingOrder = i + 1;
        }
        lastCursorPosition = CameraHelper.getCursorPosition();
		pauseBuffer = true;
	}


	void Update()
	{
		Vector2 currentCursorPosition = CameraHelper.getCursorPosition();

        if (flingSoundCooldown > 0f)
            flingSoundCooldown -= Time.deltaTime;


        if (health < 0)
		{
			if (transform.moveTowards2D(Vector2.zero, 30f))
            {
                enabled = false;
            }
			return;
		}
		if (pauseBuffer)
		{
			pauseBuffer = false;
			lastCursorPosition = currentCursorPosition;
			return;
		}

		float diff = (currentCursorPosition - lastCursorPosition).magnitude;
		if (diff / Time.deltaTime >= minSpeed)
			lowerHealth(diff * progressMult);

		lastCursorPosition = currentCursorPosition;

        MicrogameController.instance.getSFXSource().panStereo = AudioHelper.getAudioPan(transform.position.x) * .8f;

    }

	void lowerHealth(float amount)
	{
		health -= amount;
		for (int i = 0; i < suikas.Length; i++)
		{
			suikas[i].setHealth(health - (i * healthPerSuika),
                (((Vector2)CameraHelper.getCursorPosition() - lastCursorPosition) / Time.deltaTime));
		}

		if (health <= 0)
        {
            GetComponent<FollowCursor>().enabled = false;
            var sineWave = GetComponent<SineWave>();
            if (sineWave != null)
            {
                sineWave.enabled = true;
                sineWave.resetCycle();
            }
            sparkleGenerator.SetActive(true);
            MicrogameController.instance.setVictory(true, true);
            MicrogameController.instance.playSFX(victoryClip, AudioHelper.getAudioPan(transform.position.x));
		}
	}
}
