using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCursorOnStart : MonoBehaviour
{
    
	void Start ()
    {
        Invoke("unlockCursor", .1f);
	}

    void unlock()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
