using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatrixValue : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject highlight;

    public int row;
    public int column;

    public string value;
    private Color defaultColor;

    public Action<GameObject> OnClickEvent;

    private TextMeshProUGUI t;
    public Button b;

    // Start is called before the first frame update
    void Awake()
    {
        highlight = transform.GetChild(0).gameObject;
        defaultColor = ColorPalette._instance.yellowLight;
        t = transform.GetComponentInChildren<TextMeshProUGUI>();
        b = GetComponent<Button>();
        
        UpdateInstance(value, defaultColor);
    }

    public void UpdateInstance(string newText, Color newColor)
    {
        value = newText;
        t.text = value;
        t.color = newColor;
    }
    
    public void UpdateInstance(string newText)
    {
        value = newText;
        t.text = value;
    }

    public void OnPointerClick(PointerEventData evenData)
    {
        if (value != "[ ]" && b.interactable != false)
        {
            OnClickEvent?.Invoke(gameObject);
            UpdateInstance("[ ]", ColorPalette._instance.purpleDark);
            b.interactable = false;
        }
        else return;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}