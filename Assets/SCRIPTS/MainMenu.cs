using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject Player;

    public Transform PlayerSpawn;

    public void Play()
    {
        Instantiate(Player, PlayerSpawn.position, PlayerSpawn.rotation);
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
