using UnityEngine;

public class ClothesChooseDoll : MonoBehaviour
{
    [System.Serializable]
    public struct Outfit
    {
        public Sprite hat;
        public Sprite top;
        public Sprite bottom;
    }
    
    public SpriteRenderer hatRenderer;
    public SpriteRenderer topRenderer;
    public SpriteRenderer bottomRenderer;

    public Outfit[] outfits;
    public bool wearClothes;

    int outfitIndex;

    void Start()
    {
        if (wearClothes)
        {
            outfitIndex = UnityEngine.Random.Range(0, outfits.Length);
            Outfit outfit = outfits[outfitIndex];

            hatRenderer.sprite = outfit.hat;
            topRenderer.sprite = outfit.top;
            bottomRenderer.sprite = outfit.bottom;
        }
    }

    public int GetOutfitIndex()
    {
        return outfitIndex;
    }
}
