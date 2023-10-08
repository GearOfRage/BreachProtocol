using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MatrixValue : MonoBehaviour, IPointerClickHandler
{
    public GameObject highlight;

    public int row;
    public int column;

    public string value;

    public Action<GameObject> OnClickEvent;

    private TextMeshProUGUI t;
    public Button b;

    // Start is called before the first frame update
    void Start()
    {
        UpdateInstance();
    }

    public void UpdateInstance()
    {
        highlight = transform.GetChild(0).gameObject;
        t = transform.GetComponentInChildren<TextMeshProUGUI>();
        b = GetComponent<Button>();
        t.text = value;
    }

    public void OnPointerClick(PointerEventData evenData)
    {
        if (value != "[ ]" && b.interactable != false)
        {
            OnClickEvent?.Invoke(gameObject);
            t.text = "[ ]";
            value = "[ ]";
            t.color = ColorPalette._instance.purpleDark;
            b.interactable = false;
        }
        else return;
    }
}