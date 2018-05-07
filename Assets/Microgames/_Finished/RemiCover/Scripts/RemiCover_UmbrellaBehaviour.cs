using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemiCover_UmbrellaBehaviour : MonoBehaviour {

    public float verticalPosition;

    // Update is called once per frame
    void Update() {
		if (!MicrogameController.instance.getVictory())
			return;
        Vector2 mousePosition = CameraHelper.getCursorPosition();
        transform.position = new Vector2(mousePosition.x, verticalPosition);
    }

}
