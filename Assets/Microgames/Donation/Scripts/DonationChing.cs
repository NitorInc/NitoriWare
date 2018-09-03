using UnityEngine;
using System.Collections;

public class DonationChing : MonoBehaviour
{

	public ObjectPool pool;

	new public AudioSource audio;

	public float initialScale, growSpeed, maxScale, fadeStartTime, fadeSpeed, ySpeed;

	public Vector2 xBounds, yBounds;

	private float scale, time, alpha;
	private TextMesh mesh;

	void Start ()
	{
		mesh = GetComponent<TextMesh>();
	}

	public void reset(Vector3 position)
	{

		position += new Vector3(Random.Range(xBounds.x, xBounds.y), Random.Range(yBounds.x, yBounds.y), pool.prefab.transform.position.z);
		transform.position = position;

		time = 0f;
		alpha = 1f;
		scale = initialScale;

		transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
	}

	
	void Update ()
	{
		time += Time.deltaTime;

		if (scale < maxScale)
		{
			scale += growSpeed * Time.deltaTime;
		}
		scale = Mathf.Min(scale, maxScale);

		if (time >= fadeStartTime)
		{
			alpha -= fadeSpeed * Time.deltaTime;
			if (alpha <= 0f)
			{
				pool.poolObject(gameObject);
			}
		}

		updateAppearance();

		transform.position += new Vector3(0f, ySpeed , .05f) * Time.deltaTime;

	}

	void updateAppearance()
	{
		transform.localScale = new Vector3(scale, scale, transform.localScale.z);
		Color color = mesh.color;
		color.a = alpha;
		mesh.color = color;
	}

}
