using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] public TextMeshProUGUI scoreText;
    private int score;

    private void Awake()
    {
        if (Instance == null) Instance = this;
            else DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
