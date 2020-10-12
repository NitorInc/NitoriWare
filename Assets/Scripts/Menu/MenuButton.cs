using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private Button button;
    [SerializeField]
    private Animator buttonAnimator;
    [SerializeField]
    private Collider2D backupCollider;
    [SerializeField]
    private bool playPressAnimation = true;
    [SerializeField]
    private bool SFXIgnoreListenerPause;
    [SerializeField]
    private bool checkMouseOverCollider = true;
    [SerializeField]
    private AudioSource sfxSource;
    public AudioSource SfxSource { get { return sfxSource; } set { sfxSource = value; } }
    [SerializeField]
    private AudioClip pressClip;
    [SerializeField]
    private KeyCode pressKey = KeyCode.None;
    public KeyCode PressKey
    {
        get { return pressKey; }
        set { pressKey = value; }
    }
#pragma warning restore 0649

    public bool forceDisable { get; set; }
    private int clickBuffer;   //Waits one frame to enable button to prevent carry-over clicks from last scene
    private CanvasGroup parentGroup;

	void Start()
    {
        bufferClick();
        button.onClick.AddListener(() => onClick());
        parentGroup = GetComponentInParent<CanvasGroup>();
        if (SFXIgnoreListenerPause)
            sfxSource.ignoreListenerPause = true;
    }

    public void onClick()
    {
        sfxSource.PlayOneShot(pressClip);
    }

    void OnDestroy()
    {
        button.onClick.RemoveListener(() => onClick());
    }

    private void OnEnable()
    {
          bufferClick();
    }

    void bufferClick()
    {
        //button.interactable = false;
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
            {
                button.interactable = true;
            }
            return;
        }

        if (!button.IsInteractable())
            return;


        buttonAnimator.SetBool("MouseHeld", Input.GetMouseButton(0));
        buttonAnimator.SetBool("MouseDown", playPressAnimation && Input.GetMouseButtonDown(0));
        buttonAnimator.SetBool("MouseOver", !checkMouseOverCollider || isMouseOver());

        if (button.enabled && button.interactable && Input.GetKeyDown(pressKey))
        {
            button.onClick.Invoke();
        }
    }

    bool shouldButtonBeEnabled() => !forceDisable && !GameMenu.shifting;

    public bool isMouseOver()
    {
        return !GameMenu.shifting && CameraHelper.isMouseOver(backupCollider);
    }
}
