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

    // Callbacks for drag and drop events
    public void OnGrab()
    {
        SwapParents();
        MoveChildrenToFront();
    }

    public void OnRelease()
    {
        SnapToOtherFragments();
        CheckVictory();
    }

    public void ConnectTo(MaskPuzzleMaskFragment otherFragment)
    {
        fragmentGroup.connectTo(otherFragment.fragmentGroup);
    }

    public List<MaskPuzzleMaskFragment> GetAllConnected()
    {
        return fragmentGroup.fragments;
    }

    // To be called when dropping a mask
    // Checks whether any other fragments are near the drop position
    // If yes, snaps this fragment to the other one(s) by making their positions equal
    // and becoming their parent so they are moved together in the future
    void SnapToOtherFragments()
    {
        for (int i=0; i<fragmentsManager.fragments.Count; i++)
        {
            // Check if already connected
            if (GetAllConnected().Contains(fragmentsManager.fragments[i]))
                continue;

            // Check distance
            if (Vector2.Distance(transform.position, fragmentsManager.fragments[i].transform.position)
                > fragmentsManager.maxSnapDistance)
                continue;

            // Check if the grabbed fragment can be connected to the i-th fragment,
            // directly or through other already connected fragments
            print("Checking connectability; grabbed group count: " + GetAllConnected().Count + "; checked group count: "
                + fragmentsManager.fragments[i].GetAllConnected().Count);
            if (fragmentsManager.edges.areConnectable(
                fragmentGroup, fragmentsManager.fragments[i].fragmentGroup))
            {
                transform.position = (Vector2)fragmentsManager.fragments[i].transform.position;
                ConnectTo(fragmentsManager.fragments[i]);
                // Become the parent of the newly joined fragments
                SwapParents();
                print("Snapped " + name + " to " + fragmentsManager.fragments[i].name);
                print("Now the group contains " + GetAllConnected().Count + " fragments.");
            }
        }
    }

    // To be called when grabbing a mask fragment and when joining new fragments
    // Make all connected fragments children of this fragment so they are moved together
    void SwapParents()
    {
        transform.parent = fragmentsManager.transform;
        foreach (MaskPuzzleMaskFragment otherFragment in GetAllConnected())
        {
            if (otherFragment == this)
                continue;
            otherFragment.transform.parent = transform;
        }
    }

    // To be called when grabbing a mask, after SwapParents()
    // Bring this fragment group to the front
    // by setting the assigned camera's depth higher than all other cameras' depths
    void MoveChildrenToFront()
    {
        fragmentGroup.assignedCamera.depth = (++fragmentsManager.topDepth);
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
