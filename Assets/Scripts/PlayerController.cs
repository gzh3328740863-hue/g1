using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float maxHorizontalVelocity = 10f;

    [Header("跳跃设置")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    [Header("地面检测")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("边界设置")]
    [SerializeField] private float leftBoundary = -8f;
    [SerializeField] private float rightBoundary = 8f;

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float horizontalInput = 0f;
    private bool jumpPressed = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        CheckGroundStatus();
        ApplyGravity();
        ClampPosition();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpPressed = true;
        }

        Move(horizontalInput);

        if (jumpPressed)
        {
            Jump();
            jumpPressed = false;
        }
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Move(float direction)
    {
        float targetVelocity = direction * moveSpeed;
        rb.velocity = new Vector2(
            Mathf.Clamp(targetVelocity, -maxHorizontalVelocity, maxHorizontalVelocity),
            rb.velocity.y
        );
    }

    private void Jump()
    {
        if (!isGrounded) return;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void ApplyGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            OnCollideWithObstacle();
        }
    }

    private void OnCollideWithObstacle()
    {
        Debug.Log("玩家碰到障碍，游戏结束！");

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.CheckAndUpdateHighScore();

        if (GameManager.Instance != null)
            GameManager.Instance.EndGame();
    }

    public void ResetPlayer()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = Vector3.zero;
    }
}
