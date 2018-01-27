using System;
using System.Linq;
using UnityEngine;

public class ClothesChooseCarousel : MonoBehaviour
{
    public ClothesChooseClothing[] items;

    public float spacing;
    public float minScale;

    int currentIndex;
    bool listen;

    void Start()
    {
        ArrangeItems();
    }

    void OnGUI()
    {
        if (listen)
        {
            if (Event.current.Equals(Event.KeyboardEvent("right")))
            {
                Reposition(-1);
            }
            else if (Event.current.Equals(Event.KeyboardEvent("left")))
            {
                Reposition(1);
            }
        }
    }

    void Reposition(int offset)
    {
        currentIndex = GetIndex(currentIndex, offset);
        ArrangeItems();
    }

    int GetIndex(int index, int offset)
    {
        int newIndex = index + offset;
        if (newIndex < 0)
            newIndex = items.Length + newIndex;
        else if (newIndex >= items.Length)
            newIndex = 0 + (newIndex - items.Length);
        
        return newIndex;
    }

    void ArrangeItems()
    {
        foreach (ClothesChooseClothing item in items)
        {
            item.gameObject.SetActive(false);
        }
        
        foreach (int i in Enumerable.Range(-1, 3))
        {
            float itemPosition = spacing * i;
            float itemScale = minScale * Math.Abs(i);
            if (itemScale == 0)
                itemScale = 1;

            ClothesChooseClothing item = items[GetIndex(currentIndex, i)];
            item.gameObject.SetActive(true);
            item.transform.localPosition = new Vector2(itemPosition, 0);
            item.transform.localScale = new Vector2(itemScale, itemScale);
        }
    }

    public void Deactivate()
    {
        listen = false;
    }

    public void Activate()
    {
        listen = true;
    }

    public ClothesChooseClothing GetCurrentItem()
    {
        return items[currentIndex];
    }
}
