using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class DiscordController : MonoBehaviour
{
    // https://github.com/discordapp/discord-rpc/blob/master/examples/button-clicker/Assets/DiscordController.cs

    public List<RichPresenceScene> compatibleScenes;

    [System.Serializable]
    public class RichPresenceScene
    {
        public string name;
        public string displayKey;
        public string defaultDisplayName;
    }

    public string applicationId;
    public string optionalSteamId;
    public string details;
    public string largeImageKey;
    public int callbackCalls;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;

    DiscordRpc.RichPresence presence;

    DiscordRpc.EventHandlers handlers;

    public void ReadyCallback()
    {
        ++callbackCalls;
        Debug.Log("Discord: ready");
        onConnect.Invoke();
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();
    }

    void Start()
    {
        Debug.Log("Discord: init");
        callbackCalls = 0;

        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    public void checkSceneForPresence()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        var scene = compatibleScenes.FirstOrDefault(a => a.name.Equals(sceneName, System.StringComparison.OrdinalIgnoreCase));
        if (scene != null)
            updatePresence(string.IsNullOrEmpty(scene.displayKey) ? scene.defaultDisplayName :
                TextHelper.getLocalizedText(scene.displayKey, scene.defaultDisplayName.ToLower())); 
    }

    public void updatePresence(string sceneName)
    {
        presence.details = details;
        presence.state = sceneName;
        presence.largeImageKey = largeImageKey;
        presence.startTimestamp = getTimeSinceEpoch();
        DiscordRpc.UpdatePresence(ref presence);
    }

    private int getTimeSinceEpoch()
    {
        TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        return (int)t.TotalSeconds;
    }

    //void OnDisable()
    //{
    //    Debug.Log("Discord: shutdown");
    //    DiscordRpc.Shutdown();
    //}

    void OnDestroy()
    {

    }
}
