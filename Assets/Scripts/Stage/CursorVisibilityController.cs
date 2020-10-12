using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisibilityController : MonoBehaviour
{
    [SerializeField]
    MicrogamePlayer microgamePlayer;

    public void SetFromMicrogameControlScheme()
    {
        var session = microgamePlayer.CurrentMicrogameSession;
        SetVisibility(session.microgame.controlScheme == Microgame.ControlScheme.Mouse);
    }
    public void SetVisibilityToCurrent()
    {
        var session = microgamePlayer.CurrentMicrogameSession;
        SetVisibility(session.microgame.controlScheme == Microgame.ControlScheme.Mouse && !session.GetHideCursor());
    }

    public void SetVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = GameController.DefaultCursorMode;
    }
}
