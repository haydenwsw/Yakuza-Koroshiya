using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text ScoreText;

    public int ScoreCount = 0;

    public GameObject plusTen;

    public GameObject plusOne;

    public GameObject plusFive;

    public GameObject plusTenty;

    public Canvas score;

    private float time;

    private GameObject obj = null;

    public void AddScore(int i, Vector3 pos, Quaternion rot)
    {
        if (i == 1)
        {
            ScoreCount += 1;
            ScoreText.text = ScoreCount.ToString();
            obj = Instantiate(plusOne, pos + (Vector3.up * 1.5f), rot) as GameObject;
        }
        if (i == 0)
        {
            ScoreCount += 10;
            ScoreText.text = ScoreCount.ToString();
            obj = Instantiate(plusTen, pos + (Vector3.up), rot) as GameObject;
        }
        if (i == 2)
        {
            ScoreCount += 20;
            ScoreText.text = ScoreCount.ToString();
            obj = Instantiate(plusTenty, pos + (Vector3.up), rot) as GameObject;
        }
        if (i == 3)
        {
            ScoreCount += 5;
            ScoreText.text = ScoreCount.ToString();
            obj = Instantiate(plusFive, pos + (Vector3.up), rot) as GameObject;
        }
    }

    private void Update()
    {
        if (obj != null)
            obj.transform.position += Vector3.up * Time.deltaTime;
    }

    public void DestroyAI()
    {
        int children = transform.GetChild(1).childCount;
        for (int i = 0; i < children; ++i)
        {
            if (transform.GetChild(1).GetChild(i).tag == "AI" || transform.GetChild(1).GetChild(i).tag == "RifleAmmo" || transform.GetChild(1).GetChild(i).tag == "ShotgunAmmo")
            {
                Destroy(transform.GetChild(1).GetChild(i).gameObject);
            }
        }

        ScoreCount = 0;
        ScoreText.text = "0";

        GetComponentInChildren<AISpawner>().ResetSpawner();
    }
}
