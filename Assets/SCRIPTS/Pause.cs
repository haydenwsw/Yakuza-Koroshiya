using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public Canvas PauseCanvas;

    public Canvas OptionsCanvas;

    public Slider MusicSlider;
    public Text MusicText;

    public Slider SounderSlider;
    public Text Soundtext;

    public Slider fovSlider;
    public Text FOVText;

    public Slider SensSlider;
    public Text SensText;

    private Controller Control;

    private Movement Move;

    private void Awake()
    {
        Control = GetComponentInParent<Controller>();
        Move = GetComponentInParent<Movement>();
        
        FOVText.text = fovSlider.value.ToString();
    }

    public void Resume()
    {
        Move.Unpause();
    }

    public void Retry()
    {
        Control.Retry();
        GameObject.Find("SPAWNS").GetComponent<Score>().DestroyAI();
        Move.Unpause();
    }

    public void Options()
    {
        PauseCanvas.enabled = false;
        OptionsCanvas.enabled = true;
    }

    public void Exit()
    {
        GameObject g = GameObject.Find("GAME DETAIL").transform.GetChild(1).gameObject;
        g.SetActive(true);
        GameObject.Find("SPAWNS").GetComponent<Score>().DestroyAI();
        Destroy(Control.gameObject);
        Destroy(GameObject.FindGameObjectsWithTag("Finish")[0]);
        Time.timeScale = 1;
        //Destroy(GameObject.FindGameObjectsWithTag("Player")[0]);
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
        Control.cam.fieldOfView = fovSlider.value;
        FOVText.text = fovSlider.value.ToString();

        if (fovSlider.value == 120)
            FOVText.text = "Quake Pro";

        if (fovSlider.value == 60)
            FOVText.text = "Console Player";
    }

    public void Sensitivity()
    {
        Move.Sensitivity = SensSlider.value;
        SensText.text = SensSlider.value.ToString();
    }

    public void Controls()
    {

    }

    public void Back()
    {
        PauseCanvas.enabled = true;
        OptionsCanvas.enabled = false;
    }
}
