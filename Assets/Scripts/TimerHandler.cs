using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float startTime = 60f; 

    public TextMeshProUGUI timerText;

    private float currentTime;
    private bool isRunning = true;

    void Start()
    {
        currentTime = startTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            OnTimerEnd();
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
{
    int seconds = Mathf.CeilToInt(currentTime); 
    timerText.text = seconds.ToString();
}


    void OnTimerEnd()
    {
        // Plus tard je pense qu'on peut jouer une autre scène ou afficher un message je verrais bien
    }

    public void RestartTimer()
    {
        currentTime = startTime;
        isRunning = true;
    }
}
