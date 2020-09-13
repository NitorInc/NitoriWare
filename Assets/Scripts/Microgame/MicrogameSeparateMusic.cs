using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame/Separate Music")]
public class MicrogameSeparateMusic : Microgame
{
    [SerializeField]
    private AudioClip difficulty1Clip;
    [SerializeField]
    private AudioClip difficulty2Clip;
    [SerializeField]
    private AudioClip difficulty3Clip;

    new class Session : Microgame.Session
    {
        private AudioClip musicClip;

        public override AudioClip GetMusicClip() => musicClip;

        public Session(Microgame microgame, MicrogameEventListener eventListener, int difficulty, bool debugMode, AudioClip musicClip)
            : base(microgame, eventListener, difficulty, debugMode)
        {
            this.musicClip = musicClip;
        }
    }

    public override Microgame.Session CreateSession(MicrogameEventListener eventListener, int difficulty, bool debugMode = false)
    {
        return new Session(this, eventListener, difficulty, debugMode, GetAudioClip(difficulty));
    }

    private AudioClip GetAudioClip(int difficulty)
    {
        switch (difficulty)
        {
            case (1):
                return difficulty1Clip;
            case (2):
                return difficulty2Clip;
            case (3):
                return difficulty3Clip;
        }
        return null;
    }
}

