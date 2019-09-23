using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/YoumuSlash/Sound Effect")]
public class YoumuSlashSoundEffect : ScriptableObject
{
    [SerializeField]
    private Sound[] sounds;
    public Sound[] Sounds => sounds;

    [System.Serializable]
    public class Sound
    {
        [SerializeField]
        private AudioClip clip;
        public AudioClip Clip => clip;

        [SerializeField]
        private float volume = 1f;
        public float Volume => volume;

        [SerializeField]
        private Vector2 pitchRange;
        public Vector2 PitchRange => pitchRange;

        [SerializeField]
        private float panAmount;
        public float PanAmount => panAmount;

        [SerializeField]
        private float panSpeed;
        public float PanSpeed => panSpeed;
    }
}