using UnityEngine;

public class OkuuFireVictory : MonoBehaviour
{
    private SpriteRenderer victoryImage;

    void Start()
    {
        this.victoryImage = this.GetComponent<SpriteRenderer>();
        this.victoryImage.enabled = false;
    }
    
	void Victory()
    {
        victoryImage.enabled = true;
    }
}
