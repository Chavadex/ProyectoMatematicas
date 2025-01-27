using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    
    [SerializeField] private float totalTime = 120f;

    private float timeLeft;
    public TextMeshProUGUI timerText;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject player;


    void Start()
    {
        defeatPanel.SetActive(false);
        timeLeft = totalTime;
    }

    void Update()
    {
       
        if (timeLeft > 0)
        {
           
            timeLeft -= Time.deltaTime;

        
            UpdateTimerDisplay();

            if (timeLeft <= 0)
            {
                timeLeft = 0;
                OnTimerEnd();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        int milliseconds = Mathf.FloorToInt((timeLeft * 1000) % 1000);

        timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    void OnTimerEnd()
    {

        defeatPanel.SetActive(true);
        Destroy(player);

    }


}

