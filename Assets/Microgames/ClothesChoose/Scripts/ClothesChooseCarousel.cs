using System;
using System.Linq;
using UnityEngine;

public class ClothesChooseCarousel : MonoBehaviour
{
    public ClothesChooseClothing itemTemplate;
    public Sprite[] itemSprites;
    public Transform mountPoint;

    public GameObject[] arrows;

    public int layerOrder;
    public float spacing;
    public float yOffset;
    public float minScale;

    ClothesChooseClothing[] items;

    int currentIndex;
    bool listen;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        transform.position = mountPoint.position;

        items = new ClothesChooseClothing[2];
        for (int i = 0; i < items.Length; i++)
        {
            ClothesChooseClothing item = Instantiate(itemTemplate, mountPoint);
            item.SetSprite(itemSprites[i], layerOrder);
            item.gameObject.SetActive(false);
            items[i] = item;
        }

        currentIndex = UnityEngine.Random.Range(0, items.Length);
    }

    void OnGUI()
    {
        if (listen)
        {
            if (Event.current.Equals(Event.KeyboardEvent("right")))
            {
                animator.SetTrigger("ChooseRight");
                Reposition(1);
                Deactivate();
            }
            else if (Event.current.Equals(Event.KeyboardEvent("left")))
            {
                animator.SetTrigger("ChooseLeft");
                Reposition(-1);
                Deactivate();
            }

            SendMessageUpwards("CheckWin");
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
            float itemXPosition = spacing * i;
            float itemYPosition = 0;
            float itemScale = minScale * Math.Abs(i);
            if (itemScale == 0)
                itemScale = 1;
            else
                itemYPosition = yOffset;

            ClothesChooseClothing item = items[GetIndex(currentIndex, i)];
            //item.gameObject.SetActive(true);
            item.transform.localPosition = new Vector2(itemXPosition, itemYPosition);
            item.transform.localScale = new Vector2(itemScale, itemScale);
        }
    }

    public void Deactivate()
    {
        listen = false;
        
        foreach (ClothesChooseClothing item in items)
        {
            item.gameObject.SetActive(false);
        }

        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(false);
        }
    }

    public void Activate()
    {
        listen = true;
        ArrangeItems();

        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(true);
        }
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}
