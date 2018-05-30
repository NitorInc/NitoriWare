using UnityEngine;

public class OkuuFireVictory : MonoBehaviour
{
    private SpriteRenderer victoryImage;
    private AudioSource victorySource;

    void Start()
    {
        victoryImage = GetComponent<SpriteRenderer>();
        victoryImage.enabled = false;
        victorySource = GetComponent<AudioSource>();
    }
    
	void Victory()
    {
        victoryImage.enabled = true;
        victorySource.Play();
    }
}
