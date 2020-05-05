namespace AGAC.Evaluation.Base
{
    [System.Serializable]
    public class Test
    {
        public Test() : this("New Test", 0) { }
        public Test(string id, ushort passScore) 
        {
            TestID = id;
            passingScore = passScore;
        }
        public const uint AnswerOptionsPerQuestion = 4;

        public string TestID;
        public ushort maximumScore
        {
            get 
            {
                ushort value = 0;
                foreach (Question question in questions)
                    value += question.AnswerValue;
                return value;
            }
        }
        public ushort passingScore;

        public Question[] questions;
    }
}