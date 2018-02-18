using UnityEngine;

public class ClothesChooseClothing : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite, int sortingOrder)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = sortingOrder;
    }
}
