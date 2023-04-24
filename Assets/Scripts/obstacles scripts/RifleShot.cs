using System.Collections;
using UnityEngine;

public class RifleShot : MonoBehaviour
{
    [SerializeField] private Transform rifle;
    [SerializeField] private Transform shootDirection;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float speedShoot;
    [SerializeField] private Transform setPos;
    [SerializeField] private GameObject laser;
    [SerializeField] private float hp;
    [SerializeField] private Vector3 offsetRifle;
    [SerializeField] private Transform bossMain;
    [SerializeField] private string child;
    [SerializeField] private Vector3 forceAShooting;
    [SerializeField] private GameObject[] stand;
    [SerializeField] private Transform newBossPos;
    [SerializeField] private GameObject partsIdle;
    [SerializeField] private ParticleSystem partsShoot;
    [SerializeField] private AudioSource source;
    [SerializeField] private bool IsRPG;
    private Transform _player;
    private GrappleHuman _grapple;
    private Transform _cam;
    private Animator _anim;
    private bool IsEnter;
    private bool IsShot;
    private Holder _holder;
    private Vector2 _direction;
    private Boss[] _bosses;
    private Rigidbody _rb;
    private Animator _bossAnim;
    private bool IsKilledBoss;

    private void Start()
    {
        _holder = FindObjectOfType<Holder>();
        _bosses = FindObjectsOfType<Boss>();
        _bossAnim = bossMain.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _player = other.transform;
            _grapple = _player.GetComponent<GrappleHuman>();
            _cam = Camera.main.transform;
            _anim = _player.GetComponent<Animator>();
            if (_grapple.GetHooking())
            {
                _grapple.UnGrapple(false);
            }
            _grapple.SetIsStart(false);
            _rb = _player.GetComponent<Rigidbody>();
            _rb.velocity = Vector3.zero;
            _player.position = setPos.position;
            _anim.SetBool("RifleBool", true);
            laser.SetActive(true);
            foreach(Boss b in _bosses) b.SetRS(this);
            rifle.eulerAngles = Vector3.zero;
            rifle.position = _player.position + offsetRifle;
            partsIdle.SetActive(false);
            IsEnter = true;
            IsShot = true;
        }
    }

    private void FixedUpdate()
    {
        if (IsEnter)
        {
            if (IsShot)
            {
                StartCoroutine(ShootCor());
            }
            _direction = _holder.GetDirection();
            if (_direction.x > 500f)
            {
                _direction.x = 500f;
            }
            else if (_direction.x < -500f)
            {
                _direction.x = -500f;
            }
            if (_direction.y > 500f)
            {
                _direction.y = 500f;
            }
            else if (_direction.y < -500f)
            {
                _direction.y = -500f;
            }
            _player.Rotate(0f, _direction.x / 20 * Time.deltaTime, 0f);
            rifle.Rotate(-(_direction.y) / 20 * Time.deltaTime, _direction.x / 20 * Time.deltaTime, 0f);
            if(hp <= 0)
            {
                _anim.SetBool("RifleBool", false);
                _grapple.SetIsStart(true);
                _rb.AddForce(forceAShooting);
                if (IsRPG)
                {
                    Destroy(rifle.gameObject);
                    Destroy(this.gameObject);
                }
                else
                {
                    bossMain.Find(child).gameObject.SetActive(false);
                    _bossAnim.SetBool("FlipBool", true);
                    StartCoroutine(BossCor());
                }
                foreach (GameObject g in stand)
                {
                    Destroy(g);
                }
                IsEnter = false;
            }
        }
        else if(IsKilledBoss)
        {
            bossMain.Translate(0f, 1f, 0f);
        }
        else
        {
            rifle.Rotate(0f, 1f, 0f);
        }
    }

    private IEnumerator ShootCor()
    {
        IsShot = false;
        GameObject g = Instantiate(bullet);
        partsShoot.Play();
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                source.Play();
            }
            else if (PlayerPrefs.GetString("Sound") == "Vib")
            {
                Handheld.Vibrate();
            }
        }
        else
        {
            source.Play();
        }
        g.transform.position = shootDirection.position;
        g.transform.rotation = shootDirection.rotation;
        yield return new WaitForSeconds(speedShoot);
        IsShot = true;
    }

    private IEnumerator BossCor()
    {
        IsKilledBoss = true;
        yield return new WaitForSeconds(3f);
        bossMain.position = newBossPos.position;
        bossMain.eulerAngles = Vector3.up * 180f;
        _bossAnim.SetBool("FlipBool", false);
        Destroy(rifle.gameObject);
        Destroy(this.gameObject);
    }

    public void SubHP()
    {
        hp--;
    }
}
