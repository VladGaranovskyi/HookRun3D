using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_menu : MonoBehaviour
{
    private AudioSource source;

    void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("time-code"))
        {
            source.time = PlayerPrefs.GetFloat("time-code");
        }
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "Off" || PlayerPrefs.GetString("Sound") == "Vib")
            {
                Destroy(this.gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        PlayerPrefs.SetFloat("time-code", source.time);
    }
}
