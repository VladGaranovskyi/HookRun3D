using UnityEngine;

public class DeathObject : MonoBehaviour
{
    [SerializeField] private AudioSource sound;

    private void OnCollisionEnter(Collision collision)
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                sound.Play();
            }
            else if (PlayerPrefs.GetString("Sound") == "Vib")
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            sound.Play();
        }
    }
}
