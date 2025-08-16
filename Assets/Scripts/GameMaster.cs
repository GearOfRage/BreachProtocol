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

    [Header("Prefabs - Required")] [SerializeField]
    public GameObject matrixValuePrefab;

    [SerializeField] public GameObject sequenceValuePrefab;

    [HideInInspector] public List<GameObject> sequencesPrefabs = new List<GameObject>();
    [SerializeField] public List<GameObject> securityProtocolsPrefabs = new List<GameObject>();
    [SerializeField] public List<GameObject> exploitsPrefabs = new List<GameObject>();

    [SerializeField] public GameObject basicSequencePrefab;
    [SerializeField] public GameObject advancedSequencePrefab;
    [SerializeField] public GameObject expertSequencePrefab;


    [Header("Objects - Required")] [SerializeField]
    public GameObject matrixHolder;

    [SerializeField] public GameObject sequencesHolder;
    [HideInInspector] private RectTransform sequencesHolderDefaultSize;
    [SerializeField] public GameObject securityProtocolsHolder;
    [SerializeField] public GameObject exploitsHolder;

    [SerializeField] public TextMeshProUGUI exploitsCounterText;
    [SerializeField] public TextMeshProUGUI securityProtocolsCounterText;

    [SerializeField] public GameObject helpPanel;
    [SerializeField] public GameObject securityProtocolsPanel;
    [SerializeField] public GameObject exploitsPanel;
    [SerializeField] public GameObject mainPanel;
    [SerializeField] public GameObject resultPanel;
    [SerializeField] public GameObject highScorePanel;
    [SerializeField] public TextMeshProUGUI layerText;
    [SerializeField] public GameObject buffer;

    [SerializeField] public GameObject horizontalHighlight;
    [SerializeField] public GameObject verticalHighlight;
    [SerializeField] public GameObject sequenceHighlight;

    [SerializeField] public AudioClip winSound;
    [SerializeField] public AudioClip loseSound;

    [HideInInspector] public bool isBreachEnded = false;
    [HideInInspector] public bool atLeastOneSequenceIsCompleted = false;

    [HideInInspector] public int activeColumn = 0;
    [HideInInspector] public int activeRow = 0;
    [HideInInspector] public Line line = Line.Horizontal;

    [HideInInspector] public int bufferSize = 6; //starts from 0, 6 means 7 cells in buffer
    [HideInInspector] public int bufferUsed = 0;

    public const int matrixSize = 7;
    public GameObject[,] matrix = new GameObject[matrixSize, matrixSize];

    [Space(20)] [Header("In-game info")] public List<Sequence> activeSequences = new List<Sequence>();
    public List<SecurityProtocol> activeSecurityProtocols = new List<SecurityProtocol>();
    public List<Exploit> activeExploits = new List<Exploit>();

    private int level = 1;

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

        sequencesHolderDefaultSize = sequencesHolder.GetComponent<RectTransform>();

        HandleMatrixValuesInteractability();

        Utility.DestroyAllChildren(securityProtocolsHolder);
        Utility.DestroyAllChildren(exploitsHolder);

        //Security protocols
        //Only for immediate effects
        if (activeSecurityProtocols.Count > 0)
        {
            foreach (SecurityProtocol item in activeSecurityProtocols)
            {
                item.Effect();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (highScorePanel.activeInHierarchy)
            {
                CloseHighscorePanel();
            }
            else
            {
                CloseHelpPanel();
                OpenHighscorePanel();
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (helpPanel.activeInHierarchy)
            {
                CloseHelpPanel();
            }
            else
            {
                CloseHighscorePanel();
                OpenHelpPanel();
            }
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

        foreach (Sequence item in activeSequences)
        {
            item.CheckSequenceConditions(sender.GetComponent<MatrixValue>());
        }

        if (!isBreachEnded)
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
        if (bufferUsed >= bufferSize && !atLeastOneSequenceIsCompleted)
        {
            GameOver();
        }

        int counterInstalled = 0;
        int counterFailed = 0;
        foreach (Sequence item in activeSequences)
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

        if (counterInstalled == activeSequences.Count || counterFailed + counterInstalled == activeSequences.Count)
        {
            ShowResultPanel(true);
            isBreachEnded = true;
            DisableAllMatrixValueInteractability();
        }

        if (counterFailed >= activeSequences.Count)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (PlayerManager._instance.score > 0)
        {
            SaveLoadSystem.SaveHighscore(PlayerManager._instance.score, level);
        }

        level = 0;
        ShowResultPanel(false);
        securityProtocolsPanel.SetActive(false);
        exploitsPanel.SetActive(false);

        isBreachEnded = true;
        DisableAllMatrixValueInteractability();
    }

    public void ShowResultPanel(bool result)
    {
        resultPanel.SetActive(true);

        foreach (Sequence item in activeSequences)
        {
            if (item.sequenceState == SequenceState.InProgress)
            {
                item.ChangeToNegative();
            }
        }

        if (result)
        {
            resultPanel.GetComponent<ResultPanelController>().UpdateResultPanel(ColorPalette._instance.greenLight,
                ColorPalette._instance.greenDark, 0, "Daemons Installed");
            SoundFXManager._instance.PlaySoundFXClipOneShot(winSound, transform, 0.3f, 1f);
        }
        else
        {
            resultPanel.GetComponent<ResultPanelController>().UpdateResultPanel(ColorPalette._instance.redLight,
                ColorPalette._instance.redDark, PlayerManager._instance.score, "All Operations Blocked");

            SoundFXManager._instance.PlaySoundFXClipOneShot(loseSound, transform, 0.3f, 1f);

            Utility.DestroyAllChildren(securityProtocolsHolder);
            Utility.DestroyAllChildren(exploitsHolder);
            activeSecurityProtocols.Clear();
            activeExploits.Clear();

            PlayerManager._instance.nextTime = 30f;
            PlayerManager._instance.score = 0;
            PlayerManager._instance.UpdateVisuals();
        }

        AudioSource timerSound = gameObject.GetComponent<TimerController>().timerAudioSource;
        if (timerSound)
        {
            Destroy(timerSound);
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
        matrixHolder.transform.parent.parent.GetComponent<Animator>().Play("RightSlideFullOpen");

        activeSequences.Clear();

        horizontalHighlight.SetActive(true);
        verticalHighlight.SetActive(false);

        //Exploits Effects        
        if (activeExploits.Count > 0)
        {
            foreach (Exploit item in activeExploits)
            {
                item.RepeatEffect();
            }
        }

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

        //Activate security protocols
        if (level == 5)
        {
            securityProtocolsPanel.SetActive(true);
        }

        //Activate security protocols
        if (activeExploits.Count > 0)
        {
            exploitsHolder.SetActive(true);
        }

        if (level % 5 == 0 && activeSecurityProtocols.Count < PlayerManager._instance.limitSecurityProtocols)
        {
            generator.GenerateSecurityProtocols(securityProtocolsPrefabs);
            securityProtocolsCounterText.text =
                (activeSecurityProtocols.Count) + "/" + GetComponent<PlayerManager>().limitSecurityProtocols;
        }

        //Security protocols
        //Only for immediate effects
        if (activeSecurityProtocols.Count > 0)
        {
            foreach (SecurityProtocol item in activeSecurityProtocols)
            {
                item.Effect();
            }
        }
    }

    // Help panel interaction
    public void OpenHelpPanel()
    {
        helpPanel.SetActive(true);
        GetComponent<TimerController>().paused = true;
    }

    public void CloseHelpPanel()
    {
        helpPanel.SetActive(false);
        GetComponent<TimerController>().paused = false;
    }
    
    // Highscore panel interaction
    public void OpenHighscorePanel()
    {
        highScorePanel.SetActive(true);
        GetComponent<TimerController>().paused = true;
        highScorePanel.GetComponent<HighScorePanel>().UpdateVisuals();
    }

    public void CloseHighscorePanel()
    {
        highScorePanel.SetActive(false);
        GetComponent<TimerController>().paused = false;
    }
}