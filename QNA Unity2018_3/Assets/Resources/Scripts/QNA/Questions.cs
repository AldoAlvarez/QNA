namespace QNA
{
    [System.Serializable]
    public class Question
    {
        public ushort AnswerValue;

        public string questionText;
        public string correct_answer;
        public string[] other_answers;

        public void SetSampleQustion(short questionIndex = 0)
        {
            AnswerValue = 10;
            questionText = "This is sample question number " + questionIndex.ToString() + ". To get full credits, you must mark as answer the question number.";
            correct_answer = questionIndex.ToString();

            uint TotalAlternativeOptions = Test.AnswerOptionsPerQuestion - 1;
            other_answers = new string[TotalAlternativeOptions];
            for (int option = 0; option < TotalAlternativeOptions; ++option)
                other_answers[option] = (questionIndex + option + 1).ToString();
        }
    }
}