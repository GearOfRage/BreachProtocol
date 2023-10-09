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


    private int minLenght = 2;
    private int maxLenght = 8;

    [HideInInspector] public int currentLenght = 2;

    private SequenceValue previousValue;

    [HideInInspector] public bool isCompleted = false;
    [HideInInspector] public SequenceState sequenceState = SequenceState.InProgress;

    [Header("Required to fill")] [Header("Threshold")] [SerializeField]
    public int lowerThreshold = 2;

    [SerializeField] public int upperThreshold = 2;

    [Header("Details")] [SerializeField] private Sprite iconSprite;
    [SerializeField] public string seqName;
    [SerializeField] private string desc;

    [Header("Objects")] [SerializeField] private Image imageObj;
    [SerializeField] private TextMeshProUGUI nameObj;
    [SerializeField] private TextMeshProUGUI descObj;
    [SerializeField] private TextMeshProUGUI resultText;

    [SerializeField] public GameObject valueHolderObj;
    [SerializeField] public GameObject highlightBlock;

    private void Start()
    {
        imageObj.sprite = iconSprite;
        nameObj.text = seqName;
        descObj.text = desc;
    }

    public void GenerateSequenceValues()
    {
        sequenceValues.Clear();
        currentLenght = Random.Range(lowerThreshold, upperThreshold + 1);
        Generator gen = GameMaster._instance.GetComponent<Generator>();
        for (int i = 0; i < currentLenght; i++)
        {
            GameObject newSequenceValue =
                Instantiate(GameMaster._instance.sequenceValuePrefab, valueHolderObj.transform);
            sequenceValues.Add(newSequenceValue.GetComponent<SequenceValue>());

            newSequenceValue.GetComponent<SequenceValue>().value =
                gen.selectedCombinations[Random.Range(0, gen.selectedCombinations.Count)];
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

        Effect();
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
            HandlePositionChange(50f);
            int pickedCounter = 0;
            for (int i = 0; i < sequenceValues.Count; i++)
            {
                if (sequenceValues[i].b.interactable)
                {
                    if (sequenceValues[i].value == matrixValue.value)
                    {
                        sequenceValues[i].PickValue();
                        previousValue = sequenceValues[i];
                        pickedCounter++;
                    }
                    else
                    {
                        if (previousValue.value != matrixValue.value)
                        {
                            foreach (SequenceValue itemJ in sequenceValues)
                            {
                                if(!itemJ.b.interactable)
                                {
                                    HandlePositionChange(50f);
                                    itemJ.DropValue();
                                }
                            }
                        }
                        else
                        {
                            if (sequenceValues[i].value == matrixValue.value)
                            {
                                pickedCounter++;
                            }

                            HandlePositionChange(50f * pickedCounter + 1);
                        }
                    }

                    break;
                }
            }

            HandlePositionChange(-50f * pickedCounter);
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
            if (bufferLeft + counter < currentLenght)
            {
                ChangeToNegative();
            }
        }
    }

    private void HandlePositionChange(float step)
    {
        Vector3 move = valueHolderObj.transform.localPosition;
        move.x += step;
        valueHolderObj.transform.localPosition = move;
    }

    public virtual void Effect()
    {
    }
}