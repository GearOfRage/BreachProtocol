using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SequenceValue : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string value;

    private TextMeshProUGUI t;
    public Button b;
    
    private void Start()
    {
        t = transform.GetComponentInChildren<TextMeshProUGUI>();
        b = GetComponent<Button>();
        t.text = value;
    }

    public void PickValue()
    {
        b.interactable = false;
        t.color = ColorPalette._instance.yellowLight;
    }
    
    public void DropValue()
    {
        b.interactable = true;
        t.color = ColorPalette._instance.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameMaster._instance.LightUpValues(value);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        GameMaster._instance.LightDownValues();
    }
}