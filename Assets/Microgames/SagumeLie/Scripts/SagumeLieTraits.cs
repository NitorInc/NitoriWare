using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Microgame Assets/SagumeLie/Traits")]
public class SagumeLieTraits : MicrogameTraits
{
    [SerializeField]
    private SagumeQuestion[] questionPool;
    public SagumeQuestion[] QuestionPool => questionPool;

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
    public string getLocalizedQuestionText(int questionIndex)
    {
        var question = questionPool[questionIndex];
        return TextHelper.getLocalizedText($"microgame.SagumeLie.{questionIndex}", question.QuestionText);
    }

    //Response key format: "microgame.SagumeLie.{question index}.[lie/truth]{number index}"
    public string getLocalizedResponseText(int questionIndex, bool isLie, int responseIndex)
    {
        var response = isLie ? 
            questionPool[questionIndex].LieResponses[responseIndex] : questionPool[questionIndex].TruthResponses[responseIndex];
        return TextHelper.getLocalizedText($"microgame.SagumeLie.{questionIndex}.{(isLie ? "lie" : "truth")}{responseIndex}", response);
    }
}
