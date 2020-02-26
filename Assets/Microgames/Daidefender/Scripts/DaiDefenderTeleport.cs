using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DaiDefenderTeleport : MonoBehaviour
{

    [SerializeField]
    private Direction[] directions;
    private Direction currentDirection;
    [SerializeField]
    private float yBehindThreshold;
    [SerializeField]
    private int behindSortingOrder;
    [SerializeField]
    private int frontSortingOrder;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float snapSpeed;
    [SerializeField]
    private float snapProximityPowerMult = 2f;

    [System.Serializable]
    public class Direction
    {
        public SpriteRenderer directionRenderer;
        public KeyCode keycode;
        public float TimePressed { get; set; }


    }

    private bool landed = true;

    private void Start()
    {
        currentDirection = directions.FirstOrDefault();
        transform.position = currentDirection.directionRenderer.transform.position;
    }

    void Update()
    {
        spriteRenderer.sortingOrder = transform.position.y < yBehindThreshold ? frontSortingOrder : behindSortingOrder;

        foreach (var direction in directions)
        {
            if (Input.GetKeyDown(direction.keycode) && landed)
            {
                direction.TimePressed = Time.time;
                if (direction != currentDirection)
                    landed = false;
            }
            //else
            //    direction.TimePressed = -1f;
        }
        var newCurrentDirection = directions
            .Where(a => a.TimePressed > 0f)
            .OrderByDescending(a => a.TimePressed)
            .FirstOrDefault();
        if (newCurrentDirection == null)
            newCurrentDirection = currentDirection;
        if (newCurrentDirection != currentDirection)
        {
            landed = false;
        }
        currentDirection = newCurrentDirection;
        if (currentDirection != null)
        {
            spriteRenderer.sprite = currentDirection.directionRenderer.sprite;
            if (transform.moveTowards2D(currentDirection.directionRenderer.transform.position,
                snapSpeed * Mathf.Pow((currentDirection.directionRenderer.transform.position - transform.position).magnitude, snapProximityPowerMult)))
            {
                landed = true;
            }
        }

    }

}
