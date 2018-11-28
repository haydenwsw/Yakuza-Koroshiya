using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{

    public static AudioClip WEAPON_KendoSwing, WEAPON_LaserFire, WEAPON_RifleFire, WEAPON_Shotgun, AI_DeathScream;
    static AudioSource audioSrc;


    void Start()
    {
        WEAPON_KendoSwing = Resources.Load<AudioClip>("WEAPON_KendoSwing");
        WEAPON_LaserFire = Resources.Load<AudioClip>("WEAPON_LaserFire");
        WEAPON_RifleFire = Resources.Load<AudioClip>("WEAPON_RifleFire");
        WEAPON_Shotgun = Resources.Load<AudioClip>("WEAPON_Shotgun");
        AI_DeathScream = Resources.Load<AudioClip>("AI_DeathScream");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Kendo":
                audioSrc.PlayOneShot(WEAPON_KendoSwing);
                break;

            case "Laser":
                audioSrc.PlayOneShot(WEAPON_LaserFire);
                break;

            case "Rifle":
                audioSrc.PlayOneShot(WEAPON_RifleFire);
                break;

            case "Shotgun":
                audioSrc.PlayOneShot(WEAPON_Shotgun);
                break;

            case "Death":
                audioSrc.PlayOneShot(AI_DeathScream);
                break;
        }
    }
}
