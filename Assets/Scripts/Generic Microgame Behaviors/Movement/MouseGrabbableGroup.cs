using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages a collection of MouseGrabbables that have a Renderer attached (or have one in its children)
//When one is grabbed, the order in layer is adjusted to be in front of the other ones

public class MouseGrabbableGroup : MonoBehaviour
{
	[SerializeField]
	private List<MouseGrabbable> grabbables;
	[SerializeField]
	private bool shuffleOnStart;
	[SerializeField]
	private int lowestOrderInLayer;

	void Start ()
	{
		if (shuffleOnStart)
			shuffle();
		updateOrderInLayer();
	}
	
	void LateUpdate ()
	{
		//Check for click
		if (Input.GetMouseButtonDown(0))
		{
			for (int i = 0; i < grabbables.Count; i++)
			{
				if (grabbables[i].grabbed)
				{
					moveToFront(i);
					break;
				}
			}
		}
	}

	void updateOrderInLayer()
	{
		for (int i = 0; i < grabbables.Count; i++)
		{
			grabbables[i].getRenderer().sortingOrder = lowestOrderInLayer + (grabbables.Count - 1 - i);
		}
	}

	void shuffle()
	{
		MouseGrabbable hold;
		int choose;
		for (int i = 0; i < grabbables.Count - 1; i++)
		{
			choose = Random.Range(i, grabbables.Count);
			hold = grabbables[i];
			grabbables[i] = grabbables[choose];
			grabbables[choose] = hold;
		}
	}

	void moveToFront(int index)
	{
		if (grabbables.Count == 0 || index == 0)
			return;

		MouseGrabbable hold = grabbables[index];
		for (int i = index; i > 0; i--)
		{
			grabbables[i] = grabbables[i - 1];
		}
		grabbables[0] = hold;

		updateOrderInLayer();
	}

    //Moves the given grabbable to the front, displaying it in front of the other grabbables in the group. Grabbable must be in the group.
    public void moveToFront(MouseGrabbable grabbable)
    {
        if (grabbables.Contains(grabbable))
            moveToFront(grabbables.IndexOf(grabbable));
        else
            Debug.LogError("Grabbable to move to front not found in group!");
    }

    //Adds a new grabbable to the group, bringing it to the front if specified
	public void addGrabbable(MouseGrabbable grabbable, bool toFront)
	{
		grabbables.Add(grabbable);
		if (toFront)
			moveToFront(grabbables.IndexOf(grabbable));
		updateOrderInLayer();
	}

	public List<MouseGrabbable> getGrabbables()
	{
		return grabbables;
	}

	public void setGrabbables(List<MouseGrabbable> grabbables)
	{
		this.grabbables = grabbables;
		updateOrderInLayer();
	}
}
