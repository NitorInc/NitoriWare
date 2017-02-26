using System.Collections.Generic;
using UnityEngine;

public class TouhouSortSorter : MonoBehaviour
{
	// Primary class for TouhouSort game handling
	// Handles objects and win/loss

	// Spacing between starting sortables
	static float GAP = 2.0f;

	// Max number of sortable touhous
	public int slotCount;

	public Transform stagingArea;
    public TouhouSortZoneManager zoneManager;
	//public TouhouSortTouhouBucket touhouBucket;
    public GameObject victoryDisplay;

	TouhouSortSortable[] touhous;
	Vector3[] slots;

	bool sorted;

    // enum of possible categories
    public enum Style
    {
        Hat,
        Bunny
    }

    [System.Serializable]
    public struct Category
    {
        public string name;
        public Sprite leftIcon, rightIcon;
        public TouhouSortSortable[] leftPool, rightPool;
    }
    public Category[] categories;
    
    void Start() {
        Category category = categories[Random.Range(0, categories.Length)];

        zoneManager.SetZoneAttributes(0, category.leftIcon, category.name, false);
        zoneManager.SetZoneAttributes(1, category.rightIcon, category.name, true);

        // Scoop up as many touhous as we can
        touhous = LoadTouhous (category, slotCount);
        slotCount = touhous.Length;

		sorted = false;

		// Fill starting slots with touhous
		CreateSlots ();
		FillSlots ();

		// Check the sort at the start, just in case
		CheckSort ();
	}

    TouhouSortSortable[] LoadTouhous(Category category, int amount)
    {
        List<TouhouSortSortable> touhous = new List<TouhouSortSortable>();

        foreach (TouhouSortSortable touhou in category.leftPool)
        {
            touhou.SetStyle(category.name);
            touhous.Add(touhou);
        }
        foreach (TouhouSortSortable touhou in category.rightPool)
        {
            touhous.Add(touhou);
        }
        
        // Scoop <amount> random touhous from the category
        if (amount > touhous.Count)
        {
            amount = touhous.Count;
        }

        MouseGrabbableGroup grabGroup = stagingArea.GetComponent<MouseGrabbableGroup>();
        TouhouSortSortable[] randomTouhous = new TouhouSortSortable[amount];

        for (int i = 0; i < amount; i++)
        {
            TouhouSortSortable touhou = touhous[Random.Range(0, touhous.Count)];
            MouseGrabbable grabbable = touhou.GetComponent<MouseGrabbable>();
            randomTouhous[i] = touhou;

            touhous.Remove(touhou);
            touhou.transform.parent = stagingArea;

            grabbable.onRelease.AddListener(CheckSort);

            grabGroup.addGrabbable(grabbable, true);
        }

        return randomTouhous;
    }

    void CreateSlots()
    {
		// Instantiate a list of Vector3 objects
		// which will be the starting spots of
		// our touhous
		slots = new Vector3[slotCount];
		Vector3 origin = stagingArea.position;

		if (slotCount % 2 == 0) {
			origin.x = origin.x + (GAP / 2);
		}

		for (int i = 0; i < slotCount; i++) {
			Vector3 slot = origin;
			int multiplier = (i + 1) / 2;

			if (i % 2 == 0) {
				slot.x = origin.x + (multiplier * GAP);
			}
			else {
				slot.x = origin.x - (multiplier * GAP);
			}

			slots [i] = slot;
		}
	}

	void FillSlots()
    {
		// Fill instantiated slots with sortable touhous
		for (int i = 0; i < slotCount; i++)
        {
			touhous [i].transform.position = slots [i];
		}
	}

	public void CheckSort()
    {
		// Check the current state of the sort
		// End the game if everything is sorted
		bool allSorted = true;

		foreach (TouhouSortSortable sortable in touhous)
        {
			bool thisSorted = false;

			// Get the touhou's current zone, if any
			TouhouSortDropZone currentZone = sortable.GetCurrentZone();
			
			if (currentZone)
            {
				thisSorted = currentZone.Belongs (sortable);
			}

			if (thisSorted != true)
            {
				allSorted = false;
				break;
			}
		}

		if (allSorted)
        {
			// Sorted
			Debug.Log("Sorted");
			sorted = true;

            victoryDisplay.SetActive(true);

			MicrogameController.instance.setVictory(true, true);
		}
		else if (sorted)
        {
			// Unsorted (wont ever happen)
			Debug.Log("Unsorted");
			sorted = false;

            victoryDisplay.SetActive(false);

            MicrogameController.instance.setVictory(false, true);
		}
	}

}
