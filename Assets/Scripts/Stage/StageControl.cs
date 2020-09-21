using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageControl : MonoBehaviour
{
    [SerializeField]
    private Stage stage;
    public Stage Stage => stage;
    [SerializeField]
    private int maxPreloadedQueue = 2;
    [SerializeField]
    private MicrogamePlayer microgamePlayer;
    //[SerializeField]
    //private UnityEvent<int> MicrogamePlaybackIndexChanged;

    //public int MicrogamePlaybackIndex { get; private set; }

    //public void IncreasePlaybackIndex()
    //{
    //    MicrogamePlaybackIndex++;
    //    MicrogamePlaybackIndexChanged.Invoke(MicrogamePlaybackIndex);
    //}

    public void UpdateMicrogameQueue(int currentMicrogameIndex)
    {
        //Queue all available, unqueued microgames, make sure at least one is queued
        var queueCount = microgamePlayer.QueuedMicrogameCount();
        int index = currentMicrogameIndex + queueCount;
        while (queueCount == 0 || (queueCount < maxPreloadedQueue && stage.isMicrogameDetermined(index)))
        {
            CreateAndEnqueueMicrogameSession(index);
            queueCount++;
            index++;
        }
    }

    public void CreateAndEnqueueMicrogameSession(int index)
    {
        var stageMicrogame = stage.getMicrogame(index);
        var microgame = MicrogameCollection.LoadMicrogame(stageMicrogame.microgameId);
        var difficulty = stage.getMicrogameDifficulty(stageMicrogame, index);
        var session = microgame.CreateSession(difficulty);
        microgamePlayer.EnqueueSession(session);
    }

    public void CancelRemainingMicrogames()
    {
        microgamePlayer.CancelRemainingMicrogames();
    }
}
