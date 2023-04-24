using System.Collections;
using UnityEngine;

public class ObjectTeleporter : MonoBehaviour
{
    [SerializeField] private float per;
    [SerializeField] private Transform posToTeleport;
    [SerializeField] private AudioSource clip;
    [SerializeField] private SpriteRenderer hook;
    private Transform _player;
    private bool IsDestroy;
    private GrappleHuman _grap;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _player = other.transform;
            _grap = _player.GetComponent<GrappleHuman>();
            IsDestroy = true;
            if (_grap.GetHooking())
            {
                _grap.UnGrapple();
            }
            StartCoroutine(TeleportCor());
        }
    }

    private void FixedUpdate()
    {
        if (_player == null) _player = GameObject.FindWithTag("player").transform;
        if (!IsDestroy)
        {
            if (Mathf.Abs(_player.position.z - transform.position.z) >= 25f) hook.enabled = false;
            else hook.enabled = true;
        }
    }

    private IEnumerator TeleportCor()
    {
        yield return new WaitForSeconds(per);
        _player.transform.position = posToTeleport.position;
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                clip.Play();
            }
            else if ((PlayerPrefs.GetString("Sound") == "Vib"))
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            clip.Play();
        }
    }
}
