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

    public void Enlarge()
    {
        this.spriteRenderer.transform.localScale = new Vector3(enlargedScale, enlargedScale, 1);
    }

    public void ResetSize()
    {
        this.spriteRenderer.transform.localScale = new Vector3(1, 1, 1);
    }

}
