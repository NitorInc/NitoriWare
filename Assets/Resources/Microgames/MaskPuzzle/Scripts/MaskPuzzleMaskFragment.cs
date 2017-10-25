using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    // ugly, but will do for now
    [Header("The other mask fragments")]
    [SerializeField]
    private GameObject otherFragment1, otherFragment2;

    [Header("How close the fragments need to be to snap together?")]
    [SerializeField]
    private float maxSnapDistance = 1f;

    // To be called when dropping a mask
    // Checks whether any other fragments are near the drop position
    // If yes, snaps this fragment to the other one
    public void SnapToOtherFragments()
    {
        // debug stuff
        print("Positions for " + name);
        print("own: " + (Vector2)transform.position);
        print("other1: " + (Vector2)otherFragment1.transform.position);
        print("other2: " + (Vector2)otherFragment2.transform.position);

        // TODO: use a list or something
        if (Vector2.Distance(transform.position, otherFragment1.transform.position) <= maxSnapDistance)
        {
            transform.position = (Vector2)otherFragment1.transform.position;
        }
        else if (Vector2.Distance(transform.position, otherFragment2.transform.position) <= maxSnapDistance)
        {
            transform.position = (Vector2)otherFragment2.transform.position;
        }
    }
}
