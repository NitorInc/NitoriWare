using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
  [SerializeField]
  private AudioClip pressClip;
  [SerializeField]
  private KeyCode pressKey = KeyCode.None;
#pragma warning restore 0649

  private bool _forceDisable;
  public bool forceDisable
  {
    get { return _forceDisable; }
    set { _forceDisable = value; }
  }


  private int clickBuffer;   //Waits one frame to enable button to prevent carry-over clicks from last scene

  void Start()
  {
    bufferClick();
    button.onClick.AddListener(() => onClick());
    if (SFXIgnoreListenerPause)
      sfxSource.ignoreListenerPause = true;
  }

  public void onClick()
  {
    float volume = PrefsHelper.getVolume(PrefsHelper.VolumeType.SFX);
    if (volume > 0f)
      sfxSource.PlayOneShot(pressClip, volume);
  }

  void OnDestroy() => button.onClick.RemoveListener(() => onClick());

  void OnEnable() => bufferClick();

  void bufferClick()
  {
    button.interactable = false;
    clickBuffer = 2;
  }

  void enableButton() => button.interactable = true;

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
    if (button.interactable != shouldEnable)
      setButtonEnabled(shouldEnable);

    buttonAnimator.SetBool("MouseHeld", Input.GetMouseButton(0));
    buttonAnimator.SetBool("MouseDown", playPressAnimation && Input.GetMouseButtonDown(0));
    buttonAnimator.SetBool("MouseOver", !checkMouseOverCollider || isMouseOver());

    if (button.enabled && button.interactable && Input.GetKeyDown(pressKey))
    {
      button.onClick.Invoke();
    }
  }

  bool shouldButtonBeEnabled() => !_forceDisable && !GameMenu.shifting;

  void setButtonEnabled(bool enabled)
  {
    button.interactable = enabled;
    if (enabled && isMouseOver())
    {
      buttonAnimator.ResetTrigger("Normal");
      buttonAnimator.SetTrigger("Highlighted");
    }
  }

  public bool isMouseOver() => !GameMenu.shifting && CameraHelper.isMouseOver(backupCollider);
}
