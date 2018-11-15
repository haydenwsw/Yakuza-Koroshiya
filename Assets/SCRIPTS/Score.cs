using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text ScoreText;

    private int ScoreCount = 0;

    public GameObject plusTen;

    private float time;

    public void AddScore(int i, Vector3 pos, Quaternion rot)
    {
        ScoreCount += i;
        ScoreText.text = ScoreCount.ToString();

        Instantiate(plusTen, pos + (Vector3.up * 1.5f), rot);
    }

    public void DestroyAI()
    {
        int children = transform.GetChild(1).childCount;
        for (int i = 0; i < children; ++i)
        {         
            if (transform.GetChild(1).GetChild(i).tag == "AI")
            {
                Destroy(transform.GetChild(1).GetChild(i).gameObject);
            }
        }

        GetComponentInChildren<AISpawner>().ResetSpawner();
    }
}
