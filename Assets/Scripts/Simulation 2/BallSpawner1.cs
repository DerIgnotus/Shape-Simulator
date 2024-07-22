using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner1 : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int ballsToSpawn;
    [SerializeField] private float spawnDelay;
    [SerializeField] private int ballMaxSpeed;
    [SerializeField] private PhysicsMaterial2D ballBounciness;

    [SerializeField] private Color[] ballColors;

    private float currentDelay;
    private int ballsSpawned;
    private int currentBalls;
    private int whatToDestroy = 0;
    private GameManager1 gameManager;
    private float shapeSize = 1.0f;

    private List<GameObject> balls = new List<GameObject>();

    private GameObject prefab;

    void Start()
    {
        prefab = prefabs[0];
        currentBalls = 0;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager1>();

        currentDelay = spawnDelay;
    }

    void Update()
    {
        if (!gameManager.RunSimulation()) return;

        //transform.localScale = new Vector3(shapeSize, shapeSize, 1);

        foreach (Transform child in transform)
        {
            child.localScale = new Vector3(shapeSize, shapeSize, 1);
        }

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
        foreach (GameObject ball in balls)
        {
            Rigidbody2D ballRigidbody = ball.GetComponent<Rigidbody2D>();
            if (ballRigidbody.velocity.magnitude > ballMaxSpeed)
            {
                ballRigidbody.velocity = ballRigidbody.velocity.normalized * ballMaxSpeed;
            }
        }

        CheckCollisions();
    }

    void CheckCollisions()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            for (int j = i + 1; j < balls.Count; j++)
            {
                if (balls[i].GetComponent<Collider2D>().IsTouching(balls[j].GetComponent<Collider2D>()))
                {
                    OnBallCollision(balls[i], balls[j]);
                }
            }
        }
    }

    void OnBallCollision(GameObject ball1, GameObject ball2)
    {
        if (whatToDestroy == 0)
        {
            Destroy(ball1);
            Destroy(ball2);

            balls.Remove(ball1);
            balls.Remove(ball2);

            currentBalls -= 2;
            gameManager.SetCurrentBalls(currentBalls);
        }
        else if (whatToDestroy == 1)
        {
            if (ball1.name == ball2.name)
            {
                Destroy(ball1);
                Destroy(ball2);

                balls.Remove(ball1);
                balls.Remove(ball2);

                currentBalls -= 2;
                gameManager.SetCurrentBalls(currentBalls);
            }
        }
        else if (whatToDestroy == 2)
        {
            if (ball1.name != ball2.name)
            {
                Destroy(ball1);
                Destroy(ball2);

                balls.Remove(ball1);
                balls.Remove(ball2);

                currentBalls -= 2;
                gameManager.SetCurrentBalls(currentBalls);
            }
        }
    }

    public void SpawnBall()
    {
        ballsSpawned++;
        currentBalls++;
        var ball = Instantiate(prefab, RandomLocation().position, Quaternion.identity, RandomLocation());

        var random = Random.Range(0, ballColors.Length);

        ball.GetComponent<SpriteRenderer>().color = ballColors[random];
        ball.name = ballColors[random].ToString();

        //ball.GetComponent<SpriteRenderer>().color = GetRandomColor();
        //ball.name = ""

        balls.Add(ball.gameObject);

        gameManager.BallSpawned();
        gameManager.SetCurrentBalls(currentBalls);
    }

    private Transform RandomLocation()
    {
        var random = Random.Range(0, transform.childCount);

        return transform.GetChild(random);
    }

    private Color GetRandomColor()
    {
        return ballColors[Random.Range(0, ballColors.Length)];
    }

    public void ResetBalls()
    {
        ballsSpawned = 0;
        currentDelay = spawnDelay;

        foreach (GameObject ball in balls)
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

    public void WhatToDestroy()
    {
        whatToDestroy = gameManager.GetSettingsTransform().GetChild(5).GetComponent<TMPro.TMP_Dropdown>().value;
    }

    public void WhichShape()
    {
        prefab = prefabs[gameManager.GetSettingsTransform().GetChild(6).GetComponent<TMPro.TMP_Dropdown>().value];
    }

    public void SetShapeSize()
    {
        var input = gameManager.GetSettingsTransform().GetChild(7).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input, out float result))
        {
            if (result < 0.1f)
            {
                shapeSize = 0.1f;
                return;
            }
            else if (result > 2.0f)
            {
                shapeSize = 2.0f;
                return;
            }

            shapeSize = result;

            Debug.Log(shapeSize);
        }
    }
}
