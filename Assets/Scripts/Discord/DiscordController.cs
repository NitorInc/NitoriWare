using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
    // https://github.com/discordapp/discord-rpc/blob/master/examples/button-clicker/Assets/DiscordController.cs

    public DiscordRpc.RichPresence presence;
    public string applicationId;
    public string optionalSteamId;
    public string details;
    public string largeImageKey;
    public int callbackCalls;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;

    DiscordRpc.EventHandlers handlers;

    public void OnClick()
    {

    }

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

    void OnEnable()
    {
        Debug.Log("Discord: init");
        callbackCalls = 0;

        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    public void setPresence()
    {
        presence.details = details;
        presence.state = SceneManager.GetActiveScene().name;
        presence.largeImageKey = largeImageKey;
        DiscordRpc.UpdatePresence(ref presence);
        print("Discord: Presence updated");
    }

    void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        DiscordRpc.Shutdown();
    }

    void OnDestroy()
    {

    }
}
