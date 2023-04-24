using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public void PlayAudio(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
