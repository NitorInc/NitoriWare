using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouhouSortGrab : MonoBehaviour
{
    [SerializeField]
    private LayerMask spriteColliderMask;
    [SerializeField]
    private LayerMask boxColliderMask;

    void Update()
    {
        if (!MicrogameController.instance.getVictoryDetermined() && Input.GetMouseButtonDown(0))
            CheckGrab();
    }

    void CheckGrab()
    {

        var camera = MainCameraSingleton.instance;
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D spriteHit = Physics2D.GetRayIntersection(mouseRay, float.PositiveInfinity, spriteColliderMask);
        if (spriteHit)
        {
            var spriteGrabable = spriteHit.collider.GetComponentInParent<MouseGrabbable>();
            if (spriteGrabable != null)
            {
                spriteGrabable.grabbed = true;
                return;
            }
        }

        RaycastHit2D boxHit = Physics2D.GetRayIntersection(mouseRay, float.PositiveInfinity, boxColliderMask);
        if (boxHit)
        {
            var boxGrabbable = boxHit.collider.GetComponentInParent<MouseGrabbable>();
            if (boxGrabbable != null)
            {
                boxGrabbable.grabbed = true;
                return;
            }
        }
    }
}
