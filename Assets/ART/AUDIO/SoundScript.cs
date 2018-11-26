using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{

    public static AudioClip Kendo, Laser, Rifle, Shotgun, EnemyDeath;
    static AudioSource audioSrc;


    void Start()
    {
        Kendo = Resources.Load<AudioClip>("Kendo");
        Laser = Resources.Load<AudioClip>("LASER");
        Rifle = Resources.Load<AudioClip>("Rifle");
        Shotgun = Resources.Load<AudioClip>("ShotGun");
        EnemyDeath = Resources.Load<AudioClip>("Scream");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "Kendo":
                audioSrc.PlayOneShot(Kendo);
                break;

            case "Laser":
                audioSrc.PlayOneShot(Laser);
                break;

            case "Rifle":
                audioSrc.PlayOneShot(Rifle);
                break;

            case "Shotgun":
                audioSrc.PlayOneShot(Shotgun);
                break;

            case "Death":
                audioSrc.PlayOneShot(EnemyDeath);
                break;
        }
    }
}
