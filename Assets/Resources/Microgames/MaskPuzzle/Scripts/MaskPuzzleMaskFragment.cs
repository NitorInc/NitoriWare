using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    // TODO: Find them automagically; also, use a list
    [Header("The other mask fragments")]
    [SerializeField]
    private GameObject otherFragment1, otherFragment2;

    [Header("Mouse Grabbable Group")]
    [SerializeField]
    private GameObject mouseGrabbableGroup;

    // TODO: This setting should be somewhere else (so it doesn't have to be set for each fragment)
    [Header("How close the fragments need to be to snap together?")]
    [SerializeField]
    private float maxSnapDistance = 1f;

    // To be called when dropping a mask
    // Checks whether any other fragments are near the drop position
    // If yes, snaps this fragment to the other one(s) by making their positions equal
    // and becoming their parent so they are moved together in the future
    public void SnapToOtherFragments()
    {
        // debug stuff
        print("Positions for " + name);
        print("own: " + (Vector2)transform.position);
        print("other1: " + (Vector2)otherFragment1.transform.position);
        print("other2: " + (Vector2)otherFragment2.transform.position);

        // TODO: Use a list (to support variable number of fragments and get rid of duplicate code)
        if (Vector2.Distance(transform.position, otherFragment1.transform.position) <= maxSnapDistance)
        {
            transform.position = (Vector2)otherFragment1.transform.position;
            transform.parent = mouseGrabbableGroup.transform;
            otherFragment1.transform.parent = transform;
            print("Snapped " + name + " to " + otherFragment1.name + " and became its parent");
        }
        if (Vector2.Distance(transform.position, otherFragment2.transform.position) <= maxSnapDistance)
        {
            transform.position = (Vector2)otherFragment2.transform.position;
            transform.parent = mouseGrabbableGroup.transform;
            otherFragment2.transform.parent = transform;
            print("Snapped " + name + " to " + otherFragment2.name + " and became its parent");
        }

        // Check victory
        if (Vector2.Distance(transform.position, otherFragment1.transform.position) == 0 &&
            Vector2.Distance(transform.position, otherFragment2.transform.position) == 0)
        {
            MicrogameController.instance.setVictory(victory: true, final: true);
        }
    }

    // To be called when grabbing a mask
    // Checks whether this mask is a child of another mask
    // If yes, it reverses the relationship so it becomes the other mask's parent instead
    // This is done to ensure that both linked masks are moved together regardless of which one is grabbed
    // FIXME: Children aren't brought up to the front layer when grabbing the group (only the parent is),
    //        so they can be behind other fragments while being moved around
    public void SwapParents()
    {
        if (transform.parent != mouseGrabbableGroup.transform)
        {
            Transform otherMask = transform.parent;
            transform.parent = mouseGrabbableGroup.transform;
            otherMask.parent = transform;
        }
    }
}
