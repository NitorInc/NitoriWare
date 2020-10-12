using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class MicrogameSessionManager 
{
    static List<Microgame.Session> activeSessions;
    public static ReadOnlyCollection<Microgame.Session> ActiveSessions =>
        (activeSessions != null  ? activeSessions : new List<Microgame.Session>()).AsReadOnly();


    public static void AddSession(Microgame.Session session)
    {
        if (activeSessions == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            activeSessions = new List<Microgame.Session>();
        }
        activeSessions.Add(session);
    }

    public static void RemoveSession(Microgame.Session session)
    {
        if (activeSessions == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            activeSessions = new List<Microgame.Session>();
        }
        if (activeSessions.Contains(session))
            activeSessions.Remove(session);
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Prevent leaks  :)

        if (mode == LoadSceneMode.Single && !MicrogameDebugPlayer.DebugModeActive)
        {
            foreach (var session in activeSessions)
            {
                session.AsyncState = Microgame.Session.SessionState.Unloading;
            }
            activeSessions.Clear();
        }
    }
}
