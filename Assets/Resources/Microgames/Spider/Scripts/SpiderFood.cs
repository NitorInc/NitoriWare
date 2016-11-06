using UnityEngine;
using System.Collections;

public class SpiderFood : MonoBehaviour
{
	public float grabRadius;

	public float initialScale;
	public bool grabbed, eaten;
	public float particleRate;
	public float y;

	private SpriteRenderer spriteRenderer;
	private ParticleSystem particles;


	void Awake ()
	{
		particles = GetComponent<ParticleSystem>();
		particleRate = ParticleHelper.getEmissionRate(particles);
		spriteRenderer = GetComponent<SpriteRenderer>();
		ParticleHelper.setEmissionRate(particles, 0f);

	}
	
	void Update ()
	{
		Vector3 cursorPosition = CameraHelper.getCursorPosition();
		if (!eaten)
		{
			spriteRenderer.color = Color.white;
			if (!grabbed)
			{
				float distance = ((Vector2)(transform.position - cursorPosition)).magnitude;
				if (distance <= grabRadius && Input.GetMouseButtonDown(0))
					grabbed = true;

				float scale = (1f + (Mathf.Sin(Time.time * 8f) / 5f)) * initialScale;
				transform.localScale = new Vector3(scale, scale, 1f);
			}
			else
			{
				transform.position = new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z);
				transform.localScale = Vector3.one * initialScale;

				if (!Input.GetMouseButton(0))
					grabbed = false;
			}
		}
		else
		{
			spriteRenderer.color = Color.clear;

			if (Random.Range(0f, 1f) <= .55f)
				particles.startColor = new Color(1f, .75f, .8f);
			else
				particles.startColor = new Color(.08f, .025f, 0f);
		}
		
	}
}
