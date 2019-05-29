using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/YoumuSlash/Target Type")]
public class YoumuSlashTargetType : ScriptableObject
{
    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab => prefab;

    [SerializeField]
    private Effect launchEffect = Effect.None;
    public Effect LaunchEffect => launchEffect;

    [SerializeField]
    private Effect hitEffect = Effect.None;
    public Effect HitEffect => hitEffect;

    [SerializeField]
    private bool forceUp;
    public bool ForceUp => forceUp;

    [SerializeField]
    private RuntimeAnimatorController animator;
    public RuntimeAnimatorController Animator => animator;

    [SerializeField]
    private Sprite image;
    public Sprite Image => image;

    [SerializeField]
    private YoumuSlashSoundEffect launchSoundEffect;
    public YoumuSlashSoundEffect LaunchSoundEffect => launchSoundEffect;

    [SerializeField]
    private YoumuSlashSoundEffect hitBaseSoundEffect;
    public YoumuSlashSoundEffect HitBaseSoundEffect => hitBaseSoundEffect;

    [SerializeField]
    private YoumuSlashSoundEffect hitNormalSoundEffect;
    public YoumuSlashSoundEffect HitNormalSoundEffect => hitNormalSoundEffect;

    [SerializeField]
    private YoumuSlashSoundEffect hitBarelySoundEffect;
    public YoumuSlashSoundEffect HitBarelySoundEffect => hitBarelySoundEffect;

    public enum Effect
    {
        None,
        Scream,
        SlowBurst,
        FastBurst,
        RapidBurst,
        SingleBurst
    }
}
