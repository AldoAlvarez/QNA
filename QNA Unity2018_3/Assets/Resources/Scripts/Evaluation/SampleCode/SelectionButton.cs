using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField]
        private Text IDLabel;
    [SerializeField]
        private Image buttonBackground;
    private int testIndex = -1;

    public void SetID(int testIndex, string id) 
    {
        IDLabel.text = id;
        this.testIndex = testIndex;
    }
    public void SetBackgroundColor(Color color)
    {
        buttonBackground.color = color;
    }

    public int GetTestIndex() { return testIndex; }
}
