using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <Summary>

// This script controls The AI spawning

// </Summary>

public class AISpawner : MonoBehaviour {

    // Different AI types
    [Header("AI Prefabs")]
    public GameObject[] Enemies = new GameObject[3];

    // use to display what wave the player us currently on
    public Text WaveText;
     
    // used to collect all AI spawn points
    private List<Transform> spawnPoint = new List<Transform>();

    // used to collect all AI way points
    private List<Transform> wayPoint = new List<Transform>();
     
    // AI delta time
    private float time = 0;
    
    // to toggle if the AI can spawn of not
    private bool tripped = false;

    // use to count the waves
    private int wave = 0;
    
    // used to spawn an amount of AI per wave
    private int i = 0;

    // used to count the ammount of AI in game
    private int waveCount = 0;

    // it counts all the AI spawn points and way points
    private void Start()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            spawnPoint.Add(transform.GetChild(i));

            wayPoint.Add(spawnPoint[i].GetChild(0));
        }
    }

    private void Update()
    {
        if (tripped)
        {
            time += Time.deltaTime;

            if (time >= 2)
            {
                time = 0;

                // spawns AI based on the current wave
                if (i < (wave + 1) * 2)
                {
                    // spawn AI
                    int rand = Random.Range(0, spawnPoint.Count);
                    GameObject Bot = Instantiate(Enemies[Random.Range(0, 3)], spawnPoint[rand].transform.position, spawnPoint[rand].transform.rotation);
                    Bot.GetComponent<AI>().SetTarget(wayPoint[rand]);
                    Bot.transform.parent = transform;

                    i++;

                    waveCount++;
                }

                // handles the ticking up to the next wave
                if (i >= (wave + 1) * 2)
                {
                    if (waveCount <= 0)
                    {
                        wave++;
                        i = 0;
                        WaveText.text = "Wave: " + wave.ToString();
                    }
                }
            }
        }
    }

    // used if the player has started the game or not ans should start spawning that AI
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;

            tripped = true;

            WaveText.text = "Wave: 0";
        }
    }

    // the counter to track how many AI are left
    public void AIDead()
    {
        --waveCount;
    }

    // Resets the AI spawner so the player can retry
    public void ResetSpawner()
    {
        GetComponent<BoxCollider>().enabled = true;

        tripped = false;

        wave = 0;
        i = 0;
        waveCount = 0;

        WaveText.text = "Wave: ";
    }
}