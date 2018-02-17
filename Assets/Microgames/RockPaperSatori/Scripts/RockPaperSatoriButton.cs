using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperSatoriButton : MonoBehaviour
{
    [SerializeField]
    private RockPaperSatoriController.Move move;
    [SerializeField]
    private Collider2D clickCollider;
    [SerializeField]
    private RockPaperSatoriController controller;

    [Header("Hover scaling values")]
    [SerializeField]
    private float defaultScale;
    [SerializeField]
    private float hoverScale;
    [SerializeField]
    private float scaleLerpSpeed;

    void Start ()
    {
		
	}
	
	void Update ()
    {
        //Do nothing if game hasn't started
        if (!controller.isGameStarted())
            return;

        //Hover scaling
        bool hovering = CameraHelper.isMouseOver(clickCollider);
        float diff = scaleLerpSpeed * Time.deltaTime;
        float newScale = transform.localScale.x;

        //Lerp to appropriate scale
        if (hovering && transform.localScale.x < hoverScale)
            newScale = Mathf.Min(newScale + diff, hoverScale);
        else if (!hovering && transform.localScale.x > defaultScale)
            newScale = Mathf.Max(newScale - diff, defaultScale);

        transform.localScale = Vector3.one * newScale;

        //Click detection
        if (Input.GetMouseButtonDown(0) && CameraHelper.isMouseOver(clickCollider))
            click();
	}

    void click()
    {
        transform.parent.gameObject.SetActive(false);
        controller.makeMove(move);
    }
}
