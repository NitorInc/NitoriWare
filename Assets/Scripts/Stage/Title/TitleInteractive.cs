using UnityEngine;
using System.Collections;

public class TitleInteractive : MonoBehaviour
{

	public bool isTop;
	public float effectTime, rippleCoolTime;
	public TitleInteractive nextInteractive;
	public RippleEffect ripples;

	public Animator animator, animatorToDisable;

	private float rippleCooldown;

	void Start ()
	{
		rippleCooldown = 0f;
	}
	
	void Update ()
	{

		if (rippleCooldown > 0f)
			rippleCooldown -= Time.deltaTime;


		if (!isTop)
			return;

		if (effectTime <= 0f)
			checkForInteraction();
		else
		{
			effectTime -= Time.deltaTime;
			if (effectTime <= 0f)
			{
				effectTime = 0f;

				//if (animator != null)
				//{
				//	animator.enabled = true;
				//	if (animatorToDisable != null)
				//		animatorToDisable.enabled = false;
				//}
			}
		}
	}

	void checkForInteraction()
	{



		if (Input.GetMouseButtonDown(0))
		{
			checkCollision(0);
		}
		else if (Input.GetMouseButtonDown(1))
		{
			checkCollision(1);
		}
		//else if (ripples != null && Input.GetMouseButton(0))
		//{

		//	ripples.Emit();

		//	//ripples.Emit(position);
		//}



	}

	public void checkCollision(int button)
	{




		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection(mouseRay, Mathf.Infinity);

		if (hit && hit.collider.name == name)
		{
			if (animator != null)
			{
				animator.SetInteger("animation", button + 1);
				Invoke("resetAnimation", .1f);
			}

		}
		else if (nextInteractive != null)
		{
			nextInteractive.checkCollision(button);
		}
		else if (ripples != null)
		{
			if (rippleCooldown <= 0f)
			{
				ripples.Emit();
				rippleCooldown = rippleCoolTime;
			}
		}

	}

	void resetAnimation()
	{
		animator.SetInteger("animation", 0);
	}
}
