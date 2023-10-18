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
    [SerializeField] public GameObject securityProtocolsHolder;

    [SerializeField] public GameObject mainPanel;
    [SerializeField] public GameObject resultPanel;
    [SerializeField] public GameObject securityProtocolsPanel;
    [SerializeField] public GameObject highScorePanel;
    [SerializeField] public TextMeshProUGUI layerText;
    [SerializeField] public GameObject buffer;

    [SerializeField] public GameObject horizontalHighlight;
    [SerializeField] public GameObject verticalHighlight;
    [SerializeField] public GameObject sequenceHighlight;

    public bool isBreachEnded = false;

    public bool atLeastOneSequenceIsCompleted = false;

    public int activeColumn = 0;
    public int activeRow = 0;

    public Line line = Line.Horizontal;

    public int bufferSize = 7; //starts from 0, 7 means 8 cells in buffer
    public int bufferUsed = 0;

    private int level = 1;

    public static int matrixSize = 7;
    public GameObject[,] matrix = new GameObject[matrixSize, matrixSize];

    public List<Sequence> availableSequences = new List<Sequence>();
    public List<SecurityProtocol> availableSecurityProtocols = new List<SecurityProtocol>();
    public List<GameObject> sequencesPrefabs = new List<GameObject>();
    public List<GameObject> securityProtocolsPrefabs = new List<GameObject>();

    private Vector3 horizontalDefaultPosition;
    private Vector3 sequenceHighlightDefaultPosition;

    private Generator generator;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        
        generator = GetComponent<Generator>();
        
        layerText.text = "Layer " + level;
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
        
        sequencesPrefabs.Add(basicSequencePrefab);
        sequencesPrefabs.Add(advancedSequencePrefab);
        sequencesPrefabs.Add(expertSequencePrefab);
        generator.GenerateAll();

        horizontalDefaultPosition = horizontalHighlight.transform.localPosition;
        sequenceHighlightDefaultPosition = sequenceHighlight.transform.localPosition;

        HandleMatrixValuesInteractability();
        
        Utility.DestroyAllChildren(securityProtocolsHolder);
            
        //Security protocols
        //Only for immediate effects
        if (availableSecurityProtocols.Count > 0)
        {
            foreach (SecurityProtocol item in availableSecurityProtocols)
            {
                item.Effect();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2) && isBreachEnded)
        {
            mainPanel.SetActive(false);
            highScorePanel.SetActive(true);
            highScorePanel.GetComponent<HighScorePanel>().UpdateVisuals();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && highScorePanel.active)
        {
            mainPanel.SetActive(true);
            highScorePanel.SetActive(false);
        }
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

        if (!TimerController._instance.isTimerStarted)
        {
            StartCoroutine(TimerController._instance.StartCountdown());
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
        
        if(!isBreachEnded)
        {
            CheckWinLoseConditions();
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
            level = 0;
            SaveLoadSystem.SaveFile(PlayerManager._instance.score);
            ShowResultPanel(false);
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
            level = 0;
            SaveLoadSystem.SaveFile(PlayerManager._instance.score);
            ShowResultPanel(false);
            isBreachEnded = true;
            DisableAllMatrixValueInteractability();
        }
        
    }

    public void ShowResultPanel(bool result)
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
            resultPanel.GetComponent<ResultPanelController>().UpdateResultPanel(ColorPalette._instance.greenLight,
                ColorPalette._instance.greenDark, 0, "Daemons installed");
        }
        else
        {
            resultPanel.GetComponent<ResultPanelController>().UpdateResultPanel(ColorPalette._instance.redLight,
                ColorPalette._instance.redDark, PlayerManager._instance.score,"Operation interrupted");
            
            Utility.DestroyAllChildren(securityProtocolsHolder);
            availableSecurityProtocols.Clear();
            securityProtocolsHolder.SetActive(false);
            PlayerManager._instance.nextTime = 30f;
            PlayerManager._instance.score = 0;
            PlayerManager._instance.UpdateVisuals();
            
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

        generator.GenerateAll();

        TimerController._instance.timerBar.GetComponent<Image>().fillAmount = 1f;
        TimerController._instance.timerText.text = PlayerManager._instance.nextTime.ToString("F2");
        TimerController._instance.isTimerStarted = false;

        bufferUsed = 0;
        level++;
        layerText.text = "Layer " + level;
        List<GameObject> bufferValues = Utility.GetChildren(buffer);
        foreach (GameObject item in bufferValues)
        {
            item.GetComponent<BufferValue>().ClearValue();
        }

        isBreachEnded = false;
        PlayerManager._instance.currentTime = PlayerManager._instance.nextTime;

        horizontalHighlight.transform.localPosition = horizontalDefaultPosition;
        sequenceHighlight.transform.localPosition = sequenceHighlightDefaultPosition;
        
        line = Line.Horizontal;
        activeRow = 0;
        activeColumn = 0;
        HandleMatrixValuesInteractability();

        if (level == 5)
        {
            securityProtocolsPanel.SetActive(true);
        }
        if (level % 5 == 0 && availableSecurityProtocols.Count <= 2)
        {
            generator.GenerateSecurityProtocols(securityProtocolsPrefabs);
        }
        
        //Security protocols
        //Only for immediate effects
        if (availableSecurityProtocols.Count > 0)
        {
            foreach (SecurityProtocol item in availableSecurityProtocols)
            {
                item.Effect();
            }
        }
    }
}