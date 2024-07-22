using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private Transform settings;
    [SerializeField] private Transform simulation;
    [SerializeField] private Transform simulationStats;
    private Transform ballsHitText;
    private Transform ballsSpawnedText;
    private Transform fpsText;
    private Transform simulationTimeText;

    private int ballsHit;
    private int ballsSpawned;
    public float updateInterval = 1.0f; // Interval in seconds to update the FPS display
    private float accum = 0f; // Accumulated FPS over the interval
    private int frames = 0; // Number of frames over the interval
    private float timeLeft; // Time left for the current interval
    private int simulationTime;
    private float currentTime;

    private bool inSimulation = false;


    void Start()
    {
        simulation.gameObject.SetActive(true);

        ballsHitText = simulation.GetChild(0);
        fpsText = simulation.GetChild(1);
        simulationTimeText = simulation.GetChild(2);
        ballsSpawnedText = simulation.GetChild(3);

        settings.gameObject.SetActive(true);
        simulation.gameObject.SetActive(false);
        timeLeft = updateInterval;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        simulationTimeText.GetComponent<TMPro.TextMeshProUGUI>().text = "Simulation Time: " + currentTime.ToString("F2");

        if (inSimulation)
        {
            if (currentTime >= simulationTime)
            {
                StopSimulation();
            }
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

    public void BallHit()
    {
        ballsHit++;
        ballsHitText.GetComponent<TMPro.TextMeshProUGUI>().text = "Outer Circle hit by Shape: " + ballsHit.ToString() + " times";
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
        ballsSpawned = 0;
        inSimulation = true;
        settings.gameObject.SetActive(false);
        simulation.gameObject.SetActive(true);
        simulationStats.gameObject.SetActive(false);

        currentTime = 0;
    }

    public void StopSimulation()
    {
        inSimulation = false;
        settings.gameObject.SetActive(true);
        simulation.gameObject.SetActive(false);

        ballSpawner.ResetBalls();

        simulationStats.gameObject.SetActive(true);

        simulationStats.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = "Outer Circle was hit: " + ballsHit + " times";
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
}
