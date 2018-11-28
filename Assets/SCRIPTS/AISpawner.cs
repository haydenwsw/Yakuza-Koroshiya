using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// AI spawner class
[System.Serializable]
public struct Wave
{
    public int Mob;
    public int Location;
    public float Delay;

    public Wave(int mob, int location, float delay)
    {
        Mob = mob;
        Location = location;
        Delay = delay;
    }
}

[System.Serializable]
public class list
{
    public List<Wave> Enemies = new List<Wave>();
}

public class AISpawner : MonoBehaviour {

    public List<list> waveList = new List<list>();

    [Header("AI Prefabs")]
    public GameObject[] Enemies = new GameObject[3];

    [Header("Between Waves")]
    public int EnemiesRemaining;

    public int EnemiesRemainingTime;

    public float TimeBeforeWave;

    public Text WaveText;

    private List<Transform> spawnPoint = new List<Transform>();

    private List<Transform> wayPoint = new List<Transform>();

    private List<bool> enemiesRemain = new List<bool>();

    private float time = 0;

    private bool tripped = false;

    private int wave = 0;

    private int i = 0;

    private int waveCount = 0;

    private bool canSpawn = true;

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

                if (canSpawn && i < (wave + 1) * 2)
                {
                    int rand = Random.Range(0, spawnPoint.Count);
                    GameObject Bot = Instantiate(Enemies[Random.Range(0, 3)], spawnPoint[rand].transform.position, spawnPoint[rand].transform.rotation);
                    Bot.GetComponent<AI>().SetTarget(wayPoint[rand]);
                    Bot.transform.parent = transform;

                    i++;

                    waveCount++;
                }

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

            //if (i < waveList[wave].Enemies.Count && time >= waveList[wave].Enemies[i].Delay)
            //{
            //    time = 0;

            //    GameObject Bot = Instantiate(Enemies[waveList[wave].Enemies[i].Mob],
            //        spawnPoint[waveList[wave].Enemies[i].Location].transform.position,
            //        spawnPoint[waveList[wave].Enemies[i].Location].transform.rotation) as GameObject;

            //    Bot.GetComponent<AI>().SetTarget(wayPoint[waveList[wave].Enemies[i].Location]);
            //    Bot.transform.parent = transform;

            //    enemiesRemain.Add(true);

            //    i++;

            //    Debug.Log(i + " " + wave);
            //}

            //if (i >= waveList[wave].Enemies.Count)
            //{
            //    if (EnemiesRemainingTime >= enemiesRemain.Count && time >= TimeBeforeWave || EnemiesRemaining >= enemiesRemain.Count)
            //    {
            //        wave++;
            //        i = 0;

            //        WaveText.text = "Wave: " + wave.ToString();

            //        if (wave >= waveList.Count)
            //            wave = 0;
            //    }
            //}
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider>().enabled = false;

            tripped = true;

            WaveText.text = "Wave: 0";
        }
    }

    public void AIDead()
    {
        --waveCount;
    }

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