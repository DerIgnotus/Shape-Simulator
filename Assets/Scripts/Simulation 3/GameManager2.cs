using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BallSpawner1 ballSpawner;
    [SerializeField] private Transform settings;
    [SerializeField] private Transform simulation;
    [SerializeField] private Transform simulationStats;

    private Transform ballsSpawnedText;
    private Transform fpsText;
    private Transform simulationTimeText;
    private Transform currentBallsText;

    private int ballsHit;
    private int ballsSpawned;
    public float updateInterval = 1.0f; // Interval in seconds to update the FPS display
    private float accum = 0f; // Accumulated FPS over the interval
    private int frames = 0; // Number of frames over the interval
    private float timeLeft; // Time left for the current interval
    private int simulationTime;
    private float currentTime;
    private int currentBallsPub;

    private bool inSimulation = false;


    void Start()
    {
        simulation.gameObject.SetActive(true);

        fpsText = simulation.GetChild(0);
        simulationTimeText = simulation.GetChild(1);
        ballsSpawnedText = simulation.GetChild(2);
        currentBallsText = simulation.GetChild(3);

        settings.gameObject.SetActive(true);
        simulation.gameObject.SetActive(false);
        simulationStats.gameObject.SetActive(false);
        timeLeft = updateInterval;
    }

    private void Update()
    {
        if (inSimulation)
        {
            InSimulationUpdates();
        }
    }

    private void InSimulationUpdates()
    {
        currentTime += Time.deltaTime;
        if (simulationTimeText)
        {
            simulationTimeText.GetComponent<TMPro.TextMeshProUGUI>().text = "Simulation Time: " + currentTime.ToString("F2");
        }

        if (currentTime >= simulationTime)
        {
            StopSimulation();
        }


        // Calculate the time between frames
        float deltaTime = Time.unscaledDeltaTime;

        // Accumulate the time and increment the frame count
        accum += 1.0f / deltaTime;
        ++frames;

        // Decrease the time left
        timeLeft -= deltaTime;

        // If the interval has ended, update the FPS display
        if (timeLeft <= 0.0f)
        {
            float fps = accum / frames;
            fpsText.GetComponent<TMPro.TextMeshProUGUI>().text = string.Format("{0:0.} FPS", fps);

            // Reset the variables for the next interval
            timeLeft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }


    public void BallSpawned()
    {
        ballsSpawned++;
        ballsSpawnedText.GetComponent<TMPro.TextMeshProUGUI>().text = "Shapes Spawned: " + ballsSpawned.ToString();
    }

    public bool RunSimulation()
    {
        return inSimulation;
    }

    public void StartSimulation()
    {
        currentBallsPub = 0;
        ballsSpawned = 0;
        inSimulation = true;
        settings.gameObject.SetActive(false);
        simulation.gameObject.SetActive(true);
        simulationStats.gameObject.SetActive(false);


        currentBallsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Current Shapes: " + currentBallsPub.ToString();

        currentTime = 0;
    }

    public void StopSimulation()
    {
        inSimulation = false;
        settings.gameObject.SetActive(true);
        simulation.gameObject.SetActive(false);

        ballSpawner.ResetBalls();

        simulationStats.gameObject.SetActive(true);

        simulationStats.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Balls Spawned: " + ballsSpawned;
        simulationStats.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = "Balls Remaining: " + currentBallsPub.ToString();
    }

    public void SetSimulationTime()
    {
        simulationTime = int.Parse(settings.GetChild(1).GetComponent<TMPro.TMP_InputField>().text);

        Debug.Log(simulationTime);

        currentTime = 0;
    }

    public Transform GetSettingsTransform()
    {
        return settings;
    }

    public void SetCurrentBalls(int currentBalls)
    {
        currentBallsPub = currentBalls;
        currentBallsText.GetComponent<TMPro.TextMeshProUGUI>().text = "Current Balls: " + currentBalls.ToString();
    }

    public void SpawnBall()
    {
        ballSpawner.SpawnBall();
    }
}
