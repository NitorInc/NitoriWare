using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using TMPro;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MicrogamePlayer : MonoBehaviour
{
    public static MicrogamePlayer instance;

    private static int jobPriorityIndex = int.MaxValue;

    [SerializeField]
    private LoadMode microgameLoadMode;
    enum LoadMode
    {
        Asynchronous,
        Synchronous
    }
    [SerializeField]
    private bool UnloadResourcesOnGameUnload = true;

    [SerializeField]
    private MicrogameEventListener microgameEventListener;
    public MicrogameEventListener MicrogameEventListener => microgameEventListener;

    public delegate void MicrogameSessionSignal(Microgame.Session session);

    public enum Mode
    {
        Asynchronous,
        Synchronous
    }

    List<MicrogameJob> microgameJobs;
    class MicrogameJob
    {
        public Microgame.Session session;
        public AsyncOperation loadOperation;
        public AsyncOperation unloadOperation;

        public Microgame microgame => session.microgame;
        public Scene scene;
    }

    MicrogameJob CurrentJob =>
        microgameJobs.FirstOrDefault(a => a.session.AsyncState != Microgame.Session.SessionState.Unloading && !a.session.Cancelled);

    public Microgame CurrentMicrogame =>
        CurrentJob != null
        ? CurrentJob.microgame
        : null;

    public Microgame.Session CurrentMicrogameSession =>
        CurrentJob != null
        ? CurrentJob.session
        : null;

    public int QueuedMicrogameCount() =>
        microgameJobs
            .Where(a => a.session.AsyncState == Microgame.Session.SessionState.Loading && !a.session.Cancelled)
            .Count();

    private void Awake()
    {
        instance = this;
        microgameJobs = new List<MicrogameJob>();
    }

    public void EnqueueSession(Microgame.Session session)
    {
        var newJob = new MicrogameJob();
        if (session.EventListener == null)
            session.EventListener = microgameEventListener;
        newJob.session = session;
        session.AsyncState = Microgame.Session.SessionState.Loading;
        if (microgameLoadMode == LoadMode.Asynchronous)
        {
            newJob.loadOperation = SceneManager.LoadSceneAsync(
                newJob.session.GetSceneName(),
                LoadSceneMode.Additive);
            newJob.loadOperation.priority = jobPriorityIndex;
            jobPriorityIndex--;
            newJob.loadOperation.allowSceneActivation = false;
        }
        microgameJobs.Add(newJob);
    }

    public bool IsReadyToActivateScene()
    {
        if (CurrentMicrogameSession == null)
            return false;
        if (microgameLoadMode == LoadMode.Synchronous)
            return true;
        if (CurrentJob.loadOperation == null)
            return false;

        return CurrentJob.loadOperation.progress >= .9f;
    }

    public void ActivateScene() => ActivateScene(CurrentJob);
    void ActivateScene(MicrogameJob job)
    {
        CurrentJob.session.AsyncState = Microgame.Session.SessionState.Activating;
        if (microgameLoadMode == LoadMode.Asynchronous)
            CurrentJob.loadOperation.allowSceneActivation = true;
        else
            SceneManager.LoadScene(CurrentJob.session.GetSceneName(), LoadSceneMode.Additive);
    }

    public void HandleSceneActive(Microgame.Session session, Scene scene)
    {
        var job = microgameJobs.FirstOrDefault(a => a.session == session);
        job.scene = scene;
        if (job.session.Cancelled)
        {
            StartCoroutine(StopMicrogame(job));
            session.AsyncState = Microgame.Session.SessionState.Unloading;
        }
    }

    public void StopMicrogame() => StartCoroutine(StopMicrogame(CurrentJob));


    IEnumerator StopMicrogame(MicrogameJob job)
    {
        ShutDownMicrogameScene(job.scene);
        while (microgameLoadMode == LoadMode.Asynchronous && job.loadOperation.progress < 1f)
        {
            yield return null;
        }
        var operation = SceneManager.UnloadSceneAsync(job.scene);
        operation.completed += SceneUnloaded;
        job.unloadOperation = operation;
        SceneManager.SetActiveScene(gameObject.scene);
        job.session.AsyncState = Microgame.Session.SessionState.Unloading;
        job.session.EventListener.MicrogameEnd.Invoke(job.session);
    }

    public void CancelRemainingMicrogames()
    {
        foreach (var job in microgameJobs.Where(a => a.session.AsyncState == Microgame.Session.SessionState.Loading))
        {
            job.session.Cancelled = true;
            job.session.AsyncState = Microgame.Session.SessionState.Activating;
            if (microgameLoadMode == LoadMode.Asynchronous)
                job.loadOperation.allowSceneActivation = true;
        }
    }

    public bool AreAllCancelledScenesUnloaded()
    {
        return !microgameJobs.Any(a => a.session.Cancelled);
    }

    /// <summary>
    /// Disables all root objects in microgame
    /// </summary>
    public void ShutDownMicrogameScene(Scene scene)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (var rootObject in rootObjects)
        {
            rootObject.SetActive(false);
            Destroy(rootObject);
        }
    }

    private void SceneUnloaded(AsyncOperation asyncOperation)
    {
        if (this == null)
            return;
        var job = microgameJobs.FirstOrDefault(a => a.unloadOperation == asyncOperation);
        job.session.Dispose();
        microgameJobs.Remove(job);
        if (UnloadResourcesOnGameUnload && !resourceUnloadingBusy)
            StartCoroutine(ClearResources());
    }

    static bool resourceUnloadingBusy = false;
    IEnumerator ClearResources()
    {
        resourceUnloadingBusy = true;
        var unloadOperation = Resources.UnloadUnusedAssets();
        while (unloadOperation.progress < 1f)
        {
            yield return null;
        }
        resourceUnloadingBusy = false;

    }

    // Below is for debug mode purposes

    public void AddLoadedMicrogame(Microgame.Session session, Scene scene)
    {
        var job = new MicrogameJob();
        job.session = session;
        job.scene = scene;
        microgameJobs.Add(job);
    }

    public void LoadMicrogameImmediately(Microgame.Session session)
    {
        session.EventListener = microgameEventListener;
        microgameJobs = new List<MicrogameJob>();
        var job = new MicrogameJob();
        job.session = session;
        job.session.AsyncState = Microgame.Session.SessionState.Activating;
        microgameJobs.Add(job);
        SceneManager.LoadScene(session.GetSceneName());
    }
}