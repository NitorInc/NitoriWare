using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    //[Header("Fragments Manager")]
    //[SerializeField]
    public MaskPuzzleGrabbableFragmentsManager fragmentsManager;

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
        for (int i=0; i<fragmentsManager.fragments.Count; i++)
        {
            if (fragmentsManager.fragments[i] == this)
                continue;
            if (Vector2.Distance(transform.position, fragmentsManager.fragments[i].transform.position) <= maxSnapDistance)
            {
                transform.position = (Vector2)fragmentsManager.fragments[i].transform.position;
                transform.parent = fragmentsManager.transform;
                fragmentsManager.fragments[i].transform.parent = transform;
                print("Snapped " + name + " to " + fragmentsManager.fragments[i].name + " and became its parent");
            }
        }

        CheckVictory();
    }

    // To be called when grabbing a mask
    // Checks whether this mask is a child of another mask
    // If yes, it reverses the relationship so it becomes the other mask's parent instead
    // This is done to ensure that both linked masks are moved together regardless of which one is grabbed
    // FIXME: Children aren't brought up to the front layer when grabbing the group (only the parent is),
    //        so they can be behind other fragments while being moved around
    public void SwapParents()
    {
        if (transform.parent != fragmentsManager.transform)
        {
            Transform otherMask = transform.parent;
            transform.parent = fragmentsManager.transform;
            otherMask.parent = transform;
        }
    }

    // To be called after dropping a fragment and snapping to other fragments
    // Check if victory condition has been achieved
    // If distance to each other mask is 0, we have won
    void CheckVictory()
    {
        for (int i=0; i<fragmentsManager.fragments.Count; i++)
            if (Vector2.Distance(transform.position, fragmentsManager.fragments[i].transform.position) > 0)
                return;
        MicrogameController.instance.setVictory(victory: true, final: true);
    }
}
