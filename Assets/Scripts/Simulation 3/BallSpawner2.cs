using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner2 : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int ballsToSpawn;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float ballGrowSpeed;

    [SerializeField] private Color[] ballColors;

    private float currentDelay;
    private int ballsSpawned;
    private int currentBalls;
    private int whatToDestroy = 0;
    private GameManager2 gameManager;
    private float shapeSize = 1.0f;
    private float sizeToDouble = 2.0f;
    private int howManyNew = 2;

    private List<GameObject> balls = new List<GameObject>();

    private GameObject prefab;

    void Start()
    {
        prefab = prefabs[0];
        currentBalls = 0;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager2>();

        DefaultValues();

        currentDelay = spawnDelay;
    }

    void Update()
    {
        if (!gameManager.RunSimulation()) return;

        MakeShapesGrow();

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

    private void MakeShapesGrow()
    {
        for (int i = balls.Count - 1; i >= 0; i--)
        {
            GameObject ball = balls[i];
            ball.transform.localScale += new Vector3(ballGrowSpeed * Time.deltaTime, ballGrowSpeed * Time.deltaTime, 0);

            if (ball.transform.localScale.x >= sizeToDouble)
            {
                balls.RemoveAt(i);
                Destroy(ball);
                currentBalls--;
                gameManager.SetCurrentBalls(currentBalls);

                for (int j = 0; j < howManyNew; j++)
                {
                    SpawnBall();
                }
            }
        }
    }

    private void FixedUpdate()
    {
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
        var ball = Instantiate(prefab, RandomLocation(), Quaternion.identity, transform);

        var random = Random.Range(0, ballColors.Length);

        ball.GetComponent<SpriteRenderer>().color = ballColors[random];
        ball.name = ballColors[random].ToString();

        //ball.GetComponent<SpriteRenderer>().color = GetRandomColor();
        //ball.name = ""

        balls.Add(ball.gameObject);

        gameManager.BallSpawned();
        gameManager.SetCurrentBalls(currentBalls);
    }

    private Vector3 RandomLocation()
    {
        var randomX = Random.Range(-8.5f, 5f);
        var randomY = Random.Range(-3.25f, 3.25f);

        return new Vector3(randomX, randomY, 0);
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

    public void SetGrowSpeed()
    {
        var input = gameManager.GetSettingsTransform().GetChild(3).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input, out float result))
        {
            ballGrowSpeed = result;
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

    public void SetHowManyNew()
    {
        var input = gameManager.GetSettingsTransform().GetChild(7).GetComponent<TMPro.TMP_InputField>().text;

        if (int.TryParse(input, out int result))
        {
            howManyNew = result;
        }
    }

    public void SetSizeToSpawnNew()
    {
        var input = gameManager.GetSettingsTransform().GetChild(4).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input, out float result))
        {
            sizeToDouble = result;
        }
    }

    private void DefaultValues()
    {
        var input = gameManager.GetSettingsTransform().GetChild(4).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input, out float result))
        {
            sizeToDouble = result;
        }

        var input2 = gameManager.GetSettingsTransform().GetChild(7).GetComponent<TMPro.TMP_InputField>().text;

        if (int.TryParse(input2, out int result2))
        {
            howManyNew = result2;
        }
        prefab = prefabs[gameManager.GetSettingsTransform().GetChild(6).GetComponent<TMPro.TMP_Dropdown>().value];
        whatToDestroy = gameManager.GetSettingsTransform().GetChild(5).GetComponent<TMPro.TMP_Dropdown>().value;

        var input3 = gameManager.GetSettingsTransform().GetChild(3).GetComponent<TMPro.TMP_InputField>().text;

        if (float.TryParse(input3, out float result3))
        {
            ballGrowSpeed = result3;
        }

        var input4 = gameManager.GetSettingsTransform().GetChild(2).GetComponent<TMPro.TMP_InputField>().text;

        if (int.TryParse(input4, out int result4))
        {
            ballsToSpawn = result4;
        }
    }
}
