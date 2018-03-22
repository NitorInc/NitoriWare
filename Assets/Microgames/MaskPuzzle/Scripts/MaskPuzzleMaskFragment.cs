using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    public MaskPuzzleGrabbableFragmentsManager fragmentsManager;

    public FragmentGroup fragmentGroup;

    //Class to keep track of which fragments are connected and determine a fragment's group in constant time
    public class FragmentGroup
    {
        public List<MaskPuzzleMaskFragment> fragments;

        public Camera assignedCamera;

        public FragmentGroup(MaskPuzzleMaskFragment initialFragment)
        {
            fragments = new List<MaskPuzzleMaskFragment>();
            fragments.Add(initialFragment);
        }

        //Connect this group to another fragment group
        public void connectTo(FragmentGroup group)
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
        // If yes, snaps this fragment to the other one(s) by making their positions equal
        // and becoming their parent so they are moved together in the future
        public void SnapToOtherFragments()
        {
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
                print("Checking connectability; grabbed group count: " + fragments.Count + "; checked group count: "
                    + checkedFragment.fragmentGroup.fragments.Count);
                if (fragments[0].fragmentsManager.edges.areConnectable(
                    this, checkedFragment.fragmentGroup))
                {
                    // Make positions equal and connect the groups
                    foreach (MaskPuzzleMaskFragment fragment in fragments)
                    {
                        fragment.transform.position = checkedFragment.transform.position;
                        print("Snapped " + fragment.name + " to " + checkedFragment.name);
                    }
                    connectTo(checkedFragment.fragmentGroup);
                    print("Now the group contains " + fragments.Count + " fragments.");
                }
            }
        }
    }

    void Start()
    {
        fragmentGroup = new FragmentGroup(this);
        fragmentGroup.assignedCamera = Instantiate(Camera.main);
        fragmentGroup.assignedCamera.GetComponent<AudioListener>().enabled = false;
        fragmentGroup.assignedCamera.clearFlags = CameraClearFlags.Depth;
        fragmentGroup.assignedCamera.cullingMask = 1 << gameObject.layer;
        fragmentGroup.assignedCamera.depth = 0;
    }

    // Called every frame
    // Move and rotate the mask when victory achieved
    void Update()
    {
        if (MicrogameController.instance.getVictory())
        {
            if (Time.time > fragmentsManager.victoryStartTime + fragmentsManager.victoryMoveTime)
            {
                // Animation time elapsed, set animated variables to the final values
                transform.position = fragmentsManager.victoryGoal;
                transform.eulerAngles = fragmentsManager.victoryRotation;
                fragmentsManager.backgroundImage.color = Color.white;
            }
            else
            {
                float timeFactor = (Time.time - fragmentsManager.victoryStartTime) /
                                   fragmentsManager.victoryMoveTime;
                transform.position = Vector3.Lerp(
                    fragmentsManager.victoryStartPosition,
                    fragmentsManager.victoryGoal,
                    1 - Mathf.Pow(1 - timeFactor, 2)
                );
                transform.eulerAngles = Vector3.Slerp(
                    fragmentsManager.victoryStartRotation,
                    fragmentsManager.victoryRotation,
                    1 - Mathf.Pow(1 - timeFactor, 2)
                );
                fragmentsManager.backgroundImage.color = Color.Lerp(
                    fragmentsManager.victoryStartBgColor,
                    Color.white,
                    timeFactor * 4
                );
            }
        }
    }
}
