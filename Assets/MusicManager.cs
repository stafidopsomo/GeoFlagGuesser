using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource BGM;

    public void Update()
    {
        if (PlayerPrefs.GetInt("MuteBGM") == 1)
        {
            BGM.mute = true;
        }
        else
        {
            BGM.mute = false;
        }
    }
}
