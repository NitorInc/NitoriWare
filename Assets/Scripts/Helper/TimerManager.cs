using UnityEngine;
using System.Collections.Generic;

namespace NitorInc.Utility {
    public class TimerManager : MonoBehaviour {
        static TimerManager _instance;
        static TimerManager Instance {
            get {
                if (_instance == null) {
                    _instance = new GameObject("TimerManager").AddComponent<TimerManager>();
                    _instance.Initialize();
                }
                else if (!_instance.gameObject.activeInHierarchy) {
                    _instance = new GameObject("TimerManager").AddComponent<TimerManager>();
                    _instance.Initialize();
                }
                return _instance;
            }
        }

        int idCounter;
        List<Timer> timers;
        void Initialize() {
            timers = new List<Timer>();
        }

        public static Timer NewTimer(float time, System.Action callback, int repeatTimes, bool startImmediate = true, bool deleteWhenDone = true) {
            var timer = new Timer(time, callback, Instance.idCounter++, repeatTimes, startImmediate, deleteWhenDone);
            Instance.timers.Add(timer);
            return timer;
        }

        void Update() {
            timers.RemoveAll((x) => x.ReadyToDelete);
            for (int i = timers.Count - 1; i >= 0; i--) {
                timers[i].Tick(Time.deltaTime);
            }
        }

        public static void Remove(int id) {
            Instance.timers.RemoveAll((x) => x.Id == id);
        }
    }

    [System.Serializable]
    public class Timer {
        public int Id { get; }
        float t;
        float time;
        int repeatTimes;
        int repeatCounter;
        bool done;
        bool readyToDelete;
        public bool ReadyToDelete => readyToDelete;
        bool deleteWhenDone;
        bool paused;
        System.Action callback;
        System.Action onFinish;
        public Timer(float time, System.Action callback, int id, int repeatTimes = 0, bool startImmediate = true, bool deleteWhenDone = true) {
            this.time = time;
            t = 0.0f;
            this.repeatTimes = repeatTimes;
            repeatCounter = repeatTimes;
            done = !startImmediate;
            Id = id;
            this.callback = callback;
            this.deleteWhenDone = deleteWhenDone;
            readyToDelete = false;
            paused = false;
        }

        public void Tick(float deltaTime) {
            if (!readyToDelete) {
                if (!done) {
                    if (!paused) {
                        t += deltaTime;
                        if (t >= time) {
                            if (repeatCounter <= 0) {
                                done = true;
                                if (onFinish != null) {
                                    onFinish();
                                }
                                if (deleteWhenDone)
                                    readyToDelete = true;
                            } else {
                                --repeatCounter;
                            }
                            t = 0.0f;
                            if (callback != null) {
                                callback();
                            }
                        }
                    }
                }
            }
        }

        public void SetCallback(System.Action callback) => this.callback = callback;
        public void SetOnFinish(System.Action onFinish) => this.onFinish = onFinish;

        public void StartOnce() {
            if (done) {
                done = false;
            }
        }

        public void Start() {
            if (done) {
                Restart();
            }
        }

        public void StartWithCallback() {
            if (done) {
                Restart();
                if (callback != null) {
                    callback();
                }
            }
        }

        public void Restart() {
            t = 0.0f;
            done = false;
            repeatCounter = repeatTimes;
        }

        public void Restart(float time) {
            this.time = time;
            t = 0.0f;
            done = false;
            repeatCounter = repeatTimes;
        }

        public void SetTime(float time) => this.time = time;
        public float GetCurrentTime() => time;
        public void Pause() => paused = true;
        public void Unpause() => paused = false;

        public void Stop(bool triggerCallback = false) {
            if (!done) {
                t = 0.0f;
                done = true;
                repeatCounter = 0;
                if (triggerCallback)
                    if (callback != null)
                        callback();
                if (deleteWhenDone)
                    SetToRemove();
            }
        }

        public bool IsRunning() => !done && !paused && !readyToDelete;

        void SetToRemove() {
            readyToDelete = true;
        }
    }
}