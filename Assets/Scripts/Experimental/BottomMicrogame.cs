using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class BottomMicrogame : MonoBehaviour
{
    [SerializeField]
    int difficulty = 1;

    MicrogameJob topJob;
    MicrogameJob bottomJob;

    Microgame[] microgameData;

    public class MicrogameJob
    {
        public Microgame microgame;
        public AsyncOperation asyncOperation;
        public Microgame.Session session;
        public string sceneName;
        public Scene scene;
        public List<Camera> cameras;
    }

    [SerializeField]
    private Mode mode;


    [SerializeField]
    private LayerMask topGameMask;
    [SerializeField]
    private LayerMask bottomGameMask;
    [SerializeField]
    private int bottomLayer;

    private enum Mode
    {
        Burst,
        Stream
    }

    //MicrogameTimer timer;

    bool bottomLoaded;


    // Start is called before the first frame update
    void Start()
    {
        microgameData = MicrogameCollection.LoadAllMicrogames();

        //if (MicrogameTimer.instance == null)
        //    SceneManager.LoadScene("Microgame Debug", LoadSceneMode.Additive);


        LoadTopScene();
    }

    void LoadTopScene()
    {
        if (mode == Mode.Stream)
            Invoke("LoadBottomScene", (60f / 130f) * 4f);

        if (topJob != null)
            UnloadMicrogame(topJob);
        topJob = LoadMicrogame(true);

        if (mode == Mode.Burst)
            LoadBottomScene();

    }

    void LoadBottomScene()
    {
        Invoke("LoadTopScene", (60f / 130f) * (mode == Mode.Burst ? 9 : 4f));

        if (bottomJob != null)
            UnloadMicrogame(bottomJob);
        bottomJob = LoadMicrogame(false);

    }

    MicrogameJob LoadMicrogame(bool isTop)
    {

        var newJob = new MicrogameJob();
        var controlScheme = isTop ? Microgame.ControlScheme.Mouse : Microgame.ControlScheme.Key;
        var microgames = microgameData
            .Where(a => a.controlScheme == controlScheme
                && a.duration == Microgame.Duration.Short8Beats
                && a.milestone >= Microgame.Milestone.StageReady
                && !a.isBossMicrogame())
            .ToList();

        newJob.microgame = microgames[Random.Range(0, microgames.Count - 1)];
        newJob.session = newJob.microgame.CreateDebugSession(difficulty);
        newJob.sceneName = newJob.session.GetSceneName();


        newJob.asyncOperation = SceneManager.LoadSceneAsync(newJob.sceneName, LoadSceneMode.Additive);
        if (isTop)
            newJob.asyncOperation.completed += TopMicrogame_completed;
        else
            newJob.asyncOperation.completed += BottomMicrogame_completed;

        return newJob;
    }

    private void TopMicrogame_completed(AsyncOperation obj)
    {
        topJob.asyncOperation.completed -= TopMicrogame_completed;
        MicrogameCompleted(topJob, true);
    }

    private void BottomMicrogame_completed(AsyncOperation obj)
    {
        bottomJob.asyncOperation.completed -= BottomMicrogame_completed;
        MicrogameCompleted(bottomJob, false);
    }

    private void MicrogameCompleted(MicrogameJob job, bool isTop)
    {
        //print("Hey hey " + isTop);
        //MicrogameTimer.instance.beatsLeft = 8f;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.name.Equals(job.sceneName))
            {
                job.scene = scene;
                break;
            }
        }

        foreach (var baseObj in job.scene.GetRootGameObjects())
        {
            job.cameras = new List<Camera>();
            var cam = baseObj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                job.cameras.Add(cam);
                //print("Found camera in " + job.scene.name);
                //cam.tag = "Camera";
                cam.GetComponent<AudioListener>().enabled = false;
                cam.GetComponent<RestrictCameraAspectRatio>().enabled = false;
                cam.rect = new Rect(.25f, isTop ? .5f : 0f, .5f, .5f);
                cam.cullingMask = isTop ? topGameMask : bottomGameMask;
                break;
            }
        }
    }
    
    void UnloadMicrogame(MicrogameJob job)
    {
        if (job.scene.IsValid())
            SceneManager.UnloadScene(job.scene);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Cursor.visible = !topJob.session.GetHideCursor();
        if (bottomJob != null && bottomJob.scene.IsValid())
        {
            //print("listen to meeeeee");
            foreach (var rootObj in bottomJob.scene.GetRootGameObjects())
            {
                SetLayerRecursive(rootObj.transform);
            }
        }

        //if (MicrogameDebugObjects.instance != null && MicrogameDebugObjects.instance.musicSource.volume > 0f)
        //    MicrogameDebugObjects.instance.musicSource.volume = 0f; ;
    }

    private void SetLayerRecursive(Transform trans)
    {
        for (int i = 0; i < trans.transform.childCount; i++)
        {
            SetLayerRecursive(trans.GetChild(i));
        }
        if (trans.gameObject.layer != bottomLayer)
        {
            trans.gameObject.layer = bottomLayer;
            //print(trans.gameObject.name + " is " + trans.gameObject.layer);
        }
    }
}
