using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] protected float destroyPositionX = -15f;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (!GameManager.Instance.IsGameActive) return;

        Move();
        CheckDestroy();
    }

    protected virtual void Move()
    {
        rb.velocity = new Vector2(-GameManager.Instance.GameSpeed, rb.velocity.y);
    }

    protected virtual void CheckDestroy()
    {
        if (transform.position.x < destroyPositionX)
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.OnObstacleAvoided();

            Destroy(gameObject);
        }
    }
}
