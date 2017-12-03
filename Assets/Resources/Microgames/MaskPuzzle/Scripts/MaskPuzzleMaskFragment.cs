using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPuzzleMaskFragment : MonoBehaviour {

    public MaskPuzzleGrabbableFragmentsManager fragmentsManager;

    public List<GameObject> connectedTo = new List<GameObject>();

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

    public void ConnectTo(GameObject otherFragment)
    {
        connectedTo.Add(otherFragment);
        otherFragment.GetComponent<MaskPuzzleMaskFragment>().connectedTo.Add(this.gameObject);
    }

    public List<GameObject> GetAllConnected()
    {
        List<GameObject> connected = new List<GameObject>();
        AddAllConnected(connected);
        return connected;
    }

    public void AddAllConnected(List<GameObject> connected)
    {
        if (connected.Contains(gameObject))
            return;
        connected.Add(gameObject);
        foreach (GameObject node in connectedTo)
            node.GetComponent<MaskPuzzleMaskFragment>().AddAllConnected(connected);
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
                + fragmentsManager.fragments[i].GetComponent<MaskPuzzleMaskFragment>().GetAllConnected().Count);
            if (fragmentsManager.edges.areConnectable(
                GetAllConnected(), fragmentsManager.fragments[i].GetComponent<MaskPuzzleMaskFragment>().GetAllConnected()))
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
        foreach (GameObject otherFragment in GetAllConnected())
        {
            if (otherFragment == gameObject)
                continue;
            otherFragment.transform.parent = transform;
        }
    }

    // To be called when grabbing a mask, after SwapParents()
    // The grabbed fragment is brought up to the front layer automatically by MouseGrabbableGroup,
    // but the other linked fragments need to be brought to the front manually - this method does this
    void MoveChildrenToFront()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            print("MoveChildrenToFront(): " + child.gameObject + " moved to the front.");
            fragmentsManager.GetComponent<MouseGrabbableGroup>().moveToFront(child.gameObject.GetComponent<MouseGrabbable>());
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
