using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanelController : MonoBehaviour
{
    [SerializeField] private GameObject endButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject mainBlock;
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI minorText;
    
    public void ChangeColorPalette(Color bright, Color dark)
    {
        Image endButtonFrame = endButton.transform.GetChild(0).GetComponent<Image>();
        endButtonFrame.color = bright;
        Image endButtonBody = endButton.GetComponent<Image>();
        endButtonBody.color = dark;
        TextMeshProUGUI endButtonText = endButton.GetComponentInChildren<TextMeshProUGUI>();
        endButtonText.color = bright;
        
        Image continueButtonFrame = continueButton.transform.GetChild(0).GetComponent<Image>();
        continueButtonFrame.color = bright;
        Image continueButtonBody = continueButton.GetComponent<Image>();
        continueButtonBody.color = dark;
        TextMeshProUGUI continueButtonText = continueButton.GetComponentInChildren<TextMeshProUGUI>();
        continueButtonText.color = bright;
        
        Image backgroundImage = mainBlock.GetComponent<Image>();
        backgroundImage.color = bright;

        mainBlock.GetComponent<Image>().color = bright;
        mainBlock.transform.GetChild(0).GetComponent<Image>().color = dark;

        minorText.color = dark;
        mainText.color = bright;
    }
}
