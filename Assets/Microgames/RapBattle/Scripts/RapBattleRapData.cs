using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/RapBattle/Rap Data")]
public class RapBattleRapData : ScriptableObject
{
    [SerializeField]
    private Color rhymeHighlightColor;
    public Color RhymeHighlightColor => rhymeHighlightColor;

    [SerializeField]
    private Rap[] raps;
    public Rap[] Raps => raps;

    [System.Serializable]
    public class Rap
    {
        [SerializeField]
        private string name;
        public string Name => name;
        [SerializeField]
        private Line[] lines;
        public Line[] Lines => lines;
        [SerializeField]
        private string answer;
        public string Answer => answer;
        [SerializeField]
        private string[] wrongAnswers;
        public string[] WrongAnswers => wrongAnswers;
    }

    [System.Serializable]
    public class Line
    {
        [SerializeField]
        private string verse;
        public string Verse => verse;
        [SerializeField]
        private string rhyme;
        public string Rhyme => rhyme;
    }
}
