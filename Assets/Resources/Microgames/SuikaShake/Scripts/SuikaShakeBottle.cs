using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikaShakeBottle : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
	private float health, minSpeed, progressMult, sideChance;
	[SerializeField]
	private int healthPerSuika;
    [SerializeField]
    private BoxCollider2D middleCollider, leftCollider, rightCollider;
    [SerializeField]
    private Sprite middleSprite, rightSprite, fallSprite;
	[SerializeField]
	private GameObject suikaPrefab;
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
		suikas = new SuikaShakeSuika[(int)(health / healthPerSuika)];
		for (int i = 0; i < suikas.Length; i++)
		{
            suikas[i] = Instantiate(suikaPrefab, transform.position, Quaternion.identity).GetComponent<SuikaShakeSuika>();
            if (Random.Range(0f, 1f) < sideChance)
            {
                suikas[i].spriteRenderer.sprite = rightSprite;
                if (MathHelper.randomBool())
                {
                    suikas[i].transform.position += generateSuikaOffset(leftCollider);
                    suikas[i].transform.GetChild(0).localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    suikas[i].transform.position += generateSuikaOffset(rightCollider);
                }
            }
            else
            {
                suikas[i].spriteRenderer.sprite = middleSprite;
                suikas[i].transform.position += generateSuikaOffset(middleCollider);
            }
			suikas[i].transform.parent = transform;
			suikas[i].spriteRenderer.sortingOrder = i + 1;

            Vibrate vibrate = suikas[i].transform.GetChild(0).GetComponent<Vibrate>();
            vibrate.vibrateSpeed *= Random.Range(.5f, 1.5f);
        }
		lastCursorPosition = CameraHelper.getCursorPosition();
		pauseBuffer = true;
	}

	void Update()
	{
		Vector2 currentCursorPosition = CameraHelper.getCursorPosition();

		if (health < 0)
		{
			if (transform.moveTowards(Vector2.zero, 30f))
				enabled = false;
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
	}

	void lowerHealth(float amount)
	{
		health -= amount;
		for (int i = 0; i < suikas.Length; i++)
		{
			suikas[i].setHealth(health - (i * healthPerSuika));
		}

		if (health <= 0)
		{
			GetComponent<FollowCursor>().enabled = false;
			MicrogameController.instance.setVictory(true, true);
		}
	}

	Vector3 generateSuikaOffset(Collider2D spawnCollider)
	{
        float xOffset = spawnCollider.bounds.extents.x, yOffset = spawnCollider.bounds.extents.y;
		return (Vector3)spawnCollider.offset + new Vector3(Random.Range(-xOffset, xOffset) , Random.Range(-yOffset, yOffset), 0f);
	}
}
