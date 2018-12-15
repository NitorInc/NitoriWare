using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSuckCursorStuff : MonoBehaviour {
    private bool click;
    void Start()
    {
        click = true;
    }

    void LateUpdate()
    {
        Vector3 cursorPosition = CameraHelper.getCursorPosition();
        cursorPosition.z = transform.position.z;
        transform.position = cursorPosition;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (click == false)
            {
                click = true;
                CircleCollider2D collider = GetComponentInChildren<CircleCollider2D>();
                collider.gameObject.SetActive(true);
            }
            
        }
        else if (click == true)
        {
            click = false;
            CircleCollider2D collider = GetComponentInChildren<CircleCollider2D>();
            collider.gameObject.SetActive(false);
        }
    }

}
