using UnityEngine;
using System.Collections;

public class ChenFoodVibrate : MonoBehaviour
{
	public bool vibrateOn;
	public float vibrateSpeed, vibrateRadius;
	private Vector2 vibrateGoal;

	public Sprite mainSprite, otherSprite;

	private float startZ;

	private SpriteRenderer spriteRenderer;

	void Awake()
	{
		startZ = transform.position.z;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void setSprite(bool isOther)
	{
		if (isOther)
			spriteRenderer.sprite = otherSprite;
		else
			spriteRenderer.sprite = mainSprite;
	}
	
	void Update ()
	{
		if (vibrateOn)
			updateVibrate();
		else
			transform.localPosition = Vector3.zero;
	}

	

	void updateVibrate()
	{
		Vector2 diff = vibrateGoal - (Vector2)transform.localPosition;
		if (diff.magnitude <= vibrateSpeed * Time.deltaTime)
		{

			transform.localPosition = (Vector3)vibrateGoal;
			resetVibrate();
		}
		else
		{
			transform.localPosition +=
				(Vector3)MathHelper.getVector2FromAngle(MathHelper.getAngle(vibrateGoal - (Vector2)transform.localPosition),
				vibrateSpeed * Time.deltaTime);

		}

		transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
	}

	public void resetVibrate()
	{
		vibrateGoal = MathHelper.getVector2FromAngle(Random.Range(0f, 360f), Random.Range(0f, vibrateRadius));
	}
	
}
