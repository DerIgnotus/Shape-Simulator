using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int ballsToSpawn;
    [SerializeField] private float spawnDelay;
    [SerializeField] private int ballMaxSpeed;
    [SerializeField] private PhysicsMaterial2D ballBounciness;

    [SerializeField] private Color[] ballColors;

    private float currentDelay;
    private int ballsSpawned;
    private float shapeSize = 1.0f;
    private GameManager gameManager;

    private GameObject prefab;

    private List<Rigidbody2D> balls = new List<Rigidbody2D>();

    void Start()
    {
        prefab = prefabs[0];
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        currentDelay = spawnDelay;

        DefaultValues();
    }

    void Update()
    {
        if (!gameManager.RunSimulation()) return;

        transform.localScale = new Vector3(shapeSize, shapeSize, 1);

        if (ballsSpawned >= ballsToSpawn)
        {
            return;
        }

        if (currentDelay <= 0)
        {
            SpawnBall();
            currentDelay = spawnDelay;
        }
        else
        {
            currentDelay -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody2D ball in balls)
        {
            if (ball.velocity.magnitude > ballMaxSpeed)
            {
                ball.velocity = ball.velocity.normalized * ballMaxSpeed;
            }
        }

        for (int i = 0; i < balls.Count; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                if (balls[i].GetComponent<Collider2D>().IsTouching(balls[j].GetComponent<Collider2D>()))
                {
                    Camera.main.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public void SpawnBall()
    {
        ballsSpawned++;
        var ball = Instantiate(prefab, transform.position, Quaternion.identity, transform);
        ball.GetComponent<SpriteRenderer>().color = GetRandomColor();

        Rigidbody2D ballRigidbody = ball.GetComponent<Rigidbody2D>();
        balls.Add(ballRigidbody);

        gameManager.BallSpawned();
    }

    private Color GetRandomColor()
    {
        return ballColors[Random.Range(0, ballColors.Length)];
    }

    public void ResetBalls()
    {
        ballsSpawned = 0;
        currentDelay = spawnDelay;

        foreach (Rigidbody2D ball in balls)
        {
            Destroy(ball.gameObject);
        }

        balls.Clear();
    }

    public void SetBallsToSpawn()
    {
        var input = gameManager.GetSettingsTransform().GetChild(2).GetComponent<TMPro.TMP_InputField>().text;

        if (int.TryParse(input, out int result))
        {
            ballsToSpawn = result;
        }
    }

    public void SetBallMaxSpeed()
    {
        var input = gameManager.GetSettingsTransform().GetChild(3).GetComponent<TMPro.TMP_InputField>().text;

        if (int.TryParse(input, out int result))
        {
            ballMaxSpeed = result;
        }
    }

    public void SetBallBounciness()
    {
        var input = gameManager.GetSettingsTransform().GetChild(4).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input, out float result))
        {
            ballBounciness.bounciness = result;
        }
    }

    public void WhichShape()
    {
        prefab = prefabs[gameManager.GetSettingsTransform().GetChild(5).GetComponent<TMPro.TMP_Dropdown>().value];
    }

    public void SetShapeSize()
    {
        var input = gameManager.GetSettingsTransform().GetChild(6).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input, out float result))
        {
            if (result < 0.1f)
            {
                shapeSize = 0.1f;
                return;
            }
            else if (result > 10.0f)
            {
                shapeSize = 10.0f;
                return;
            }

            shapeSize = result;
        }
    }

    private void DefaultValues()
    {
        var input = gameManager.GetSettingsTransform().GetChild(6).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input, out float result))
        {
            if (result < 0.1f)
            {
                shapeSize = 0.1f;
                return;
            }
            else if (result > 10.0f)
            {
                shapeSize = 10.0f;
                return;
            }

            shapeSize = result;
        }

        var input2 = gameManager.GetSettingsTransform().GetChild(4).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input2, out float result2))
        {
            ballBounciness.bounciness = result2;
        }

        var input3 = gameManager.GetSettingsTransform().GetChild(3).GetComponent<TMPro.TMP_InputField>().text;

        if (int.TryParse(input3, out int result3))
        {
            ballMaxSpeed = result3;
        }

        var input4 = gameManager.GetSettingsTransform().GetChild(2).GetComponent<TMPro.TMP_InputField>().text;

        if (int.TryParse(input4, out int result4))
        {
            ballsToSpawn = result4;
        }
    }
}
