using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/SagumeLie/Traits")]
public class SagumeLieTraits : Microgame
{
    [SerializeField]
    private SagumeQuestion[] questionPool1;
    public SagumeQuestion[] QuestionPool1 => questionPool1;

    [SerializeField]
    private SagumeQuestion[] questionPool2;
    public SagumeQuestion[] QuestionPool2 => questionPool2;

    [SerializeField]
    private SagumeQuestion[] questionPool3;
    public SagumeQuestion[] QuestionPool3 => questionPool3;

    [System.Serializable]
    public class SagumeQuestion
    {
        [SerializeField]
        private string questionText;
        public string QuestionText => questionText;

        [SerializeField]
        private string[] lieResponses;
        public string[] LieResponses => lieResponses;

        [SerializeField]
        private string[] truthResponses;
        public string[] TruthResponses => truthResponses;
    }

    //Question key format: "microgame.SagumeLie.{question index}"
    //Planned question key format: "microgame.SagumeLie.{difficulty}.question{question index}"
    public string getLocalizedQuestionText(int questionIndex)
    {
        var question = getQuestionPool()[questionIndex];
        return TextHelper.getLocalizedText($"microgame.SagumeLie.{questionIndex}", question.QuestionText);
        //return TextHelper.getLocalizedText($"microgame.SagumeLie.{difficulty}.question{questionIndex}", question.QuestionText);
    }

    //Response key format: "microgame.SagumeLie.{question index}.[lie/truth]{number index}"
    //Planned response key format: "microgame.SagumeLie.difficulty.[question]{question index}.[lie/truth]{number index}"
    public string getLocalizedResponseText(int questionIndex, bool isLie, int responseIndex)
    {
        var response = isLie ?
            getQuestionPool()[questionIndex].LieResponses[responseIndex] : getQuestionPool()[questionIndex].TruthResponses[responseIndex];
        return TextHelper.getLocalizedText($"microgame.SagumeLie.{questionIndex}.{(isLie ? "lie" : "truth")}{responseIndex}", response);
        //return TextHelper.getLocalizedText($"microgame.SagumeLie.{difficulty}.question{questionIndex}.{(isLie ? "lie" : "truth")}{responseIndex}", response);
    }

    public SagumeQuestion[] getQuestionPool()
    {
        //if (difficulty == 1) return questionPool1;
        //if (difficulty == 2) return questionPool2;
        //if (difficulty == 3) return questionPool3;
        return questionPool1;
    }
}
