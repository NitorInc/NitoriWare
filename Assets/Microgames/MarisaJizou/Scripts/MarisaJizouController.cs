using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NitorInc.MarisaJizou {

    public class MarisaJizouController : MonoBehaviour {

        public delegate void Action();
        public static event Action onVictory;

        public int requiredJizou = 2;
        int totalJizou => spawnLocations.Count;
        int successCounter = 0;
        public int failTolerance => hatsCarried - requiredJizou;
        int failCounter = 0;
        public int hatsCarried = 3;

        public List<Transform> spawnLocations;

        public List<GameObject> specialJizouProtos;
        public GameObject normalJizouProto;
        List<GameObject> jizouList;

        public AudioClip[] touchJizouClip;
        public float finishClipDelay = 0.2f;
        public AudioClip finishClip;
        // Use this for initialization
        void Start() {
            specialJizouProtos.Shuffle();
            spawnLocations.Shuffle();

            jizouList = new List<GameObject>();
            GameObject go = null;
            for (int i = 0; i < totalJizou; i++) {
                if (i < requiredJizou) {
                    go = Instantiate(specialJizouProtos[i]);
                    go.GetComponent<MarisaJizouJizou>().Register(this);
                } else {
                    go = Instantiate(normalJizouProto);
                }
                jizouList.Add(go);
                go.transform.position = spawnLocations[i].position;
            }
        }

        void PlayFinishClip () {
            MicrogameController.instance.playSFX(finishClip, MicrogameController.instance.getSFXSource().panStereo);
        }

        public void Notify(bool succeed) {
            if (succeed) {
                MicrogameController.instance.playSFX(touchJizouClip[successCounter], MicrogameController.instance.getSFXSource().panStereo);
                successCounter++;
                if (successCounter >= requiredJizou) {
                    Invoke("PlayFinishClip", finishClipDelay);
                    MicrogameController.instance.setVictory(true, true);
                    if (onVictory != null)
                        onVictory();
                }
            } else {
                failCounter++;
                if (failCounter > failTolerance)
                    MicrogameController.instance.setVictory(false, true);
            }
        }

    }

    public static class ListExtension {

        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}