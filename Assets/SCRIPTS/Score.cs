using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <Summary>

    // This script Handles the score

// </Summary>

public class Score : MonoBehaviour {

    // Score text for the UI
    public Text ScoreText;

    // score counter
    public int ScoreCount = 0;

    // +10 prefab
    public GameObject plusTen;

    // +1 prefab
    public GameObject plusOne;
    
    // +5 prefab
    public GameObject plusFive;

    // +20 prefab
    public GameObject plusTenty;

    // score canvas
    public Canvas score;

    // handles the score prefabs
    private GameObject obj = null;

    // adds scores to the counter and displays how much score was earned
    public void AddScore(int i, Vector3 pos, Quaternion rot)
    {
        // +1 earned on headshots
        if (i == 1)
        {
            ScoreCount += 1;
            ScoreText.text = ScoreCount.ToString();
            obj = Instantiate(plusOne, pos + (Vector3.up * 1.5f), rot) as GameObject;
        }

        // +10 earned when rifle enemie dies
        if (i == 0)
        {
            ScoreCount += 10;
            ScoreText.text = ScoreCount.ToString();
            obj = Instantiate(plusTen, pos + (Vector3.up), rot) as GameObject;
        }

        // +20 earned when shotgun enemie dies
        if (i == 2)
        {
            ScoreCount += 20;
            ScoreText.text = ScoreCount.ToString();
            obj = Instantiate(plusTenty, pos + (Vector3.up), rot) as GameObject;
        }

        // +5 earned when laser pistol enemie dies
        if (i == 3)
        {
            ScoreCount += 5;
            ScoreText.text = ScoreCount.ToString();
            obj = Instantiate(plusFive, pos + (Vector3.up), rot) as GameObject;
        }
    }

    // moves the score prefabs upwards
    private void Update()
    {
        if (obj != null)
            obj.transform.position += Vector3.up * Time.deltaTime;
    }

    // when the player dies this detroys all dropped items, left over AI and reset the spawn for when the player retries
    public void DestroyAI()
    {
        // destory all droped items and AI
        int children = transform.GetChild(1).childCount;
        for (int i = 0; i < children; ++i)
        {
            if (transform.GetChild(1).GetChild(i).tag == "AI" || transform.GetChild(1).GetChild(i).tag == "RifleAmmo" || transform.GetChild(1).GetChild(i).tag == "ShotgunAmmo" || transform.GetChild(1).GetChild(i).tag == "HealthSmall")
            {
                Destroy(transform.GetChild(1).GetChild(i).gameObject);
            }
        }

        // resets score
        ScoreCount = 0;
        ScoreText.text = "0";

        // resers spawner
        GetComponentInChildren<AISpawner>().ResetSpawner();
    }
}
