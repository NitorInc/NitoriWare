using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private Button button;
    [SerializeField]
    private Animator buttonAnimator;
    [SerializeField]
    private Collider2D backupCollider;
#pragma warning restore 0649

	void Start()
	{
		
	}
	
	void LateUpdate()
	{
        bool shouldEnable = shouldButtonBeEnabled();
        if (button.enabled != shouldEnable)
            setButtonEnabled(shouldEnable);

        buttonAnimator.SetBool("MouseHeld", Input.GetMouseButton(0));
        buttonAnimator.SetBool("MouseDown", Input.GetMouseButtonDown(0));
    }

    bool shouldButtonBeEnabled()
    {
        return !GameMenu.shifting;
    }

    void setButtonEnabled(bool enabled)
    {
        button.enabled = enabled;
        if (enabled && CameraHelper.isMouseOver(backupCollider))
        {
            buttonAnimator.ResetTrigger("Normal");
            buttonAnimator.SetTrigger("Highlighted");
        }
    }
}
