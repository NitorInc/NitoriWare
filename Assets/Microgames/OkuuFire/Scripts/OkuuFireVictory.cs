using UnityEngine;

public class OkuuFireVictory : MonoBehaviour
{
    private SpriteRenderer victoryImage;
    private AudioSource victorySource;

    void Start()
    {
        this.victoryImage = this.GetComponent<SpriteRenderer>();
        this.victoryImage.enabled = false;
        victorySource = GetComponent<AudioSource>();
    }
    
	void Victory()
    {
        victoryImage.enabled = true;
        victorySource.Play();
    }
}
