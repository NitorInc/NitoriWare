using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCursorOnStart : MonoBehaviour
{
    [SerializeField]
    bool forceOnUpdate = true;

    void Start()
    {
        Invoke("unlockCursor", .1f);
    }

    void unlockCursor()
    {
        Cursor.lockState = GameController.DefaultCursorMode;
    }

    private void Update()
    {
        if (Cursor.lockState != GameController.DefaultCursorMode)
            Cursor.lockState = GameController.DefaultCursorMode;
    }
}
