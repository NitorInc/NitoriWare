using UnityEngine;
using System.Collections;

public class VictorySprite : MonoBehaviour
{
    [SerializeField]
    private bool ignoreIfNull = false;

	public Sprite normalSprite, victorySprite, failureSprite;

	private SpriteRenderer _spriteRenderer;

	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if (MicrogameController.instance.getVictoryDetermined())
		{
			if (MicrogameController.instance.getVictory())
				setSprite(victorySprite);
			else
				setSprite(failureSprite);
		}
		else
			setSprite(normalSprite);
	}

	void setSprite(Sprite sprite)
	{
		if (_spriteRenderer != null && !(ignoreIfNull && sprite == null))
		{
			_spriteRenderer.sprite = sprite;
		}
	}
}
