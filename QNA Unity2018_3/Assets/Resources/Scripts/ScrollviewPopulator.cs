using AGAC.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollviewPopulator : MonoBehaviour
{
    #region UNITY METHODS
    private void Awake()
    {
        contentsDeltaHeight = objectsSeparation + objectsHeight;
        contentParent = GetComponent<ScrollRect>().content;
    }
    #endregion

    #region VARIABLES
    private Transform contentParent;

    [Header("Scrollview Configuration")]
    [SerializeField]
    [Range(10,200)]
    private uint baseScrollviewHeight = 100;
    [SerializeField]
    [Range(1, 500)]
    private uint objectsHeight = 150;
    [SerializeField]
    [Range(1, 200)]
    private uint objectsSeparation = 50;
    [Space]
    [SerializeField]
    private Transform ContentObjectsContainer;

    private uint contentsDeltaHeight;
    #endregion

    #region PUBLIC METHODS
    public void Populate() 
    {
        Transform[] ContentObjects = GetContentObjects();
        SetContentObjectsInScrollview(ContentObjects);
        ResizeScrollviewToMatchContent(ContentObjects);
    }
    #endregion

    #region PRIVATE METHODS
    private Transform[] GetContentObjects() 
    {
        return GeneralMethods.GetComponentsInParent<Transform>(ContentObjectsContainer);
    }

    private void SetContentObjectsInScrollview(Transform[] contentObjects) 
    {
        Vector3 newPos = Vector3.zero;
        foreach (Transform contentObj in contentObjects) 
        {
            RelateObjectToScrollview(contentObj);
            SetContentObjectPosition(contentObj, newPos);
            newPos.y -= contentsDeltaHeight;
        }
    }
    private void SetContentObjectPosition(Transform obj, Vector3 position) 
    {
        obj.GetComponent<RectTransform>().anchoredPosition = position;
    }
    private void RelateObjectToScrollview(Transform obj) { obj.parent = contentParent; }

    private void ResizeScrollviewToMatchContent(Transform[]contentObjects) 
    {
        if (contentParent == null) 
        {
            contentsDeltaHeight = objectsSeparation + objectsHeight;
            contentParent = GetComponent<ScrollRect>().content;
        }
        RectTransform contentTransform = contentParent.GetComponent<RectTransform>();
        Vector3 contentSize = contentTransform.sizeDelta;
        contentSize.y = GetScrollviewAppropiateHeight(contentObjects);
        contentTransform.sizeDelta = contentSize;
    }
    private uint GetScrollviewAppropiateHeight(Transform[]contentObjects) 
    {
        uint height = baseScrollviewHeight;
        foreach (Transform obj in contentObjects)
            if (obj.gameObject.activeSelf)
                height += contentsDeltaHeight;
        return height;
    }
    #endregion
}
