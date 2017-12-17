using UnityEngine;

public class HecShapesCelestialBody : MonoBehaviour
{

    public HecShapesSlottable.Shape shape;
    public Sprite graySprite;

    SpriteRenderer spriteRenderer;
    Sprite defaultSprite;

    void Start()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.defaultSprite = this.spriteRenderer.sprite;
    }

    public void MakeGray()
    {
        GetComponent<SpriteRenderer>().sprite = this.graySprite;
    }

    public void ResetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = this.defaultSprite;
    }

}
