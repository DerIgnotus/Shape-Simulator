using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyGames;

public class ADManager : MonoBehaviour
{
    private static ADManager instance;
    public static ADManager Instance { get { return instance; } }

    private int adCounter = 0;

    void Awake()
    {
        // Check if instance already exists and if it does, destroy this one
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Make this object persistent
        }
        else if (instance != this)
        {
            Destroy(gameObject);  // Destroy this object if an instance already exists
        }
    }

    private void Update()
    {
        if (adCounter >= 5)
        {
            PlayAd();
            adCounter = 0;
        }
    }

    private void PlayAd()
    {
        CrazySDK.Ad.HasAdblock((adblockPresent) => { adCounter = 5; });

        CrazySDK.Ad.RequestAd(CrazyAdType.Midgame, () => { print("Ad started"); }, (error) =>
            {
                enabled = false;
                print("Ad error, not respawning: " + error);
                adCounter = 5;
            }, () =>
            {
                print("Ad Finished! So respawning player!");

            });
    }

    public void IncrementAdCounter()
    {
        adCounter++;
    }
}
