using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefJunkToss : MonoBehaviour
{

#pragma warning disable 0649
    [SerializeField]
    private GameObject junkPrefab, gunPrefab;
    [SerializeField]
    private List<Sprite> junkPieces;
    [SerializeField]
    private float disanceBetweenJunk;
#pragma warning restore 0649

    private bool activated;
    private float junkTimer;
    private Vector3 lastPosition;

	void Start()
	{
        activated = false;
        lastPosition = transform.position;
	}

    void Update()
    {
        if (activated)
        {
            junkTimer -= ((Vector2)(transform.position - lastPosition)).magnitude;
            if (junkTimer <= 0f)
                createJunk();
        }
        lastPosition = transform.position;
	}

    public void activateToss()
    {
        activated = true;
        createJunk();
    }

    public void createGun()
    {
        GameObject.Instantiate(gunPrefab, transform.position, Quaternion.identity).name = "PaperThiefGun";
    }

    void createJunk()
    {
        GameObject.Instantiate(junkPrefab, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().sprite = junkPieces[0];
        junkPieces.RemoveAt(0);
        junkTimer += disanceBetweenJunk;
        if (junkPieces.Count == 0)
            enabled = false;
    }
}
