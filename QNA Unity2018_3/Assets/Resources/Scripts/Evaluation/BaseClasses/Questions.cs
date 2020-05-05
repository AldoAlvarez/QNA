using System;
using UnityEngine;

namespace AGAC.Evaluation.Base
{
    [Serializable]
    public class Question
    {
        public Question() {
            displayed = true;
            other_answers = new string[Test.AnswerOptionsPerQuestion - 1];
            keywords = new string[2] { "Keyword1", "keyword2" };
        }

        [SerializeField]
        private bool displayed = true;
        public string[] keywords;

        public ushort AnswerValue;

        public string questionText;
        public string correct_answer;
        public string[] other_answers;
    }
}