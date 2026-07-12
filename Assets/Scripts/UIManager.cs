using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("分数显示")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [Header("游戏状态UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private TextMeshProUGUI gameOverHighScoreText;

    [Header("按钮")]
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseMenuButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        InitializeUI();
        SetupButtonListeners();
    }

    private void InitializeUI()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        UpdateScoreDisplay(0);
        UpdateHighScoreDisplay();
    }

    private void SetupButtonListeners()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(() => GameManager.Instance?.RestartGame());

        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => GameManager.Instance?.TogglePause());

        if (pauseMenuButton != null)
            pauseMenuButton.onClick.AddListener(() => GameManager.Instance?.TogglePause());
    }

    public void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void UpdateHighScoreDisplay()
    {
        if (highScoreText != null)
            highScoreText.text = $"High Score: {ScoreManager.Instance?.HighScore ?? 0}";

        if (gameOverHighScoreText != null)
            gameOverHighScoreText.text = $"High Score: {ScoreManager.Instance?.HighScore ?? 0}";
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverScoreText != null)
                gameOverScoreText.text = $"Final Score: {ScoreManager.Instance?.CurrentScore ?? 0}";

            if (gameOverHighScoreText != null)
                gameOverHighScoreText.text = $"High Score: {ScoreManager.Instance?.HighScore ?? 0}";
        }
    }

    public void ShowPauseMenu(bool show)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(show);
        }
    }
}
