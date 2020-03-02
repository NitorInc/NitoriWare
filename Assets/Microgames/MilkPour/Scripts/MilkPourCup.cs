using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPourCup : MonoBehaviour {
	
	[SerializeField]
	private SpriteRenderer glassSprite;
	
	[SerializeField]
	private SpriteRenderer tintSprite;
	
	[SerializeField]
	private SpriteRenderer fillSprite;
	
	[SerializeField]
	private SpriteRenderer lowerFillLineSprite;
	
	[SerializeField]
	private SpriteRenderer upperFillLineSprite;
	
	[SerializeField]
	private float initialGlassHeight;
	
	[SerializeField]
	private float initialLowerFillLineHeight;
	
	[SerializeField]
	private float initialUpperFillLineHeight;
	
	[SerializeField]
	private float fillPerSecond;

    [SerializeField]
    protected MilkPourPourSpeedAnimation animationSpeedMult;

	/// <summary>
	/// 	Get/set the height of the glass.
	/// </summary>
	protected float GlassHeight
	{
		get
		{
			return glassSprite.size.y;
		}
		set
		{
			glassSprite.size = new Vector2(glassSprite.size.x, value);
			tintSprite.size = new Vector2(tintSprite.size.x, value);
		}
	}

	/// <summary>
	/// 	Get/set the height of the fill.
	/// </summary>
	protected float FillHeight
	{
		get
		{
			return fillSprite.size.y;
		}
		set
		{
			fillSprite.size = new Vector2(fillSprite.size.x, value);
		}
	}

	/// <summary>
	/// 	Get/set the height of the lower fill line.
	/// </summary>
	protected float LowerFillLineHeight
	{
		get
		{
			return lowerFillLineSprite.size.y;
		}
		set
		{
			lowerFillLineSprite.size = new Vector2(lowerFillLineSprite.size.x, value);
            var particleChild = lowerFillLineSprite.transform.GetChild(0);
            particleChild.localPosition = new Vector3(
                particleChild.localPosition.x,
                value - .05f,
                particleChild.localPosition.z);
        }
	}

	/// <summary>
	/// 	Get/set the height of the upper fill line.
	/// </summary>
	protected float UpperFillLineHeight
	{
		get
		{
			return upperFillLineSprite.size.y;
		}
		set
		{
			upperFillLineSprite.size = new Vector2(upperFillLineSprite.size.x, value);
            var particleChild = upperFillLineSprite.transform.GetChild(0);
            particleChild.localPosition = new Vector3(
                particleChild.localPosition.x,
                value - .05f,
                particleChild.localPosition.z);
        }
	}
	
	/// <summary>
	/// 	If true, the cup will not fill when Fill() is called.
	/// </summary>
	public bool Stopped { get; private set; }

	/// <summary>
	/// 	Returns whether the cup has been filled between the fill lines.
	/// </summary>
	public bool IsPassing()
	{
		return FillHeight >= LowerFillLineHeight && FillHeight <= UpperFillLineHeight;
	}
	
	/// <summary>
	/// 	Returns whether the cup has been filled over the glass height to trigger an insta-fail.
	/// </summary>
	public bool IsSpilled()
	{
		return FillHeight >= GlassHeight;
	}

	/// <summary>
	/// 	Returns whether the cup has been filled beyond the max fill line.
	/// </summary>
	public bool IsOverfilled()
	{
		return FillHeight > UpperFillLineHeight;
	}
	
	/// <summary>
	/// 	Fills the cup by an amount proportionate to the given deltaTime.
	/// </summary>
	public virtual void Fill(float deltaTime)
	{
        //if (!Stopped)
            FillHeight = Mathf.Min(FillHeight + fillPerSecond * animationSpeedMult.PourSpeedMult * deltaTime, GlassHeight);
	}

	/// <summary>
	/// 	Stops the cup from filling/growing further.
	/// </summary>
	public void Stop()
	{
		Stopped = true;
	}

	private void Start()
	{
		GlassHeight = initialGlassHeight;
		FillHeight = 0;
		LowerFillLineHeight = initialLowerFillLineHeight;
		UpperFillLineHeight = initialUpperFillLineHeight;
	}
}
