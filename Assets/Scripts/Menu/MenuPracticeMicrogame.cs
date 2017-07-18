using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPracticeMicrogame : MonoBehaviour
{

#pragma warning disable 0649	//Serialized Fields
    [SerializeField]
    private AnimationCurve shiftCurve;
    [SerializeField]
    private float shiftDuration;
#pragma warning restore 0649

    private static List<Stage.Microgame> microgamePool;

    private Vector3 initialScale, initialParentScale;
    private Vector3 initialPosition;

    private int microgameNumber;
    private float shiftStartTime;
    private bool isShifting, inCorrectMenu;
    private Stage.Microgame microgame;

	void Start()
	{
        if (microgamePool == null)
            microgamePool = GameController.instance.microgameCollection.getStageMicrogames(MicrogameCollection.Restriction.StageReady);

        microgameNumber = int.Parse(name.Split('(')[1].Split(')')[0]);
        if (microgameNumber >= microgamePool.Count)
        {
            gameObject.SetActive(false);
            return;
        }
        else
            microgame = microgamePool[microgameNumber];

        initialParentScale = transform.parent.localScale;
        initialScale = transform.localScale;
        initialPosition = transform.localPosition;

        //Vector2 displacement = transform.localPosition;

        //isShifting = false;
        //inCorrectMenu = GameMenu.subMenu == GameMenu.SubMenu.Practice;
        //if (!inCorrectMenu)
        //{   
        //    shiftStartTime = -shiftDuration;
        //    updateShift(1f);
        //}
	}
	
	void LateUpdate()
    {
        if (GameMenu.shifting)
        {
            float mult = initialParentScale.x / transform.parent.localScale.x;
            transform.localScale = initialScale * mult;

            transform.localPosition = transform.parent.localScale.x <= .011f ? Vector3.zero : initialPosition;
        }
        else
            transform.localScale = initialScale;

        ////Update shift status
        //if (!isShifting && GameMenu.shifting)
        //{
        //    isShifting = true;
        //    if (inCorrectMenu || GameMenu.subMenu == GameMenu.SubMenu.Practice) //We're either leaving our submenu or going to it
        //    {
        //        inCorrectMenu = !inCorrectMenu;
        //        shiftStartTime = Time.time;
        //    }
        //    else //Otherwise set shift out progress to 1
        //        shiftStartTime = -shiftDuration;
        //}

        //if (isShifting)
        //{
        //    float progress = getShiftProgress();
        //    if (progress >= 1f)
        //    {
        //        progress = 1f;
        //        isShifting = false;
        //    }
        //    updateShift(GameMenu.subMenu == GameMenu.SubMenu.Practice ? -progress : progress);
        //}
	}

    void updateShift(float progress)
    {
        //transform.localPosition = initialPosition +
        //    (Vector3)MathHelper.getVector2FromAngle(shiftAngle, shiftCurve.Evaluate(progress) * shiftDistance);
    }

    float getShiftProgress()
    {
        return ((Time.time - shiftStartTime) / shiftDuration);
    }
}
