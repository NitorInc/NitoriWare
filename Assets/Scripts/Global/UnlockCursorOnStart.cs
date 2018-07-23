using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCursorOnStart : MonoBehaviour
{

    void Start()
    {
        Invoke("unlockCursor", .1f);
    }

    void unlockCursor()
    {
        Cursor.lockState = GameController.DefaultCursorMode;
    }
}
