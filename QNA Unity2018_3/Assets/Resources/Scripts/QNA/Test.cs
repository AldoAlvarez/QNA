namespace QNA
{
    [System.Serializable]
    public class Test
    {
        public Test() { }
        public Test(string id, ushort passScore) 
        {
            TestID = id;
            passingScore = passScore;
        }
        public const uint AnswerOptionsPerQuestion = 3;

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