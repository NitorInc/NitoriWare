using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicBubble_BubbleTextBoxBehaviour : MonoBehaviour {

    [SerializeField]
    private Vector2 castOffset;

    [SerializeField]
    private float castWidth;

    [SerializeField]
    private float castHeight;

    [SerializeField]
    private float speechSpeed;  // The speed for the bubblespech to complete if it had only one character

    [SerializeField]
    private GameObject target;

    private AdvancingText aText;
    private float aSpeed;

    // Use this for initialization
    void Start () {
        aText = GetComponentInChildren<AdvancingText>();
        aSpeed = speechSpeed * aText.GetComponent<TMPro.TMP_Text>().text.Length;
        stopSpeechText();
    }

    // This is for showing where the square box cast is being placed
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(castOffset + (Vector2)transform.position, new Vector3(castWidth, castHeight, 1));
    }

    // Update is called once per frame
    void Update () {

        RaycastHit2D[] result = Physics2D.BoxCastAll(castOffset + (Vector2) transform.position, new Vector3(castWidth, castHeight, 1), 0 , Vector2.zero);

        if (result.Length > 0)
        {
            foreach (RaycastHit2D r in result)
            {
                if (r.collider.gameObject == target)
                {
                    advanceSpeechText();
                }

            }
        }

        else
        {
            stopSpeechText();
        }

	}

    // Stop text from showing
    void stopSpeechText()
    {
        aText.setAdvanceSpeed(0);
    }

    // Advance text
    void advanceSpeechText()
    {
        aText.setAdvanceSpeed(aSpeed);
    }


    
}
