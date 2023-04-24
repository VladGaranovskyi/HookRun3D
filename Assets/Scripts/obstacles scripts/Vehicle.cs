using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private float speed;
    [SerializeField] private float speedRotate;
    [SerializeField] private bool IsCar;
    [SerializeField] private AudioSource sound;
    private bool IsSoundPlayed;
    private float rotationCar;
    private Vector3 startPos;
    private Transform player;
    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (player == null) player = GameObject.FindWithTag("player").transform;
        transform.Translate(0f, 0f, speed * Time.deltaTime);
        if(rotationCar != 0f)
        {
            if (Mathf.Abs(rotationCar - transform.eulerAngles.y) > 0.001f)
                transform.eulerAngles = new Vector3(0f, Mathf.Lerp(transform.eulerAngles.y, rotationCar, speedRotate * Time.deltaTime), 0f);
            else transform.eulerAngles = new Vector3(0f, rotationCar, 0f);
        }
        if (IsCar)
        {
            if ((transform.position - player.position).sqrMagnitude <= 1600f)
            {
                if (!IsSoundPlayed)
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
                    IsSoundPlayed = true;
                }
            }
            else
            {
                sound.Stop();
                IsSoundPlayed = false;
            }
        }
    }

    public void ChangeEuler(float r)
    {
        rotationCar = r;
    }

    public GameObject[] GetCars()
    {
        return cars;
    }
    
    public Vector3 GetStart()
    {
        return startPos;
    }

    public float[] GetSpeed()
    {
        return new float[2] { speed, speedRotate };
    }

    public void SetSpeed(float speed1, float speedRotate1)
    {
        speed = speed1;
        speedRotate = speedRotate1;
    }
}
