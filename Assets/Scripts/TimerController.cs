using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public static TimerController _instance;
    
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public GameObject timerBar;
    
    public bool isTimerStarted = false;
    
    private float countdownTime;
    private float currentTime = 0f;
    private Image img;
    
    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
        countdownTime = PlayerManager._instance.currentTime;
        currentTime = 0f;
        img = timerBar.GetComponent<Image>();
    }

    public IEnumerator StartCountdown()
    {
        countdownTime = PlayerManager._instance.currentTime;
        currentTime = 0f;
        currentTime = countdownTime;
        PlayerManager._instance.nextTime = 0f;

        isTimerStarted = true;
        img = timerBar.GetComponent<Image>();
        
        GameMaster._instance.audioManager.Play();

        while (currentTime > 0)
        {
            // Update the Text component with the current time
            timerText.text = currentTime.ToString("F2"); // Display time with two decimal place

            img.fillAmount = currentTime / countdownTime;
            // Decrease the current time by Time.deltaTime
            currentTime -= Time.deltaTime;

            if (GameMaster._instance.isBreachEnded)
            {
                PlayerManager._instance.nextTime += currentTime;
                PlayerManager._instance.nextTime = Math.Clamp(PlayerManager._instance.nextTime, 0f, PlayerManager._instance.defaultTime);
                break;
            }

            yield return null; // Wait for the next frame
        }

        // When the countdown reaches 0 or less, you can perform any desired action.
        if (!GameMaster._instance.isBreachEnded)
        {
            GameMaster._instance.ShowResultPanel(false);
        }
        
        GameMaster._instance.audioManager.Stop();
    }
}
