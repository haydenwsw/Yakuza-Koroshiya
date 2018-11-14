using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Canvas MenuCanvas;

    public Canvas OptionsCanvas;

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

    public void Play()
    {
        Instantiate(Player, PlayerSpawn.position, PlayerSpawn.rotation);
        Camera.main.fieldOfView = fovSlider.value;
        GameObject.Find("Player").GetComponent<Movement>();//.Sensitivity = SensSlider.value;
        gameObject.SetActive(false);
    }

    public void Options()
    {
        MenuCanvas.enabled = false;
        OptionsCanvas.enabled = true;
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
