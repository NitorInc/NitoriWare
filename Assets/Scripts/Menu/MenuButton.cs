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
    [SerializeField]
    private bool playPressAnimation = true;
    [SerializeField]
    private KeyCode pressKey = KeyCode.None;
#pragma warning restore 0649

    private int clickBuffer;   //Waits one frame to enable button to prevent carry-over clicks from last scene

	void Start()
    {
        bufferClick();
    }

    private void OnEnable()
    {
        bufferClick();
    }

    void bufferClick()
    {
        button.interactable = false;
        clickBuffer = 2;
    }

    void enableButton()
    {
        button.interactable = true;
    }
	
	void LateUpdate()
	{
        if (clickBuffer > 0)
        {
            clickBuffer--;
            if (clickBuffer == 0)
                button.interactable = true;
            return;
        }

        bool shouldEnable = shouldButtonBeEnabled();
        if (button.enabled != shouldEnable)
            setButtonEnabled(shouldEnable);

        buttonAnimator.SetBool("MouseHeld", Input.GetMouseButton(0));
        buttonAnimator.SetBool("MouseDown", playPressAnimation && Input.GetMouseButtonDown(0));

        if (button.enabled & button.interactable && Input.GetKeyDown(pressKey))
        {
            button.onClick.Invoke();
        }
    }

    bool shouldButtonBeEnabled()
    {
        return !GameMenu.shifting;
    }

    void setButtonEnabled(bool enabled)
    {
        button.enabled = enabled;
        if (enabled && isMouseOver())
        {
            buttonAnimator.ResetTrigger("Normal");
            buttonAnimator.SetTrigger("Highlighted");
        }
    }

    public bool isMouseOver()
    {
        return !GameMenu.shifting && CameraHelper.isMouseOver(backupCollider);
    }
}
