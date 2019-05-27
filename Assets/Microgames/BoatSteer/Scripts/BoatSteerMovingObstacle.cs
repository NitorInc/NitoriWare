using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteerMovingObstacle : MonoBehaviour
{
    [SerializeField]
    private float moveChance = .2f;
    [SerializeField]
    private SineWave sineWave;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite movingSprite;
    
	void Start ()
    {
        if (Random.Range(0f,1f) <= moveChance)
        {
            sineWave.xOffset = Random.Range(0f, 100f);
            sineWave.enabled = true;
            spriteRenderer.sprite = movingSprite;
        }
	}
}
