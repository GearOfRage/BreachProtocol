using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecurityProtocol : MonoBehaviour
{
    [Header("Required Details")]
    [SerializeField] private Sprite protocolIcon;
    [SerializeField] private string protocolName = "DefaultSecurityProtocolName";
    [SerializeField] private string protocolDesc = "DefaultSecurityProtocolDesc";
    
    [Header("Objects")]
    [SerializeField] private Image protocolIconObj;
    [SerializeField] private TextMeshProUGUI protocolNameObj;
    [SerializeField] private TextMeshProUGUI protocolDescObj;

    private int blockIndex;
    
    void Start()
    {
        protocolIconObj.sprite = protocolIcon;
        protocolNameObj.text = protocolName;
        protocolDescObj.text = protocolDesc;

        blockIndex = Random.Range(0, GameMaster._instance.activeSequences.Count - 1);
    }

    public virtual void Effect()
    {
        GameMaster._instance.activeSequences[blockIndex].ChangeToNegative();
    }
}
