using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlanGrab_SightBehaviour : MonoBehaviour {

    private GameObject sightSprite;
    [SerializeField]
    private GameObject meteorInSight;
    [SerializeField]
    private AudioClip hitClip, missClip;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {

		if (!MicrogameController.instance.getVictoryDetermined())
        {

            moveSightToMousePosition();
            if (Input.GetMouseButtonDown(0))
            {
                CastRay();
            }

        }

        // Deactivate Sight if game has ended
        else
        {
            if (gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }
	}

    // move sight gameObject to cursor Position
    void moveSightToMousePosition()
    {
        Vector2 mousePosition = CameraHelper.getCursorPosition();
        transform.position = mousePosition;
    }

    // cast CircleCast and destroy every meteor which collider was captured by the CircleCast
    void CastRay()
    {
        var position = new Vector2(transform.position.x, transform.position.y);
        var radius = GetComponent<CircleCollider2D>().radius;
        RaycastHit2D[] colliders = Physics2D.CircleCastAll(position, radius, new Vector2(0, 0));
        foreach (RaycastHit2D r in colliders)
        {
            var objCollider = r.collider;
            if (objCollider.name.Contains("Meteor"))
            {
                var objScript = objCollider.gameObject.GetComponent<FlanGrab_Meteor_BehaviourScript>();
                objScript.destroyMeteor();
                MicrogameController.instance.playSFX(hitClip, AudioHelper.getAudioPan(CameraHelper.getCursorPosition().x) * .75f);
                return;
            }
        }
        MicrogameController.instance.playSFX(missClip, AudioHelper.getAudioPan(CameraHelper.getCursorPosition().x) * .75f);
    }

}
