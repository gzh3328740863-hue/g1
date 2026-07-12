using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏管理器 - 负责游戏的整体状态和流程管理
/// 包括游戏速度、暂停、游戏结束等功能
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float gameSpeed = 5f;
    [SerializeField] private float speedIncrement = 0.5f;
    [SerializeField] private float speedIncrementInterval = 10f;

    private bool isGameActive = true;
    private bool isPaused = false;
    private float timeSinceLastSpeedIncrease = 0f;

    public bool IsGameActive => isGameActive;
    public bool IsPaused => isPaused;
    public float GameSpeed => gameSpeed;

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
        Time.timeScale = 1f;
        isGameActive = true;
        isPaused = false;
    }

    private void Update()
    {
        if (!isGameActive) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (!isPaused)
        {
            timeSinceLastSpeedIncrease += Time.deltaTime;
            if (timeSinceLastSpeedIncrease >= speedIncrementInterval)
            {
                IncreaseGameSpeed();
                timeSinceLastSpeedIncrease = 0f;
            }
        }
    }

    public void EndGame()
    {
        isGameActive = false;
        Time.timeScale = 0f;
        if (UIManager.Instance != null)
            UIManager.Instance.ShowGameOver();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void TogglePause()
    {
        if (!isGameActive) return;

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (UIManager.Instance != null)
            UIManager.Instance.ShowPauseMenu(isPaused);
    }

    private void IncreaseGameSpeed()
    {
        gameSpeed += speedIncrement;
        Debug.Log($"游戏速度提升到: {gameSpeed}");
    }

    public void SetGameSpeed(float speed)
    {
        gameSpeed = Mathf.Max(speed, 0f);
    }
}
