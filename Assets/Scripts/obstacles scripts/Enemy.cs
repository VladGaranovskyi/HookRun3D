using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] bonesLook;
    [SerializeField] private float distanceLook;
    [SerializeField] private float per;
    [SerializeField] private GameObject laserBullet;
    [SerializeField] private Transform pointKick;
    [SerializeField] private Collider[] hitBoxes;
    [SerializeField] private Vector3 forceKick;
    [SerializeField] private Transform skin;
    private bool IsShoot = true;
    private bool letShoot = true;
    private Transform player;
    private Animator _anim;
    private GrappleHuman _grap;
    private Rigidbody _rb;
    [SerializeField] private AudioSource sound;
    [SerializeField] private AudioSource soundKick;
    private AudioPlayer ap;

    private void Start()
    {
        ap = FindObjectOfType<AudioPlayer>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            player.position = pointKick.position;
            _grap = other.GetComponent<GrappleHuman>();
            letShoot = false;
            if (!_grap.GetHooking())
            {
                _anim = other.GetComponent<Animator>();
                _anim.SetBool("KickBool", true);
                StartCoroutine(KickCor());
            }
            StartCoroutine(RagDollCor());
        }
    }

    private void FixedUpdate()
    {
        if (player == null) player = GameObject.FindWithTag("player").transform;
        if(transform.position.z - player.position.z <= distanceLook && transform.position.z > player.position.z && letShoot)
        {
            bonesLook[0].LookAt(player, Vector3.up/*new Vector3(1, 0, 1)*/);
            bonesLook[1].up = -(bonesLook[1].position) + player.position;
            if (IsShoot) StartCoroutine(ShootCor());
        }
    }

    private IEnumerator ShootCor()
    {
        IsShoot = false;
        GameObject bullet = Instantiate(laserBullet);
        bullet.transform.position = bonesLook[0].position;
        bullet.transform.up = bonesLook[0].forward;
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                ap.PlayAudio(sound, sound.clip);
            }
            else if (PlayerPrefs.GetString("Sound") == "Vib")
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            ap.PlayAudio(sound, sound.clip);
        }
        yield return new WaitForSeconds(per);
        IsShoot = true;
    }

    private IEnumerator KickCor()
    {
        yield return new WaitForSeconds(0.07f);
        _anim.SetBool("KickBool", false);
    }

    private IEnumerator RagDollCor()
    {
        if (_grap.GetHooking())
        {
            yield return new WaitForSeconds(0.02f);
        }
        else
        {
            yield return new WaitForSeconds(0.08f);
        }
        foreach (Collider c in hitBoxes)
        {
            c.isTrigger = false;
            Rigidbody rb = c.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        _rb.useGravity = true;
        skin.transform.parent = hitBoxes[0].transform;
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                ap.PlayAudio(soundKick, soundKick.clip);
            }
            else if (PlayerPrefs.GetString("Sound") == "Vib")
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            ap.PlayAudio(soundKick, soundKick.clip);
        }
        hitBoxes[1].GetComponent<Rigidbody>().AddForce(forceKick, ForceMode.Impulse);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
