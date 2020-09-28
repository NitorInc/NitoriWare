using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MicrogameEventListener : MonoBehaviour
{
    public UnityEvent<Microgame.Session, Scene> SceneAwake;
    public UnityEvent<Microgame.Session> MicrogameStart;
    public UnityEvent<Microgame.Session> MicrogameEnd;
    public UnityEvent<Microgame.Session> VictoryStatusUpdated;
    public UnityEvent<Microgame.Session> VictoryStatusFinalized;
    public UnityEvent<Microgame.Session, string, MicrogameCommandSettings> DisplayCommand;
}
