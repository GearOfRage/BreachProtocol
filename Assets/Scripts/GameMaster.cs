using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Line
{
    Horizontal,
    Vertical
}

public class GameMaster : MonoBehaviour
{
    public static GameMaster _instance;

    [SerializeField] public GameObject matrixValuePrefab;
    [SerializeField] public GameObject sequenceValuePrefab;

    [SerializeField] public GameObject basicSequencePrefab;
    [SerializeField] public GameObject advancedSequencePrefab;
    [SerializeField] public GameObject expertSequencePrefab;

    [SerializeField] public GameObject matrixHolder;
    [SerializeField] public GameObject sequencesHolder;

    [SerializeField] public GameObject resultPanel;

    [SerializeField] public GameObject buffer;

    [SerializeField] public GameObject horizontalHighlight;
    [SerializeField] public GameObject verticalHighlight;
    [SerializeField] public GameObject sequenceHighlight;

    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public GameObject timerBar;


    private bool isTimerStarted = false;
    private bool isBreachEnded = false;

    public bool atLeastOneSequenceIsCompleted = false;

    public int activeColumn = 0;
    public int activeRow = 0;

    public Line line = Line.Horizontal;

    public int bufferSize = 7; //starts from 0, 7 means 8 cells in buffer
    public int bufferUsed = 0;

    private static int matrixSize = 7;
    public GameObject[,] matrix = new GameObject[matrixSize, matrixSize];

    public List<Sequence> availableSequences = new List<Sequence>();
    public List<GameObject> sequencesPrefabs = new List<GameObject>();

    private Vector3 horizontalDefaultPosition;
    private Vector3 sequenceHighlightDefaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        sequencesPrefabs.Add(basicSequencePrefab);
        sequencesPrefabs.Add(advancedSequencePrefab);
        sequencesPrefabs.Add(expertSequencePrefab);
        GetComponent<Generator>().GenerateAll();

        horizontalDefaultPosition = horizontalHighlight.transform.localPosition;
        sequenceHighlightDefaultPosition = sequenceHighlight.transform.localPosition;

        HandleMatrixValuesInteractability();
    }

    public void HandleMatrixClick(GameObject sender)
    {
        BufferValue bufferValue = buffer.transform.GetChild(bufferUsed).GetComponent<BufferValue>();
        MatrixValue matrixValue = sender.GetComponent<MatrixValue>();

        bufferValue.value = matrixValue.value;
        bufferValue.UpdateVisualText();

        Vector3 moveHighlight = sequenceHighlight.transform.localPosition;
        moveHighlight.x += 50f;
        sequenceHighlight.transform.localPosition = moveHighlight;
        
        bufferUsed++;

        matrixValue.b.interactable = false;

        if (!isTimerStarted)
        {
            isTimerStarted = true;
            StartCoroutine(StartCountdown());
        }

        switch (line)
        {
            case Line.Horizontal:
            {
                line = Line.Vertical;
                verticalHighlight.SetActive(true);
                horizontalHighlight.SetActive(false);

                MoveVertical(sender);

                break;
            }
            case Line.Vertical:
            {
                line = Line.Horizontal;
                horizontalHighlight.SetActive(true);
                verticalHighlight.SetActive(false);

                MoveHorizontal(sender);

                break;
            }
            default:
            {
                break;
            }
        }

        activeColumn = matrixValue.column;
        activeRow = matrixValue.row;

        HandleMatrixValuesInteractability();

        foreach (Sequence item in availableSequences)
        {
            item.CheckSequenceConditions(sender.GetComponent<MatrixValue>());
        }
        
        CheckWinLoseConditions();
    }

    private IEnumerator StartCountdown()
    {
        float countdownTime = 30.0f;
        float currentTime = 0f;
        currentTime = countdownTime;

        Image img = timerBar.GetComponent<Image>();

        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        while (currentTime > 0)
        {
            // Update the Text component with the current time
            timerText.text = currentTime.ToString("F2"); // Display time with two decimal place

            img.fillAmount = currentTime / countdownTime;
            // Decrease the current time by Time.deltaTime
            currentTime -= Time.deltaTime;

            if (isBreachEnded) break;

            yield return null; // Wait for the next frame
        }

        // When the countdown reaches 0 or less, you can perform any desired action.
        timerText.text = "0.00"; // Ensure the text displays "0.00" when the countdown finishes.
        if (!isBreachEnded)
        {
            ShowResultPanel(false);
        }
    }

    void HandleMatrixValuesInteractability()
    {
        DisableAllMatrixValueInteractability();

        if (line == Line.Vertical)
        {
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (j == activeColumn)
                    {
                        MatrixValue item = matrix[i, j].GetComponent<MatrixValue>();
                        item.b.interactable = true;
                    }
                }
            }
        }

        if (line == Line.Horizontal)
        {
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    if (i == activeRow)
                    {
                        MatrixValue item = matrix[i, j].GetComponent<MatrixValue>();
                        item.b.interactable = true;
                    }
                }
            }
        }
    }

    public void LightUpValues(string value)
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                MatrixValue item = matrix[i, j].GetComponent<MatrixValue>();
                if (item.value == value)
                {
                    item.highlight.SetActive(true);
                }
            }
        }
    }

    public void LightDownValues()
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                matrix[i, j].GetComponent<MatrixValue>().highlight.SetActive(false);
            }
        }
    }

    public void MoveHorizontal(GameObject target)
    {
        Vector3 newPosition = horizontalHighlight.GetComponent<RectTransform>().localPosition;
        float targetPositionY = target.GetComponent<RectTransform>().localPosition.y;
        newPosition.y = targetPositionY + 5 * (targetPositionY / 55);
        horizontalHighlight.GetComponent<RectTransform>().localPosition = newPosition;
    }

    public void MoveVertical(GameObject target)
    {
        Vector3 newPosition = verticalHighlight.GetComponent<RectTransform>().localPosition;
        float targetPositionX = target.GetComponent<RectTransform>().localPosition.x;
        newPosition.x = targetPositionX + 5 * (targetPositionX / 55);
        verticalHighlight.GetComponent<RectTransform>().localPosition = newPosition;
    }

    public void CheckWinLoseConditions()
    {
        if (bufferUsed == bufferSize)
        {
            ShowResultPanel(atLeastOneSequenceIsCompleted);
            isBreachEnded = true;
            DisableAllMatrixValueInteractability();
        }
        
        int counterInstalled = 0;
        int counterFailed = 0;
        foreach (Sequence item in availableSequences)
        {
            if (item.sequenceState == SequenceState.Installed)
            {
                atLeastOneSequenceIsCompleted = true;
                counterInstalled++;
            }
            if (item.sequenceState == SequenceState.Failed)
            {
                counterFailed++;
            }
            
        }
        if (counterInstalled == availableSequences.Count || counterFailed + counterInstalled == availableSequences.Count)
        {
            ShowResultPanel(true);
            isBreachEnded = true;
            DisableAllMatrixValueInteractability();
        }
        if (counterFailed == availableSequences.Count)
        {
            ShowResultPanel(false);
            isBreachEnded = true;
            DisableAllMatrixValueInteractability();
        }
        
    }

    private void ShowResultPanel(bool result)
    {
        resultPanel.SetActive(true);

        foreach (Sequence item in availableSequences)
        {
            if(item.sequenceState == SequenceState.InProgress)
            {
                item.ChangeToNegative();
            }
        }

        if (result)
        {
            resultPanel.GetComponent<ResultPanelController>().ChangeColorPalette(ColorPalette._instance.greenLight,
                ColorPalette._instance.greenDark);
        }
        else
        {
            resultPanel.GetComponent<ResultPanelController>().ChangeColorPalette(ColorPalette._instance.redLight,
                ColorPalette._instance.redDark);
        }
    }

    private void DisableAllMatrixValueInteractability()
    {
        for (int i = 0; i < matrixSize; i++)
        {
            for (int j = 0; j < matrixSize; j++)
            {
                matrix[i, j].GetComponent<Button>().interactable = false;
            }
        }
    }

    public void StartNewBreach()
    {
        resultPanel.SetActive(false);

        availableSequences.Clear();

        horizontalHighlight.SetActive(true);
        verticalHighlight.SetActive(false);

        GetComponent<Generator>().GenerateAll();

        timerBar.GetComponent<Image>().fillAmount = 1f;
        timerText.text = "30.00";

        bufferUsed = 0;
        List<GameObject> bufferValues = Utility.GetChildren(buffer);
        foreach (GameObject item in bufferValues)
        {
            item.GetComponent<BufferValue>().ClearValue();
        }

        isTimerStarted = false;
        isBreachEnded = false;

        horizontalHighlight.transform.localPosition = horizontalDefaultPosition;
        sequenceHighlight.transform.localPosition = sequenceHighlightDefaultPosition;
        
        line = Line.Horizontal;
        activeRow = 0;
        activeColumn = 0;
        HandleMatrixValuesInteractability();
    }
}