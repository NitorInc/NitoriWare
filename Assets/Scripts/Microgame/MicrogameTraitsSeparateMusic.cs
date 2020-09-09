using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Traits/Traits (Separate Music)")]
public class MicrogameTraitsSeparateMusic : Microgame
{
    [SerializeField]
    private AudioClip difficulty1Clip;
    [SerializeField]
    private AudioClip difficulty2Clip;
    [SerializeField]
    private AudioClip difficulty3Clip;

    public override AudioClip GetMusicClip(MicrogameSession session)
        => new AudioClip[] { null, difficulty1Clip, difficulty2Clip, difficulty3Clip }[session.Difficulty];

    public override AudioClip[] GetAllMusicClips()
        => new AudioClip[] { difficulty1Clip, difficulty2Clip, difficulty3Clip };
}

