using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BufferValue : MonoBehaviour
{
    public string value;

    private TextMeshProUGUI t;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite filledSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponentInChildren<TextMeshProUGUI>();
        value = t.text;
    }

    public void UpdateVisualText()
    {
        t.text = value;
        GetComponent<Image>().sprite = filledSprite;
    }

    public void ClearValue()
    {
        value = "";
        t.text = value;
        GetComponent<Image>().sprite = defaultSprite;
    }
}
