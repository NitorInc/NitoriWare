using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RumiaRescueMapGenerater : MonoBehaviour {
    [SerializeField]
    private GameObject rumia;
    [SerializeField]
    private GameObject[] needHelpList;
    [SerializeField]
    private GameObject[] obstacleList;

    [SerializeField]
    private int needHelpCount;
    [SerializeField]
    private int obstacleCount;

    [SerializeField]
    private Transform needHelpGroupTF;
    [SerializeField]
    private Transform obstacleGroupTF;

    private const string POSITION_DATA_RELATIVE_PATH = "/Microgames/RumiaRescue/ScriptableObjects/";
    private const string POSITION_DATA_NAME_EXTENSION = ".asset";

    private List<FileInfo> positionDataFileList;

    private void Start() {
        ResetPositionDataFileList();

        // Find a good positionData
        RumiaRescuePositionData positionData = null;
        while(true) {
            if (positionDataFileList.Count <= 0)
                break;
            RumiaRescuePositionData tempPositionData = RandomGetPositionData();
            if (tempPositionData == null)
                continue;
            if (tempPositionData.GetNeedHelpListLength() < needHelpCount)
                continue;
            if (tempPositionData.GetObstacleListLength() < obstacleCount)
                continue;
            positionData = tempPositionData;
            break;
        }

        if (positionData == null)
            throw new System.Exception("Scene setting error, please rearrange the scene!");
        Debug.Log("Load data from: " + positionData.name);


        // Create GameObjects
        // someone needs help
        List<Vector3> needHelpPositionList = positionData.NeedHelpPositionList.ToList();
        for(int i = 0; i < needHelpCount; i++) {
            int randomIndex = Random.Range(0, needHelpPositionList.Count);
            Vector3 position = needHelpPositionList[randomIndex];
            needHelpPositionList.RemoveAt(randomIndex);
            Instantiate(needHelpList[i % needHelpList.Length], position, Quaternion.identity, needHelpGroupTF);
        }
        // Trees
        List<Vector3> obstaclePositionList = positionData.ObstaclePositionList.ToList();
        for (int i = 0; i < obstacleCount; i++) {
            int randomIndex = Random.Range(0, obstaclePositionList.Count);
            Vector3 position = obstaclePositionList[randomIndex];
            obstaclePositionList.RemoveAt(randomIndex);
            Instantiate(obstacleList[i % obstacleList.Length], position, Quaternion.identity, obstacleGroupTF);
        }
        // Rumia
        Instantiate(rumia, positionData.RumiaPosition, Quaternion.identity, transform);
    }

    private void ResetPositionDataFileList() {
        positionDataFileList = new List<FileInfo>();
        DirectoryInfo folder = new DirectoryInfo(Application.dataPath + POSITION_DATA_RELATIVE_PATH);
        positionDataFileList = folder.GetFiles("*" + POSITION_DATA_NAME_EXTENSION).ToList();
    }

    private RumiaRescuePositionData RandomGetPositionData() {
        int filesLength = positionDataFileList.Count;
        if (filesLength <= 0)
            return null;
        int randomFileIndex = (int)Random.Range(0, filesLength);
        string positionDataPath = $"Assets{POSITION_DATA_RELATIVE_PATH}{positionDataFileList[randomFileIndex].Name}";
        RumiaRescuePositionData positionData = AssetDatabase.LoadAssetAtPath<RumiaRescuePositionData>(positionDataPath);
        positionDataFileList.RemoveAt(randomFileIndex);
        if (positionData != null)
            return positionData;
        return null;
    }
}
