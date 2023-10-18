using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HighScorePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalText;
    [SerializeField] private TextMeshProUGUI mainText;

    private char[] posibleChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

    private string[] highScore;

    public void UpdateVisuals()
    {
        highScore = SaveLoadSystem.LoadFile();
        StringBuilder sb = new StringBuilder();

        int counter = 0;
        foreach (string item in highScore)
        {
            string detailText = "";
            for (int i = 0; i < 6; i++)
            {
                detailText += posibleChar[Random.Range(0, posibleChar.Length)];
            }

            sb.Append(detailText).Append(" ").Append(highScore).Append(" €$").AppendLine();
            
            counter++;
            if (counter >= 130)
            {
                break;
            }
        }
        
        mainText.text = sb.ToString();
    }
}