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

    private Controller Control;

    private void Awake()
    {
        Control = GetComponentInParent<Controller>();
        FOVText.text = fovSlider.value.ToString();
    }

    public void Resume()
    {
        GetComponentInParent<Movement>().Unpause();
    }

    public void Retry()
    {

    }

    public void Options()
    {
        PauseCanvas.enabled = false;
        OptionsCanvas.enabled = true;
    }

    public void Exit()
    {
        
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
