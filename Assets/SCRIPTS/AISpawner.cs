using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {

<<<<<<< HEAD
    [Header("Spawn Locations")]
    public Transform Spawn1;
    public Transform Spawn2;
    public Transform Spawn3;
=======
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

    // 0 = Laser Pistol
    // 1 = Rifle
    // 2 = Auto Shotgun
    // 3 = Boss

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
>>>>>>> 7b00fb2b0eb47fadd9ca2b279362f6fbe65679e0

            wayPoint.Add(spawnPoint[i].GetChild(0));
        }

        {
            waveList.Add(new List<Wave>());
            waveList[0].Add(new Wave(1, 0, 0.5f));
            waveList[0].Add(new Wave(1, 0, 1f));
            waveList[0].Add(new Wave(1, 0, 1.5f));

            waveList.Add(new List<Wave>());
            waveList[1].Add(new Wave(1, 0, 0.5f));
            waveList[1].Add(new Wave(1, 0, 1f));
            waveList[1].Add(new Wave(1, 0, 1.5f));
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
<<<<<<< HEAD
        Instantiate(AI, Spawn1.transform.position, Spawn1.transform.rotation);
        Instantiate(AI, Spawn2.transform.position, Spawn2.transform.rotation);
        Instantiate(AI, Spawn3.transform.position, Spawn3.transform.rotation);
    }
=======
>>>>>>> 7b00fb2b0eb47fadd9ca2b279362f6fbe65679e0

        tripped = true;
    }
}
