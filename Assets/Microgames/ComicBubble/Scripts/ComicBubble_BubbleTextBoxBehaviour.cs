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

    private GameObject target;

    private AdvancingText textObject;

    [SerializeField]
    private float textSpeed;            // Has to be setted outside of class 

    // Use this for initialization
    void Start () {
        textObject = GetComponentInChildren<AdvancingText>();
        stopSpeechText();
    }

    // Update is called once per frame
    void Update () {

        if (textSpeed > 0)
        {

            RaycastHit2D[] result = Physics2D.BoxCastAll(castOffset + (Vector2)transform.position, new Vector3(castWidth, castHeight, 1), 0, Vector2.zero);

            if (result.Length > 0)
                foreach (RaycastHit2D r in result)
                    if (r.collider.gameObject == target)
                        advanceSpeechText();
            else
                stopSpeechText();

        }

	}

    


    public void setTextSpeed(float speed)
    {
        textSpeed = speed * GetComponentInChildren<AdvancingText>().GetComponent<TMPro.TMP_Text>().text.Length;
    }

    public void setTarget(GameObject target)
    {
        this.target = target;
    }

    // Stop text from showing
    void stopSpeechText()
    {
        textObject.setAdvanceSpeed(0);
    }

    // Advance text
    void advanceSpeechText()
    {
        textObject.setAdvanceSpeed(textSpeed);
    }

    // This is for showing where the square box cast is being placed
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(castOffset + (Vector2)transform.position, new Vector3(castWidth, castHeight, 1));
    }

}
