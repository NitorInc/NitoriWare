using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePartsExpressionManager : MonoBehaviour {

	[SerializeField]
	private List<Sprite> expressionSprites = new List<Sprite>();

	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
