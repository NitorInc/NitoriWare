using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashBlackBars : MonoBehaviour
{
    [SerializeField]
    private Transform topBar;
    [SerializeField]
    private Transform bottomBar;
    [SerializeField]
    private float bottomBarDistanceMult = .5f;

    [SerializeField]
    private float closeSpeed = 1f;
    public float CloseSpeed
    {
        get { return closeSpeed; }
        set { closeSpeed = value; }
    }
    [SerializeField]
    private float openSpeed = 1f;
    public float OpenSpeed
    {
        get { return openSpeed; }
        set { openSpeed = value; }
    }

    private float goalY;
	
	void Update ()
    {
        float currentY = getY();
		if (currentY != goalY)
            setY(Mathf.MoveTowards(currentY, goalY, Time.deltaTime * (getSpeedToGoal())));
	}

    void setY(float y)
    {
        topBar.position = Vector3.down * y;
        bottomBar.position = -topBar.position * bottomBarDistanceMult;
    }

    float getY() => -topBar.position.y;

    public void setGoalY(float goalY)
    {
        this.goalY = goalY;
        if (getSpeedToGoal() <= 0f)
            setY(goalY);
    }

    float getSpeedToGoal() => goalY > getY() ? closeSpeed : openSpeed;

}
