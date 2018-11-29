using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>

    // Erv comment code plz

// </Summary>

public class MENU_MenuBlip : MonoBehaviour {

    public AudioSource MenuBlipSource;
    public AudioClip HoverBlip;
    public AudioClip ClickBlip;
    public AudioClip ReleaseBlip;

    public void HoverSound()
    {
        MenuBlipSource.PlayOneShot(HoverBlip);
    }

    public void ClickSound()
    {
        MenuBlipSource.PlayOneShot(ClickBlip);
    }

    public void ReleaseSound()
    {
        MenuBlipSource.PlayOneShot(ReleaseBlip);
    }
}
