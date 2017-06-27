using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperThiefJunkToss : MonoBehaviour
{

#pragma warning disable 0649   //Serialized Fields
    [SerializeField]
    private GameObject junkPrefab, gunPrefab;
    [SerializeField]
    private List<Sprite> junkPieces;
    [SerializeField]
    private float timeBetweenJunk;
#pragma warning restore 0649

    private bool activated;
    private float junkTimer;

	void Start()
	{
        activated = false;
	}

    void Update()
    {
        if (activated)
        {
            junkTimer -= Time.deltaTime;
            if (junkTimer <= 0f)
                createJunk();
        }
	}

    public void activateToss()
    {
        activated = true;
        createJunk();
    }

    public void createGun()
    {
        GameObject.Instantiate(junkPrefab, transform.position, Quaternion.identity);
    }

    void createJunk()
    {
        GameObject.Instantiate(junkPrefab, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().sprite = junkPieces[0];
        junkPieces.RemoveAt(0);
        junkTimer += timeBetweenJunk;
    }
}
