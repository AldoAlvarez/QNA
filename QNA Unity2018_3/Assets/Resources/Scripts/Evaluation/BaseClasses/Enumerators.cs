namespace AGAC.Evaluation
{
    public enum AnswerType { NONE = -1, CORRECT, INCORRECT }

    namespace ButtonLabel
    {
        public enum Settings : uint { Background, Text, counter }
        namespace Options
        {
            public enum Background : uint { Neutral, Right_Answer, Wrong_Answer, counter }
            public enum Text : uint { Normal, Highlightened, counter }
        }
    }
}