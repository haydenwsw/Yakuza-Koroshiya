using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Canvas MenuCanvas;

    public Canvas OptionsCanvas;

    public Canvas RecordsCanvas;    // ADDED BY ERV, 19/11/18

    public Slider MusicSlider;
    public Text MusicText;

    public Slider SounderSlider;
    public Text Soundtext;

    public Slider fovSlider;
    public Text FOVText;

    public Slider SensSlider;
    public Text SensText;

    public GameObject Player;

    public Transform PlayerSpawn;

    public Camera cam;

    public void Play()
    {
        GameObject player = Instantiate(Player, PlayerSpawn.position, PlayerSpawn.rotation) as GameObject;
        Pause pause = player.GetComponentInChildren<Pause>();

        player.GetComponent<Movement>().Sensitivity = SensSlider.value;
        pause.SensSlider.value = SensSlider.value;
        pause.SensText.text = SensSlider.value.ToString();

        Camera.main.fieldOfView = fovSlider.value;
        pause.fovSlider.value = fovSlider.value;
        pause.FOVText.text = fovSlider.value.ToString();

        gameObject.SetActive(false);
    }

    public void Options()
    {
        MenuCanvas.enabled = false;
        OptionsCanvas.enabled = true;
    }

    public void Records()           // ADDED BY ERV 19/11/18
    {
        MenuCanvas.enabled = false;
        RecordsCanvas.enabled = true;
    }

    public void Credits()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Music()
    {
        MusicText.text = MusicSlider.value.ToString() + "%";
    }

    public void Sound()
    {
        Soundtext.text = SounderSlider.value.ToString() + "%";
    }

    public void FOV()
    {
        FOVText.text = fovSlider.value.ToString();

        cam.fieldOfView = fovSlider.value;

        if (fovSlider.value == 120)
            FOVText.text = "Quake Pro";

        if (fovSlider.value == 60)
            FOVText.text = "Console Player";
    }

    public void Sensitivity()
    {
        SensText.text = SensSlider.value.ToString();
    }

    public void Controls()
    {

    }

    public void Back()
    {
        MenuCanvas.enabled = true;
        OptionsCanvas.enabled = false;
    }
}
