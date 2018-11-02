using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {

    // AI spawner class
    struct Wave
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


    [Header("AI Prefabs")]
    public GameObject[] Enemies = new GameObject[4];

    // Wave list
    private List<List<Wave>> waveList = new List<List<Wave>>();

    private List<Transform> spawnPoint = new List<Transform>();

    private List<Transform> wayPoint = new List<Transform>();

    private float time = 0;

    private bool tripped = false;

    private int wave = 0;

    private int i = 0;

    // replace this with enemies remaining
    public float timeBetweenWave = 5;

    private void Start()
    {
        int children = transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            spawnPoint.Add(transform.GetChild(i));

            wayPoint.Add(spawnPoint[i].GetChild(0));
        }
        /// <summary>
        /// WAVE DESIGN by Ervin Nunez
        /// SCRIPTED by Hayden Swift
        // 
        /// Mob Id                  
        ///     0 = Laser Pistol
        ///     1 = Rifle
        ///     2 = Auto Shotgun
        ///     3 = Boss
        /// Spawn Location          
        ///     0 = A1
        ///     1 = A2
        ///     2 = A3
        ///     3 = A4
        /// Delay from Wave Start
        ///     Time = [x]f (Float)
        /// </summary>

        {
            waveList.Add(new List<Wave>());
            waveList[0].Add(new Wave(1, 0, 0f));
            waveList[0].Add(new Wave(1, 1, 0f));
            waveList[0].Add(new Wave(1, 0, 3f));
            waveList[0].Add(new Wave(1, 1, 3f));

            waveList.Add(new List<Wave>());
            waveList[1].Add(new Wave(1, 0, 0f));
            waveList[1].Add(new Wave(1, 1, 0f));
            waveList[1].Add(new Wave(1, 0, 0.5f));
            waveList[1].Add(new Wave(1, 1, 0.5f));
            waveList[1].Add(new Wave(1, 2, 0.5f));
            waveList[1].Add(new Wave(1, 3, 0.5f));

            waveList.Add(new List<Wave>());
            waveList[2].Add(new Wave(1, 0, 0f));
            waveList[2].Add(new Wave(1, 1, 0f));
            waveList[2].Add(new Wave(1, 2, 0f));
            waveList[2].Add(new Wave(1, 3, 0f));
            waveList[2].Add(new Wave(1, 0, 4f));
            waveList[2].Add(new Wave(1, 1, 4f));
            waveList[2].Add(new Wave(1, 2, 3f));
            waveList[2].Add(new Wave(1, 3, 3f));

            waveList.Add(new List<Wave>());
            waveList[3].Add(new Wave(1, 0, 0f));
            waveList[3].Add(new Wave(1, 1, 0f));
            waveList[3].Add(new Wave(1, 2, 0f));
            waveList[3].Add(new Wave(1, 3, 0f));
            waveList[3].Add(new Wave(1, 0, 0.5f));
            waveList[3].Add(new Wave(1, 1, 0.5f));
            waveList[3].Add(new Wave(1, 0, 3f));
            waveList[3].Add(new Wave(1, 1, 3f));
            waveList[3].Add(new Wave(1, 2, 3f));
            waveList[3].Add(new Wave(1, 3, 3f));

            waveList.Add(new List<Wave>());
            waveList[4].Add(new Wave(1, 0, 0f));
            waveList[4].Add(new Wave(1, 1, 0f));
            waveList[4].Add(new Wave(1, 2, 0f));
            waveList[4].Add(new Wave(1, 3, 0f));
            waveList[4].Add(new Wave(1, 0, 0.5f));
            waveList[4].Add(new Wave(1, 1, 0.5f));
            waveList[4].Add(new Wave(1, 2, 0.5f));
            waveList[4].Add(new Wave(1, 3, 0.5f));
            waveList[4].Add(new Wave(1, 2, 2.5f));
            waveList[4].Add(new Wave(1, 3, 2.5f));
        }
    }

    private void Update()
    {
        if (tripped)
        {
            time += Time.deltaTime;

            if (i < waveList[wave].Count && time >= waveList[wave][i].Delay)
            {
                time = 0;

                GameObject Bot = Instantiate(Enemies[waveList[wave][i].Mob],
                    spawnPoint[waveList[wave][i].Location].transform.position,
                    spawnPoint[waveList[wave][i].Location].transform.rotation) as GameObject;

                Bot.GetComponent<AI>().SetTarget(wayPoint[waveList[wave][i].Location]);

                i++;
            }

            if (i >= waveList[wave].Count)
            {
                if (time >= timeBetweenWave)
                {
                    wave++;
                    i = 0;

                    Debug.Log("Wave: " + wave);

                    if (wave >= waveList.Count)
                        tripped = false;
                }
            }
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        BoxCollider b = GetComponent<BoxCollider>();
        b.enabled = false;

        tripped = true;
    }
}
