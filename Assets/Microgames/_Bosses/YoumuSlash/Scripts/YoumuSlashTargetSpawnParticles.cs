using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuSlashTargetSpawnParticles : MonoBehaviour
{
    [SerializeField]
    private GameObject goodParticles;
    [SerializeField]
    private GameObject normalParticles;
    [SerializeField]
    private GameObject badParticles;

    [SerializeField]
    private float goodThreshold = .05f;
    [SerializeField]
    private float normalThreshold = .1f;

    [SerializeField]
    private Vector3 positionOffset;

    void onSlash(YoumuSlashTarget.SlashData data)
    {
        var offset = Mathf.Abs(data.timeOffset);
        GameObject effectParticle;

        if (offset < goodThreshold)
            effectParticle = goodParticles;
        else if (offset < normalThreshold)
            effectParticle = normalParticles;
        else
            effectParticle = badParticles;

        var newObject = Instantiate(effectParticle, transform.position, Quaternion.identity);
        newObject.transform.parent = transform.root;
        newObject.transform.position += positionOffset;
    }
}
