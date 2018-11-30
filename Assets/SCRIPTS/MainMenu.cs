using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <Summary>

    // This script is the main menu

// </Summary>

public class MainMenu : MonoBehaviour {

    // Main menu canvas
    public Canvas MenuCanvas;

    // Options Canvas
    public Canvas OptionsCanvas;

    // Records Canvas
    public Canvas RecordsCanvas;    // ADDED BY ERV, 19/11/18

    // Credits canvas
    public Canvas CreditsCanvas;

    // Exit Canvas
    public Canvas ExitCanvas;

    // Controls canvas
    public Canvas ControlsCanvas;

    // erv did this
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Background Music slider
    public Slider MusicSlider;
    // Background Music text
    public Text MusicText;

    // sound FX slider
    public Slider SounderSlider;
    // sound FX text
    public Text Soundtext;

    // FOV slider
    public Slider fovSlider;
    // FOV text
    public Text FOVText;

    // Sensitivity Slider
    public Slider SensSlider;
    // Sensitivity text
    public Text SensText;

    // Player gameobject
    public GameObject Player;

    // player's spawn postition
    public Transform PlayerSpawn;

    // menu camera
    public Camera cam;

    // BG music gameobject
    public GameObject BGMusic;

    // Score text
    public Text ScoreData;

    // menu animations
    private Animator _anim;                 // ADDED BY ERV, 21/11/18

    // background audio scource
    private AudioSource Audio;

    private void Start()                    // SECTION BY ERV, 21/11/18
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        _anim = GetComponent<Animator>();
        _anim.SetBool("TWEEN_MainMenu", true);
        _anim.SetBool("TWEEN_OptionsMenu", false);
        _anim.SetBool("TWEEN_RecordsScreen", false);
        _anim.SetBool("TWEEN_CreditsScreen", false);
        _anim.SetBool("TWEEN_ExitScreen", false);

        // refence to the background music
        Audio = GetComponent<AudioSource>();
    }

    // play the game
    public void Play()
    {
        // Instantiate player
        GameObject player = Instantiate(Player, PlayerSpawn.position, PlayerSpawn.rotation) as GameObject;
        player.GetComponent<Controller>().enabled = true;

        // get a refence to the pause class
        Pause pause = player.GetComponentInChildren<Pause>();

        //  Set sensitivity in the player's options menu
        player.GetComponent<Movement>().Sensitivity = SensSlider.value;
        pause.SensSlider.value = SensSlider.value;
        pause.SensText.text = SensSlider.value.ToString();

        // set FOV in the player's options menu
        Camera.main.fieldOfView = fovSlider.value;
        pause.fovSlider.value = fovSlider.value;
        pause.FOVText.text = fovSlider.value.ToString();

        // enable the score canvas
        GameObject.Find("SPAWNS").GetComponent<Score>().score.enabled = true;

        // set the background volumne in the player's options menu
        Instantiate(BGMusic, Vector3.zero, BGMusic.transform.rotation).GetComponent<AudioSource>().volume = MusicSlider.value;
        pause.MusicText.text = MusicSlider.value.ToString();
        pause.MusicSlider.value = MusicSlider.value;

        // set sound FX in the player's options menu
        GameObject.Find("SoundManager").GetComponent<AudioSource>().volume = SounderSlider.value;
        pause.Soundtext.text = SounderSlider.value.ToString();
        pause.SounderSlider.value = SounderSlider.value;

        // disable this game object
        gameObject.SetActive(false);
    }

    // goto the options menu
    public void Options()
    {
        MenuCanvas.enabled = false;
        OptionsCanvas.enabled = true;
        _anim.SetBool("TWEEN_OptionsMenu", true);
        _anim.SetBool("TWEEN_MainMenu", false);
    }

    // goto the Records menu
    public void Records()           // ADDED BY ERV 19/11/18
    {
        MenuCanvas.enabled = false;
        RecordsCanvas.enabled = true;
        _anim.SetBool("TWEEN_RecordsScreen", true);
        _anim.SetBool("TWEEN_MainMenu", false);

        //ScoreData.text = FileSaver.ReadString();
    }

    // got the credits menu
    public void Credits()
    {
        MenuCanvas.enabled = false;
        CreditsCanvas.enabled = true;
        _anim.SetBool("TWEEN_CreditsScreen", true);
        _anim.SetBool("TWEEN_MainMenu", false);
    }

    // Quit out of the game
    public void Quit()
    {
        Application.Quit();
    }

    // background music slider
    public void Music()
    {
        MusicText.text = MusicSlider.value.ToString() + "%";
        Audio.volume = MusicSlider.value;
    }

    // Sound FX slider
    public void Sound()
    {
        Soundtext.text = SounderSlider.value.ToString() + "%";
    }
    
    // FOV slider
    public void FOV()
    {
        FOVText.text = fovSlider.value.ToString();

        cam.fieldOfView = fovSlider.value;

        if (fovSlider.value == 120)
            FOVText.text = "Quake Pro";

        if (fovSlider.value == 60)
            FOVText.text = "Console Player";
    }
    
    // Sensitivity slider
    public void Sensitivity()
    {
        SensText.text = SensSlider.value.ToString();
    }

    // Goto controls menu
    public void Controls()
    {
        ControlsCanvas.enabled = true;
        OptionsCanvas.enabled = false;
    }

    // go back to the main menu
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

    // Go to to the option menu
    public void BackControls()
    {
        ControlsCanvas.enabled = false;
        OptionsCanvas.enabled = true;
    }
}
