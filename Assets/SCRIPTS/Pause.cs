using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <Summary>

    // This script handles the pause menu

// </Summary>

public class Pause : MonoBehaviour {

    // refence to the pause Canvas
    public Canvas PauseCanvas;

    // refence Options to the pause Canvas
    public Canvas OptionsCanvas;

    // Background Music volume slider
    public Slider MusicSlider;
    // Background Music volume text
    public Text MusicText;

    // sound FX slider
    public Slider SounderSlider;
    // sound FX text
    public Text Soundtext;

    // FOV slider
    public Slider fovSlider;
    // FOV text
    public Text FOVText;

    // sensitivity slider
    public Slider SensSlider;
    // sensitivity text
    public Text SensText;

    // refence to the controller script
    private Controller Control;

    // refence to the moement script
    private Movement Move;

    private void Awake()
    {
        // gets the refence to the controller script
        Control = GetComponentInParent<Controller>();

        // gets the refence to the moement script
        Move = GetComponentInParent<Movement>();
        
        // sets the FOV text to what you set it in the menu
        FOVText.text = fovSlider.value.ToString();
    }

    // Resumes the game
    public void Resume()
    {
        Move.Unpause();
    }

    // Retries the game
    public void Retry()
    {
        Control.Retry();
        GameObject.Find("SPAWNS").GetComponent<Score>().DestroyAI();
        Move.Unpause();
    }

    // opens the options menu
    public void Options()
    {
        PauseCanvas.enabled = false;
        OptionsCanvas.enabled = true;
    }

    // Exits to the main menu
    public void Exit()
    {
        Control.currentlyHolding.transform.parent = null;
        GameObject g = GameObject.Find("GAME DETAIL").transform.GetChild(1).gameObject;
        g.SetActive(true);
        GameObject.Find("SPAWNS").GetComponent<Score>().DestroyAI();
        //Destroy(Control.gameObject);
        Destroy(GameObject.FindGameObjectsWithTag("Finish")[0]);
        Time.timeScale = 1;
        Destroy(GameObject.FindGameObjectsWithTag("Player")[0]);
    }

    // Background music slider
    public void Music()
    {
        MusicText.text = MusicSlider.value.ToString() + "%";
        GameObject.FindGameObjectsWithTag("Finish")[0].GetComponent<AudioSource>().volume = MusicSlider.value;
    }

    // Sound FX slider
    public void Sound()
    {
        Soundtext.text = SounderSlider.value.ToString() + "%";
        GameObject.Find("SoundManager").GetComponent<AudioSource>().volume = SounderSlider.value;
    }
    
    // FOV slider
    public void FOV()
    {
        Control.cam.fieldOfView = fovSlider.value;
        FOVText.text = fovSlider.value.ToString();

        if (fovSlider.value == 120)
            FOVText.text = "Quake Pro";

        if (fovSlider.value == 60)
            FOVText.text = "Console Player";
    }

    // Sensitivity slider
    public void Sensitivity()
    {
        Move.Sensitivity = SensSlider.value;
        SensText.text = SensSlider.value.ToString();
    }

    // Controls page
    public void Controls()
    {

    }

    // gose back to the pause menu froms the pause menu
    public void Back()
    {
        PauseCanvas.enabled = true;
        OptionsCanvas.enabled = false;
    }
}
