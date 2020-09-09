using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Traits/Traits (Single Scene)")]
public class MicrogameTraitsSingleScene : Microgame
{
    public override bool SceneDeterminesDifficulty => false;

    public override string GetSceneName(MicrogameSession session) => session.MicrogameId;
}

