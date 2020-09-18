﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace StageFSM
{
    public class PlayPlayable : StageStateMachineBehaviour
    {
        [SerializeField]
        string stateName;

        protected override void OnStateEnterOfficial()
        {
            base.OnStateEnterOfficial();
            var asset = assetToolbox.GetAssetGroupForState(stateName).GetAsset<PlayableAsset>();
            toolbox.GetTool<PlayableDirector>().GetComponent<DirectorPlaybackController>().AssetToSwap = asset;
        }
    }
}