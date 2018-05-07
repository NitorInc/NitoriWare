using UnityEngine;

public class HecShapesCelestialBody : MonoBehaviour
{

    public HecShapesSlottable.Shape shape;
    public Sprite graySprite;

    public float enlargedScale;

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

    public void Enlarge()
    {
        spriteRenderer.transform.localScale = new Vector3(enlargedScale, enlargedScale, 1);
    }

    public void ResetSize()
    {
        spriteRenderer.transform.localScale = new Vector3(1, 1, 1);
    }

}
