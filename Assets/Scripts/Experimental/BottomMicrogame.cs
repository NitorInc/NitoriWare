using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class BottomMicrogame : MonoBehaviour
{
    [SerializeField]
    private LayerMask topGameMask;
    [SerializeField]
    private LayerMask bottomGameMask;
    [SerializeField]
    int difficulty = 1;

    MicrogameScene topScene;
    MicrogameScene bottomScene;

    public class MicrogameScene
    {
        public MicrogameSession session;
        public string name;
        public Scene scene;
        public List<Camera> cameras;
    }

    //MicrogameTimer timer;

    bool bottomLoaded;

    // Start is called before the first frame update
    void Start()
    {

        if (MicrogameTimer.instance == null)
            SceneManager.LoadScene("Microgame Debug", LoadSceneMode.Additive);



        topScene = new MicrogameScene();
        bottomScene = new MicrogameScene();

        var topMicrogames = MicrogameCollection.instance.microgames
            .Where(a => a.traits.controlScheme == MicrogameTraits.ControlScheme.Mouse
                && a.traits.duration == MicrogameTraits.Duration.Short8Beats
                && a.traits.milestone >= MicrogameTraits.Milestone.StageReady
                && !a.traits.isBossMicrogame())
            .ToList();
        var bottomMicrogames = MicrogameCollection.instance.microgames
            .Where(a => a.traits.controlScheme == MicrogameTraits.ControlScheme.Key
                && a.traits.duration == MicrogameTraits.Duration.Short8Beats
                && a.traits.milestone >= MicrogameTraits.Milestone.StageReady
                && !a.traits.isBossMicrogame())
            .ToList();

        var topMicrogame = topMicrogames[Random.Range(0, topMicrogames.Count - 1)];
        var bottomMicrogame = bottomMicrogames[Random.Range(0, bottomMicrogames.Count - 1)];

        var topSession = topMicrogame.traits.onAccessInStage(topMicrogame.microgameId, difficulty, true);
        var bottomSession = bottomMicrogame.traits.onAccessInStage(bottomMicrogame.microgameId, difficulty, true);

        
        topScene.name = topMicrogame.traits.GetSceneName(topSession);
        bottomScene.name = bottomMicrogame.traits.GetSceneName(bottomSession);
        SceneManager.LoadSceneAsync(topScene.name, LoadSceneMode.Additive).completed += TopMicrogame_completed; ;
        SceneManager.LoadSceneAsync(bottomScene.name, LoadSceneMode.Additive).completed += BottomMicrogame_completed; ;

    }

    private void TopMicrogame_completed(AsyncOperation obj)
    {
        MicrogameTimer.instance.beatsLeft = 9f;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.name.Equals(topScene.name))
            {
                topScene.scene = scene;
                break;
            }
        }

        foreach (var baseObj in topScene.scene.GetRootGameObjects())
        {
            topScene.cameras = new List<Camera>();
            var cam = baseObj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                topScene.cameras.Add(cam);
                //cam.tag = "Camera";
                cam.GetComponent<AudioListener>().enabled = false;
                cam.GetComponent<RestrictCameraAspectRatio>().enabled = false;
                cam.rect = new Rect(.25f, .5f, .5f, .5f);
                cam.cullingMask = topGameMask;
                break;
            }
        }
    }

    private void BottomMicrogame_completed(AsyncOperation obj)
    {
        MicrogameTimer.instance.beatsLeft = 9f;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.name.Equals(bottomScene.name))
            {
                bottomScene.scene = scene;
                break;
            }
        }

        foreach (var baseObj in bottomScene.scene.GetRootGameObjects())
        {
            bottomScene.cameras = new List<Camera>();
            var cam = baseObj.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                bottomScene.cameras.Add(cam);
                cam.tag = "Camera";
                //cam.GetComponent<AudioListener>().enabled = false;
                cam.GetComponent<RestrictCameraAspectRatio>().enabled = false;
                cam.rect = new Rect(.25f, 0, .5f, .5f);
                cam.cullingMask = bottomGameMask;
                break;
            }
        }

        bottomLoaded = true;
    }
    

    // Update is called once per frame
    void LateUpdate()
    {
        Cursor.visible = true;
        if (Input.GetMouseButtonDown(2))
        {
            SceneManager.LoadScene(gameObject.scene.buildIndex);
        }
        if (bottomLoaded)
        {
            foreach (var rootObj in bottomScene.scene.GetRootGameObjects())
            {
                SetLayerRecursive(rootObj.transform);
            }
        }

    }

    private void SetLayerRecursive(Transform trans)
    {
        for (int i = 0; i < trans.transform.childCount; i++)
        {
            SetLayerRecursive(trans.GetChild(i));
        }
        if (trans.gameObject.layer != 18)
            trans.gameObject.layer = 18;
    }
}
