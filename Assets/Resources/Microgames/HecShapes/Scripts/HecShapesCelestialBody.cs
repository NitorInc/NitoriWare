using UnityEngine;

public class HecShapesCelestialBody : MonoBehaviour
{

    public HecShapesSlottable.Shape shape;
    public Sprite graySprite;

    SpriteRenderer spriteRenderer;
    Sprite defaultSprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
    }

    public void MakeGray()
    {
        GetComponent<SpriteRenderer>().sprite = graySprite;
    }

    public void ResetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

}
