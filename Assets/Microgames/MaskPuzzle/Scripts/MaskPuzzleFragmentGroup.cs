using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to keep track of which fragments are connected and determine a fragment's group in constant time
public class MaskPuzzleFragmentGroup
{
    public List<MaskPuzzleMaskFragment> fragments;

    public Camera assignedCamera;

    public MaskPuzzleFragmentGroup(MaskPuzzleMaskFragment initialFragment)
    {
        fragments = new List<MaskPuzzleMaskFragment>();
        fragments.Add(initialFragment);

        // Create a new camera for this group by cloning the main camera
        // A separate camera for each group is needed so they don't clip into each other
        // Drawing order is determined by the camera depth
        assignedCamera = Camera.Instantiate(MainCameraSingleton.instance);
        assignedCamera.clearFlags = CameraClearFlags.Depth;
        assignedCamera.cullingMask = 1 << initialFragment.gameObject.layer;
        assignedCamera.depth = .1f;
    }

    //Connect this group to another fragment group
    public void connectTo(MaskPuzzleFragmentGroup group)
    {
        if (group == this)
            return;

        group.assignedCamera.enabled = false;
        assignedCamera.cullingMask |= group.assignedCamera.cullingMask;

        fragments.AddRange(group.fragments);
        foreach (var fragment in group.fragments)
        {
            fragment.fragmentGroup = this;
        }
    }

    // To be called when dropping a mask
    // Checks whether any other fragments are near the drop position
    // If yes, snaps the fragments of this group to them by making their positions equal
    // and joins their groups
    public bool SnapToOtherFragments()
    {
        bool connected = false;
        for (int i=0; i<fragments[0].fragmentsManager.fragments.Count; i++)
        {
            MaskPuzzleMaskFragment checkedFragment = fragments[0].fragmentsManager.fragments[i];

            // Check if already connected
            if (fragments.Contains(checkedFragment))
                continue;

            // Check distance
            if (Vector2.Distance(fragments[0].transform.position, checkedFragment.transform.position)
                    > fragments[0].fragmentsManager.maxSnapDistance)
                continue;

            // Check if the grabbed fragment can be connected to the i-th fragment,
            // directly or through other already connected fragments
            Debug.Log("Checking connectability; grabbed group count: " + fragments.Count + "; checked group count: "
                + checkedFragment.fragmentGroup.fragments.Count);
            if (fragments[0].fragmentsManager.edges.areConnectable(
                this, checkedFragment.fragmentGroup))
            {
                // Make positions equal and connect the groups
                foreach (MaskPuzzleMaskFragment fragment in fragments)
                {
                    fragment.transform.position = checkedFragment.transform.position;
                    Debug.Log("Snapped " + fragment.name + " to " + checkedFragment.name);
                }
                connectTo(checkedFragment.fragmentGroup);
                Debug.Log("Now the group contains " + fragments.Count + " fragments.");
                connected = true;
            }
        }
        return connected;
    }
}
