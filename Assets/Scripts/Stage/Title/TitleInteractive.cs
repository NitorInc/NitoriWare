using UnityEngine;
using System.Collections;

public class TitleInteractive : MonoBehaviour
{
    public float effectTime;

	public Animator animator, animatorToDisable;
    public int altAnimationFrequency = 3;

    private int currentAnimation;

    void Start ()
	{
        currentAnimation = 0;
        if (GameMenu.subMenu != GameMenu.SubMenu.Splash)
            effectTime = .01f;
	}
	
	void Update ()
	{

		if (effectTime <= 0f)
			checkForInteraction();
		else
		{
			effectTime -= Time.deltaTime;
			if (effectTime <= 0f)
			{
				effectTime = 0f;

                if (animator != null)
                {
                    animator.enabled = true;
                }
            }
		}
	}

	void checkForInteraction()
	{
		if (!GameMenu.shifting && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
			checkCollision();
	}

	public void checkCollision()
	{
		Ray mouseRay = MainCameraSingleton.instance.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection(mouseRay, Mathf.Infinity);

		if (hit && hit.collider.name == name)
		{
			if (animator != null)
			{
                animator.SetInteger("animation", (currentAnimation % altAnimationFrequency == altAnimationFrequency - 1) ? 2 : 1);
				Invoke("resetAnimation", .1f);
                currentAnimation++;
			}

		}

	}

	void resetAnimation()
	{
		animator.SetInteger("animation", 0);
	}
}
