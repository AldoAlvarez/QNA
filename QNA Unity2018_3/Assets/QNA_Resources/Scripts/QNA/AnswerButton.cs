using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QNA
{
    public class AnswerButton : MonoBehaviour
    {

        [SerializeField] private Image background_image;
        [SerializeField] private Text answer_label;

        public AnswerType AnswerType { get; private set; }

        internal void SetAnswer(string text, AnswerType type)
        {
            answer_label.text = text;
            AnswerType = type;
        }

        internal void ChangeBackground(Color color)
        {
            background_image.color = color;
        }

        internal void ChangeTextColor(Color color)
        {
            answer_label.color = color;
        }
    }
}