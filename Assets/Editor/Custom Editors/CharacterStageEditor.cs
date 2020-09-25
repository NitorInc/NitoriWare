using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CharacterStage))]
[CanEditMultipleObjects]
public class CharacterStageEditor : Editor
{
    //SerializedProperty lookAtPoint;

    CharacterStage characterStage => target as CharacterStage;
    List<CharacterStage.MicrogameBatch> microgameBatches;

    void OnEnable()
    {
        PullBatches();
    }

    void PullBatches()
    {
        microgameBatches = characterStage.GetFullMicrogamePool().batches;
    }

    void WriteBatches()
    {
        characterStage.SetInternalBatches(microgameBatches);
        EditorUtility.SetDirty(characterStage);
        PullBatches();
    }

    public override void OnInspectorGUI()
    {
        var headerStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
        var boldStyle = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold };

        DrawDefaultInspector();
        GUILayout.Label("Microgame Roster", headerStyle );
        for (int i = 0; i < microgameBatches.Count; i++)
        {
            var batch = microgameBatches[i];
            GUILayout.Label("Batch " + (i + 1).ToString(), boldStyle);
            GUILayout.BeginHorizontal();

            GUILayout.Label("Pick: ", boldStyle, GUILayout.Width(30));
            var pick = EditorGUILayout.IntField(batch.pick, GUILayout.Width(30));
            if (pick != batch.pick && pick >= 0)
            {
                batch.pick = pick;
                WriteBatches();
            }

            GUILayout.EndHorizontal();
            for (int j = 0; j < batch.pool.Count; j++)
            {
                var microgame = batch.pool[j];
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent("↑"), GUILayout.Width(19)) && i > 0)
                {
                    batch.pool.RemoveAt(j);
                    j--;
                    microgameBatches[i - 1].pool.Add(microgame);
                    WriteBatches();
                }
                if (GUILayout.Button(new GUIContent("↓"), GUILayout.Width(19)) && i < microgameBatches.Count - 1)
                {
                    batch.pool.RemoveAt(j);
                    j--;
                    microgameBatches[i + 1].pool.Add(microgame);
                    WriteBatches();
                }
                GUILayout.Label(microgame.microgame.microgameId, GUILayout.Width(100));
                var difficulty = microgame.difficulty;
                GUILayout.Label(difficulty.ToString(), GUILayout.Width(15));

                if (GUILayout.Button(new GUIContent("-"), GUILayout.Width(19)) && difficulty > 1)
                {
                    microgame.difficulty--;
                    WriteBatches();
                }
                if (GUILayout.Button(new GUIContent("+"), GUILayout.Width(19)) && difficulty < 3)
                {
                    microgame.difficulty++;
                    WriteBatches();
                }
                GUILayout.EndHorizontal();
            }
        }


        if (GUILayout.Button("Add Batch"))
        {
            var newBatch = new CharacterStage.MicrogameBatch();
            newBatch.pool = new List<Stage.StageMicrogame>();
            microgameBatches.Add(newBatch);
            WriteBatches();
        }
        if (GUILayout.Button("Remove Last Batch") && microgameBatches.Count > 1)
        {
            microgameBatches[microgameBatches.Count - 2].pool.AddRange(microgameBatches[microgameBatches.Count - 1].pool);
            microgameBatches.RemoveAt(microgameBatches.Count - 1);
            WriteBatches();
        }

        if (GUILayout.Button("Copy To Clipboard"))
        {
            var text = "";
            for (int i = 0; i < microgameBatches.Count; i++)
            {
                var batch = microgameBatches[i];
                text += $"Batch {i + 1}: Pick {batch.pick}\n";
                foreach (var microgame in batch.pool)
                {
                    text += $"    {microgame.microgame.microgameId} {microgame.difficulty}\n";
                }
            }
            text = text.Trim();
            GUIUtility.systemCopyBuffer = text;   
        }

    }
}