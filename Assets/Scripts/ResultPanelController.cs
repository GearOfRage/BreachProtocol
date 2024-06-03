using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ResultPanelController : MonoBehaviour
{
    [SerializeField] private GameObject endButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject mainBlock;
    [SerializeField] private GameObject highScoreObj;
    
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI minorText;
    [SerializeField] private TextMeshProUGUI consoleText;

    [Header("Exploit")]
    [SerializeField] private GameObject exploitHolder;
    [SerializeField] private Image exploitIconComp;
    [SerializeField] private TextMeshProUGUI exploitNameComp;
    [SerializeField] private TextMeshProUGUI exploitDescComp;
    [SerializeField] private GameObject exploitDeployButton;
    [SerializeField] private GameObject exploitFragmentButton;

    private int randExploitIndex = 0;
    public StringBuilder sb = new StringBuilder();

    private void Start()
    {
        sb.Clear();
        sb.Append("//root folder").AppendLine().Append("//access request").AppendLine().Append("//access granted").AppendLine();
    }

    public void UpdateResultPanel(Color bright, Color dark, int highScore, string text)
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

        
        //consoleText.text = sb.ToString();
        /*
            //root folder
            //access request
            //access granted
            //extracting packages 1..............................completed
            //extracting packages 2..............................completed
            //extracting packages 3..............................completed
        */
            
        //highScoreObj.SetActive(false);

        // Animator anim = GetComponent<Animator>();
        // anim.Play("ShowUp");

        minorText.color = dark;
        minorText.text = text;
        mainText.color = bright;
        if (highScore > 0)
        {
            highScoreObj.GetComponent<TextMeshProUGUI>().color = dark;
            highScoreObj.GetComponent<TextMeshProUGUI>().text = "Extracted " + highScore + " €$";
            highScoreObj.SetActive(true);
        }
        else
        {
            highScoreObj.SetActive(false);
        }
        
    }

    public void AddExploit()
    {
        exploitHolder.SetActive(true);
        randExploitIndex = Random.Range(0, GameMaster._instance.exploitsPrefabs.Count);
        Exploit newExploit = GameMaster._instance.exploitsPrefabs[randExploitIndex].GetComponent<Exploit>();

        exploitIconComp.sprite = newExploit.exploitIcon;
        exploitNameComp.text = newExploit.exploitName;
        exploitDescComp.text = newExploit.exploitDesc;
        
        if (GameMaster._instance.activeExploits.Count >= 4)
        {
            exploitDeployButton.GetComponent<Button>().interactable = false;
        }

        continueButton.GetComponent<Button>().interactable = false;
    }

    public void DeployButtonClick()
    {
        GameMaster._instance.exploitsHolder.transform.parent.gameObject.SetActive(true);
        GameObject newExploit =
            Instantiate(GameMaster._instance.exploitsPrefabs[randExploitIndex], GameMaster._instance.exploitsHolder.transform);

        Exploit expComp = newExploit.GetComponent<Exploit>();
        GameMaster._instance.activeExploits.Add(expComp);
        
        exploitHolder.SetActive(false);
        continueButton.GetComponent<Button>().interactable = true;
    }
    
    public void FragmentButtonClick()
    {
        PlayerManager._instance.score += 250;
        PlayerManager._instance.UpdateVisuals();
        exploitHolder.SetActive(false);
        continueButton.GetComponent<Button>().interactable = true;
    }
}