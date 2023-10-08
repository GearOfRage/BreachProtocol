using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum SequenceState
{
    InProgress,
    Installed,
    Failed
}

public class Sequence : MonoBehaviour
{
    public List<SequenceValue> sequenceValues;

    [SerializeField] public int lowerThreshold = 2;
    [SerializeField] public int upperThreshold = 2;

    private int minLenght = 2;
    private int maxLenght = 8;

    public int currentLenght = 2;

    private SequenceValue previousValue;

    public bool isCompleted = false;
    public SequenceState sequenceState = SequenceState.InProgress;

    [SerializeField] private Sprite iconSprite;
    [SerializeField] public string seqName;
    [SerializeField] private string desc;

    [SerializeField] private Image imageObj;
    [SerializeField] private TextMeshProUGUI nameObj;
    [SerializeField] private TextMeshProUGUI descObj;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] public GameObject valueHolderObj;
    [SerializeField] public GameObject highlightBlock;

    private void Start()
    {
        valueHolderObj = transform.GetChild(1).gameObject;
        imageObj.sprite = iconSprite;
        nameObj.text = seqName;
        descObj.text = desc;
    }

    public void GenerateSequenceValues()
    {
        sequenceValues.Clear();
        currentLenght = Random.Range(lowerThreshold, upperThreshold + 1);
        for (int i = 0; i < currentLenght; i++)
        {
            GameObject newSequenceValue =
                Instantiate(GameMaster._instance.sequenceValuePrefab, valueHolderObj.transform);
            sequenceValues.Add(newSequenceValue.GetComponent<SequenceValue>());

            newSequenceValue.GetComponent<SequenceValue>().value =
                GameMaster._instance.GetComponent<Generator>().selectedCombinations[
                    UnityEngine.Random.Range(0,
                        GameMaster._instance.GetComponent<Generator>().selectedCombinations.Count)];
            // int rndIndex = Random.Range(0, 49);
            // string v = Utility.GetChildren(GameMaster._instance.matrixHolder)[rndIndex].GetComponent<MatrixValue>().value;
            // newSequenceValue.GetComponent<SequenceValue>().value = v;
        }

        previousValue = sequenceValues[0];
    }

    public void ChangeThreshold(int shift)
    {
        lowerThreshold += shift;
        upperThreshold += shift;
    }

    public void ChangeToPositive()
    {
        Image backgroundImage = highlightBlock.GetComponent<Image>();
        backgroundImage.color = ColorPalette._instance.greenLight;

        resultText.color = ColorPalette._instance.greenDark;
        imageObj.color = ColorPalette._instance.greenDark;
        nameObj.color = ColorPalette._instance.greenDark;
        descObj.color = ColorPalette._instance.greenDark;

        resultText.text = "Installed";

        isCompleted = true;
        sequenceState = SequenceState.Installed;
        valueHolderObj.SetActive(false);
        highlightBlock.SetActive(true);
        GameMaster._instance.CheckWinLoseConditions();
    }

    public void ChangeToNegative()
    {
        Image backgroundImage = highlightBlock.GetComponent<Image>();
        backgroundImage.color = ColorPalette._instance.redLight;

        resultText.color = ColorPalette._instance.redDark;
        imageObj.color = ColorPalette._instance.redDark;
        nameObj.color = ColorPalette._instance.redDark;
        descObj.color = ColorPalette._instance.redDark;

        resultText.text = "Failed";

        isCompleted = false;
        sequenceState = SequenceState.Failed;
        valueHolderObj.SetActive(false);
        highlightBlock.SetActive(true);
        GameMaster._instance.CheckWinLoseConditions();
    }

    public void CheckSequenceConditions(MatrixValue matrixValue)
    {
        if (sequenceState == SequenceState.InProgress)
        {
            for (int i = 0; i < sequenceValues.Count; i++)
            {
                int pickedCounter = 0;
                if (sequenceValues[i].b.interactable)
                {
                    if (sequenceValues[i].value == matrixValue.value)
                    {
                        pickedCounter++;
                        sequenceValues[i].PickValue();
                        previousValue = sequenceValues[i];
                    }
                    else
                    {
                        HandlePositionChange(-50f * pickedCounter);
                        if (previousValue.value != matrixValue.value)
                        {
                            foreach (SequenceValue itemJ in sequenceValues)
                            {
                                itemJ.DropValue();
                            }
                        }
                        // else
                        // {
                        //     HandlePositionChange(-50f * pickedCounter);
                        // }
                    }

                    break;
                }
            }
        }

        int counter = 0;
        foreach (SequenceValue item in sequenceValues)
        {
            if (sequenceState == SequenceState.InProgress)
            {
                if (!item.b.interactable)
                {
                    counter++;
                    if (counter == currentLenght)
                    {
                        ChangeToPositive();
                    }
                }
            }
        }
        
        //Buffer checking
        if (sequenceState == SequenceState.InProgress)
        {
            int bufferLeft = GameMaster._instance.bufferSize - GameMaster._instance.bufferUsed;
            if (bufferLeft + counter < currentLenght )
            {
                ChangeToNegative();
            }
        }
    }

    public void HandlePositionChange(float step)
    {
        float padding = 42.5f;

        Vector3 move = valueHolderObj.transform.localPosition;
        move.x = GameMaster._instance.sequenceHighlight.GetComponent<RectTransform>().anchoredPosition.x - padding + step;
        valueHolderObj.transform.localPosition = move;
    }
}