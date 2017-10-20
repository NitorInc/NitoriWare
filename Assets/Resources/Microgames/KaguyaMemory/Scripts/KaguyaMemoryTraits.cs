using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaguyaMemoryTraits : MicrogameTraits
{
  	public static int item;
    public static bool clicked;

    public GameObject[] clickableItems;

    private Sprite[] itemIcons;
    private int tickTime;
    private int manifestItemIndex;
    private bool completedManifest;

  	public override void onAccessInStage(string microgameId)
  	{
        base.onAccessInStage(microgameId);

        tickTime = 0;
        manifestItemIndex = 0;

        completedManifest = false;
        clicked = false;

        item = Random.Range(0, 4);
        // Debug.Log("Correct id: " + item);

        // Load all single sprites from the items multi-sprite
        itemIcons = Resources.LoadAll<Sprite>("Microgames/KaguyaMemory/Sprites/items");

        // Find the "treasure" item which will show the player which item we are looking for at the start
        GameObject.Find("wanted_item").GetComponent<SpriteRenderer>().sprite = itemIcons[item];
        GameObject.Find("wanted_item").GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(4, 6), 0, 0);

        Invoke("ManifestItems", 2);
  	}

    public void ManifestItems()
    {
        // Get the pregame background and the wanted item and set both to non-active
        GameObject.Find("pregame_background").SetActive(false);
        GameObject.Find("wanted_item").SetActive(false);

        // Find all clickable items in the stage and set their sprite and styleId
        clickableItems = GameObject.FindGameObjectsWithTag("Kaguya Clickable");

        // Debug.Log("Array length: " + clickableItems.Length);

        foreach (GameObject go in clickableItems)
        {
            manifestItemIndex++;
            int id = Random.Range(0, 4);

            // If last item in array and the wanted item has still not been put in, force it to be correct item
            if (manifestItemIndex == clickableItems.Length && !completedManifest) id = item;

            // If wanted item has already been put in but the random id and the wanted item id are the same then  modify the random id
            else if (completedManifest && id == item)
            {
                if (id-1 < 0 && id+1 < 4) id++;
                else if (id-1 > 0 && id+1 > 4) id--;
            }

            // Set sprite and properties of the Clickable prefab
            go.GetComponent<SpriteRenderer>().sprite = itemIcons[id];
            KaguyaMemoryClickable clickablePrefab = (KaguyaMemoryClickable) go.GetComponent(typeof(KaguyaMemoryClickable));
            clickablePrefab.SetStyleId(id);

            // If the wanted item id is equal to seeded item
            if (id == item) completedManifest = true;
        }
    }

}
