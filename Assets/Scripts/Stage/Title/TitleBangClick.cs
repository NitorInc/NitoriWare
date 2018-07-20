using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleBangClick : MonoBehaviour
{

    public Image[] images, outlineImages;
	public Sprite[] sprites;
	public Color[] colors;
	public float activationTime;

	private Collider2D _collider;
	private Animator animator;

	private int spriteIndex, colorIndex;


	void Start ()
	{
		_collider = GetComponent<Collider2D>();
		animator = GetComponent<Animator>();
        if (GameMenu.subMenu != GameMenu.SubMenu.Splash)
            activationTime = .01f;
	}

	void Update ()
	{
        
		if (activationTime > 0f)
		{
			activationTime -= Time.deltaTime;
			return;
		}
        if (GameMenu.shifting)
            return;


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
            foreach (Image image in images)
            {
                image.color = colors[colorIndex];
            }
            foreach (Image image in outlineImages)
            {
                image.color = colors[colorIndex];
            }

            animator.SetInteger("animation", 0);
		}
		else if (button == 1)
		{
			spriteIndex++;
			if (spriteIndex >= sprites.Length)
				spriteIndex = 0;
            foreach (Image image in images)
            {
                image.sprite = sprites[spriteIndex];
            }
			animator.SetInteger("animation", 0);
		}
	}

}
