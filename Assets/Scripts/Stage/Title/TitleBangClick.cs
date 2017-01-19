using UnityEngine;
using System.Collections;

public class TitleBangClick : MonoBehaviour
{
	
	public Texture2D[] textures;
	public Color[] colors;
	public float activationTime;

	private Collider2D _collider;
	private SpriteColorFX.SpriteColorMasks3 mask;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	private int textureIndex, colorIndex;


	void Start ()
	{
		_collider = GetComponent<Collider2D>();
		mask = GetComponent<SpriteColorFX.SpriteColorMasks3>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();

		//Camera.main.GetComponent<AudioSource>().time = 40f;

	}

	void Update ()
	{


		if (activationTime > 0f)
		{
			activationTime -= Time.deltaTime;
			return;
		}


		float startTime = 3f / 60f,
			duration = 15f / 60f;

		if (Input.GetMouseButtonDown(0))
		{
			if (CameraHelper.isMouseOver(_collider))
			{

				Invoke("leftClick", startTime);
				animator.SetInteger("animation", 1);
				activationTime = duration;
			}
		}
		if (Input.GetMouseButtonDown(1))
		{
			if (CameraHelper.isMouseOver(_collider))
			{
				Invoke("rightClick", startTime);
				animator.SetInteger("animation", 1);
				activationTime = duration;
			}
		}
	}

	void leftClick()
	{
		click(0);
	}

	void rightClick()
	{
		click(1);
	}

	void click(int button)
	{
		if (button == 0)
		{
			colorIndex++;
			if (colorIndex >= colors.Length)
				colorIndex = 0;
			spriteRenderer.color = colors[colorIndex];

			animator.SetInteger("animation", 0);
		}
		else if (button == 1)
		{
			textureIndex++;
			if (textureIndex >= textures.Length)
				textureIndex = 0;
			mask.textureMaskRed = textures[textureIndex];
			animator.SetInteger("animation", 0);
		}
	}

}
