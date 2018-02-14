using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBubble_BubbleTextBoxBehaviour : MonoBehaviour {

    [SerializeField]
    private Vector2 castDirection;

    [SerializeField]
    private float castWidth;

    [SerializeField]
    private float castHeight;


    private AdvancingText aText;

    private float aTextSpeed;

    // This is for shwoing where the square box cast is being placed
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(castDirection + (Vector2) transform.position, new Vector3(castWidth, castHeight, 1));

    }

    // Use this for initialization
    void Start () {
        aText = GetComponentInChildren<AdvancingText>();
        aTextSpeed = aText.getAdvanceSpeed();
        aText.setAdvanceSpeed(0);

    }


    // Update is called once per frame
    void Update () {
        //print(MicrogameTimer.instance.countdownScale);
        print(MicrogameTimer.instance.beatsLeft);      
        RaycastHit2D[] result = Physics2D.BoxCastAll(castDirection + (Vector2) transform.position, new Vector3(castWidth, castHeight, 1), 0 , Vector2.zero);

        if (result.Length > 0)
        {
            foreach (RaycastHit2D r in result)
            {
                if (r.collider.name.Contains("Character"))
                {
                    aText.setAdvanceSpeed(aTextSpeed);
                }

            }
        }
        else
        {
            aText.setAdvanceSpeed(0);
        }

	}
}
