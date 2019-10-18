using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Microgame Assets/RumiaRescue/Position Data")]
public class RumiaRescuePositionData : ScriptableObject {
    [SerializeField]
    private Vector3 rumiaPosition;
    public Vector3 RumiaPosition => rumiaPosition;


    [SerializeField]
    private Vector3[] needHelpPositionList;
    public Vector3[] NeedHelpPositionList => needHelpPositionList;

    [SerializeField]
    private Vector3[] obstaclePositionList;
    public Vector3[] ObstaclePositionList => obstaclePositionList;

    public int GetNeedHelpListLength() => needHelpPositionList.Length;

    public int GetObstacleListLength() => obstaclePositionList.Length;

}
