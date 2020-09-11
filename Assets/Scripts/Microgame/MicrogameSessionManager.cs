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
    static List<Microgame.MicrogameSession> activeSessions;
    public static ReadOnlyCollection<Microgame.MicrogameSession> ActiveSessions =>
        (activeSessions != null  ? activeSessions : new List<Microgame.MicrogameSession>()).AsReadOnly();


    public static void AddSession(Microgame.MicrogameSession session)
    {
        if (activeSessions == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            activeSessions = new List<Microgame.MicrogameSession>();
        }
        activeSessions.Add(session);
    }

    public static void RemoveSession(Microgame.MicrogameSession session)
    {
        if (activeSessions == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            activeSessions = new List<Microgame.MicrogameSession>();
        }
        if (activeSessions.Contains(session))
            activeSessions.Remove(session);
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Prevent leaks  :)

        if (mode == LoadSceneMode.Single)
        {
            foreach (var session in activeSessions)
            {
                session.State = Microgame.MicrogameSession.SessionState.Unloading;
            }
            activeSessions.Clear();
        }
    }
}
