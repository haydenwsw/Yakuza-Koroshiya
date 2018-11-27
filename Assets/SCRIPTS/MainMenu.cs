using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Canvas MenuCanvas;
    public Canvas OptionsCanvas;
    public Canvas RecordsCanvas;    // ADDED BY ERV, 19/11/18
    public Canvas CreditsCanvas;
    public Canvas ExitCanvas;
    public Canvas ControlsCanvas;

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

    public GameObject BGMusic;

    private Animator _anim;                 // ADDED BY ERV, 21/11/18
    
    private void Start()                    // SECTION BY ERV, 21/11/18
    {
        _anim = GetComponent<Animator>();
        _anim.SetBool("TWEEN_MainMenu", true);
        _anim.SetBool("TWEEN_OptionsMenu", false);
        _anim.SetBool("TWEEN_RecordsScreen", false);
        _anim.SetBool("TWEEN_CreditsScreen", false);
        _anim.SetBool("TWEEN_ExitScreen", false);
    }

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

        GameObject.Find("SPAWNS").GetComponent<Score>().score.enabled = true;

        Instantiate(BGMusic, Vector3.zero, BGMusic.transform.rotation);

        gameObject.SetActive(false);
    }

    public void Options()
    {
        MenuCanvas.enabled = false;
        OptionsCanvas.enabled = true;
        _anim.SetBool("TWEEN_OptionsMenu", true);
        _anim.SetBool("TWEEN_MainMenu", false);
    }

    public void Records()           // ADDED BY ERV 19/11/18
    {
        MenuCanvas.enabled = false;
        RecordsCanvas.enabled = true;
        _anim.SetBool("TWEEN_RecordsScreen", true);
        _anim.SetBool("TWEEN_MainMenu", false);
    }

    public void Credits()
    {
        MenuCanvas.enabled = false;
        CreditsCanvas.enabled = true;
        _anim.SetBool("TWEEN_CreditsScreen", true);
        _anim.SetBool("TWEEN_MainMenu", false);
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
        ControlsCanvas.enabled = true;
        OptionsCanvas.enabled = false;
    }

    public void BackMain()
    {
        MenuCanvas.enabled      = true;
        OptionsCanvas.enabled   = false;
        RecordsCanvas.enabled   = false;
        CreditsCanvas.enabled   = false;
        ExitCanvas.enabled      = false;

        //Animator Section by Erv 21/11/18
        _anim = GetComponent<Animator>();
        _anim.SetBool("TWEEN_MainMenu",         true);
        _anim.SetBool("TWEEN_OptionsMenu",      false);
        _anim.SetBool("TWEEN_RecordsScreen",    false);
        _anim.SetBool("TWEEN_CreditsScreen",    false);
        _anim.SetBool("TWEEN_ExitScreen",       false);
    }

    public void BackControls()
    {
        ControlsCanvas.enabled = false;
        OptionsCanvas.enabled = true;
    }
}
