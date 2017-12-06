using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    public MaskPuzzleGrabbableFragmentsManager fragmentsManager;

    // Callbacks for drag and drop events
    public void OnGrab()
    {
        SwapParents();
    }

    public void OnRelease()
    {
        SnapToOtherFragments();
        CheckVictory();
    }

    // To be called when dropping a mask
    // Checks whether any other fragments are near the drop position
    // If yes, snaps this fragment to the other one(s) by making their positions equal
    // and becoming their parent so they are moved together in the future
    void SnapToOtherFragments()
    {
        for (int i=0; i<fragmentsManager.fragments.Count; i++)
        {
            if (fragmentsManager.fragments[i] == this)
                continue;
            if (Vector2.Distance(transform.position, fragmentsManager.fragments[i].transform.position)
                <= fragmentsManager.maxSnapDistance)
            {
                transform.position = (Vector2)fragmentsManager.fragments[i].transform.position;
                transform.parent = fragmentsManager.transform;
                fragmentsManager.fragments[i].transform.parent = transform;
                print("Snapped " + name + " to " + fragmentsManager.fragments[i].name + " and became its parent");
            }
        }
    }

    // To be called when grabbing a mask
    // Checks whether this mask is a child of another mask
    // If yes, it reverses the relationship so it becomes the other mask's parent instead
    // This is done to ensure that both linked masks are moved together regardless of which one is grabbed
    // FIXME: Children aren't brought up to the front layer when grabbing the group (only the parent is),
    //        so they can be behind other fragments while being moved around
    void SwapParents()
    {
        if (transform.parent != fragmentsManager.transform)
        {
            Transform otherMask = transform.parent;
            transform.parent = fragmentsManager.transform;
            otherMask.parent = transform;
        }
    }

    // To be called after dropping a fragment and snapping to other fragments
    // Check and handle victory condition
    void CheckVictory()
    {
        // If the distance to any of the other masks > 0, we haven't won yet
        for (int i=0; i<fragmentsManager.fragments.Count; i++)
            if (Vector2.Distance(transform.position, fragmentsManager.fragments[i].transform.position) > 0)
                return;

        // Victory!
        MicrogameController.instance.setVictory(victory: true, final: true);
        // Calculate speed values for the final mask movement
        // Time must be positive or Bad Things will happen
        if (fragmentsManager.victoryMoveTime <= 0)
            fragmentsManager.victoryMoveTime = 0.001f;
        fragmentsManager.victoryMoveSpeed = Vector2.Distance(transform.position, fragmentsManager.victoryGoal) / fragmentsManager.victoryMoveTime;
        fragmentsManager.victoryRotationSpeed = (transform.rotation.z - fragmentsManager.victoryRotation) / fragmentsManager.victoryMoveTime;
    }

    // Called every frame
    // Move and rotate the mask when victory achieved
    void Update()
    {
        // Only the direct children of the manager need to be moved
        if (MicrogameController.instance.getVictory() && transform.parent == fragmentsManager.transform)
        {
            if (MathHelper.moveTowards2D(transform, fragmentsManager.victoryGoal, fragmentsManager.victoryMoveSpeed))
            {
                // Arrived at the final location, set rotation to the final value too
                Vector3 rotation = transform.eulerAngles;
                rotation.z = fragmentsManager.victoryRotation;
                transform.eulerAngles = -rotation;
            }
            else
            {
                transform.Rotate(0f, 0f, fragmentsManager.victoryRotationSpeed * Time.deltaTime);
            }
        }
    }
}
