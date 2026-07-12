using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int pointsPerSecond = 10;
    [SerializeField] private int pointsPerObstacleAvoided = 50;

    private int currentScore = 0;
    private int highScore = 0;
    private float timeSinceLastPoint = 0f;

    public int CurrentScore => currentScore;
    public int HighScore => highScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadHighScore();
    }

    private void Start()
    {
        currentScore = 0;
        timeSinceLastPoint = 0f;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameActive || GameManager.Instance.IsPaused) return;

        timeSinceLastPoint += Time.deltaTime;
        if (timeSinceLastPoint >= 1f)
        {
            AddScore(pointsPerSecond);
            timeSinceLastPoint = 0f;
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateScoreDisplay(currentScore);
    }

    public void OnObstacleAvoided()
    {
        AddScore(pointsPerObstacleAvoided);
    }

    public void CheckAndUpdateHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            SaveHighScore();
        }
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateHighScoreDisplay();
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    private void LoadHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}
