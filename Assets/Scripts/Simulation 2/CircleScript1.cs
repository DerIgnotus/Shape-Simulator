using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript1 : MonoBehaviour
{
    private GameManager1 gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager1>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            gameManager.SpawnBall();
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }
}
