using System.Collections;
using UnityEngine;

public class ChangeMode : MonoBehaviour
{
    [SerializeField] private bool IsChangeToBall;
    [SerializeField] private bool IsMain;
    [SerializeField] private Transform camPos;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject Changer;
    [SerializeField] private GameObject changeCamera;
    [SerializeField] private Vector3 force;
    private GameObject changeCharachter;
    private GameObject player;
    private GameObject currentCamera;
    private bool IsEnter;
    private GameManager manager;
    private GrappleHuman _grap_hum;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        changeCharachter = manager.GetHumanOrBall(IsChangeToBall);
        //changeCamera = manager.GetHumanOrBallCamera(IsChangeToBall);
    }

    private void FixedUpdate()
    {
        //Debug.Log(changeCharachter.name);
        if (IsEnter)
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("player");
            }
            if (currentCamera == null)
            {
                /*if (IsChangeToBall)
                {
                    currentCamera = FindObjectOfType<FollowHuman>().gameObject;
                }
                else
                {
                    currentCamera = FindObjectOfType<FollowPlayer>().gameObject;
                }*/
                currentCamera = Camera.main.gameObject;
            }
            if (IsChangeToBall)
            {
                if (Mathf.Abs(player.transform.position.x - transform.position.x) > 0.001f && Mathf.Abs(player.transform.position.y - transform.position.y) > 0.001f)
                {
                    player.transform.position = Vector3.Lerp(player.transform.position, transform.position, 2f * Time.deltaTime * player.GetComponent<Rigidbody>().velocity.z);
                }
                else if (!IsMain)
                    IsEnter = false;
                if(_grap_hum != null)
                {
                    if (_grap_hum.GetHooking())
                    {
                        _grap_hum.UnGrapple(false);
                    }
                    else
                    {
                        _grap_hum.enabled = false;
                    }
                }
                //currentCamera.transform.position = Vector3.Lerp(currentCamera.transform.position, camPos.position, Time.deltaTime);
                //currentCamera.transform.eulerAngles = new Vector3(Mathf.Lerp(currentCamera.transform.eulerAngles.x, 0f, Time.deltaTime), Mathf.Lerp(currentCamera.transform.eulerAngles.y, 270f, Time.deltaTime), 0f);
            }
            else
            {
                if (Mathf.Abs(player.transform.position.x - transform.position.x) > 0.001f && Mathf.Abs(player.transform.position.y - transform.position.y) > 0.001f)
                {
                    player.transform.position = Vector3.Lerp(player.transform.position, transform.position, 3f * Time.deltaTime);
                }
                else
                {
                    player.transform.position += new Vector3(0f, 0f, Time.deltaTime * 3f);
                }
                currentCamera.transform.position = Vector3.Lerp(currentCamera.transform.position, camPos.position, Time.deltaTime * 1.5f);
                currentCamera.transform.eulerAngles = new Vector3(Mathf.Lerp(currentCamera.transform.eulerAngles.x, 0f, Time.deltaTime * 1.5f), Mathf.Lerp(currentCamera.transform.eulerAngles.y, 270f, Time.deltaTime * 1.5f), 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("player");
            }
            if (currentCamera == null)
            {
                /*if (IsChangeToBall)
                {
                    currentCamera = FindObjectOfType<FollowHuman>().gameObject;
                }
                else
                {
                    currentCamera = FindObjectOfType<FollowPlayer>().gameObject;
                }*/
                currentCamera = Camera.main.gameObject;
            }
            if (IsMain)
            {
                if (IsChangeToBall)
                {
                    Change(true);
                }
                else
                {
                    Change(false);
                }
            }
            else
            {
                IsEnter = true;
                if (IsChangeToBall)
                {
                    _grap_hum = player.GetComponent<GrappleHuman>();//.enabled = false;
                    Rigidbody rbb = player.GetComponent<Rigidbody>();
                    rbb.velocity = Vector3.forward * rbb.velocity.z;
                    rbb.useGravity = false;
                    player.GetComponent<Animator>().SetBool("ReturnBool", true);
                    //currentCamera.GetComponent<FollowHuman>().enabled = false;
                }
                else
                {
                    player.GetComponent<Grapple>().enabled = false;
                    currentCamera.GetComponent<FollowPlayer>().enabled = false;
                    player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    player.GetComponent<Rigidbody>().useGravity = false;
                    currentCamera.transform.parent = null;
                }
            }
        }
    }

    private void Change(bool b)
    {
        if (b)
        {
            StartCoroutine(CoroutineChange());
        }
        else
        {
            GameObject.Find("Pivot").transform.eulerAngles = Vector3.zero;
            changeCamera = Instantiate(changeCamera);
            Destroy(player);
            Destroy(currentCamera);
            manager.SpawnHuman();
            player = manager.GetHumanOrBall(false);
            //player = changeCharachter;
            player.transform.position = spawnPos.position;
            //currentCamera = changeCamera;
            FindObjectOfType<Follower>().enabled = b;
            IsEnter = false;
            Destroy(Changer);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator CoroutineChange()
    {
        Destroy(Changer);
        player.GetComponent<Animator>().SetBool("ChangeBool", true);
        player.GetComponent<Rigidbody>().useGravity = true;
        player.GetComponent<Rigidbody>().AddForce(force * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        manager.SpawnBall();
        changeCamera = Instantiate(changeCamera);
        player.GetComponent<Animator>().SetBool("ChangeBool", false);
        player.GetComponent<Animator>().SetBool("ReturnBool", false);
        Vector3 pos = player.transform.position;
        Vector3 vel = player.GetComponent<Rigidbody>().velocity;
        Destroy(player);
        Destroy(currentCamera);
        player = manager.GetHumanOrBall(true);
        player.transform.position = pos;
        player.GetComponent<Rigidbody>().velocity = vel;
        currentCamera = changeCamera;
        currentCamera.transform.position = player.transform.position + new Vector3(0f, 1.5f, -5f);
        FindObjectOfType<Follower>().enabled = true;
        player.GetComponent<Grapple>().enabled = true;
        currentCamera.GetComponent<FollowPlayer>().enabled = true;
        GameObject.Find("Pivot").transform.eulerAngles = Vector3.zero;
        IsEnter = false;
        Destroy(this.gameObject);
    }
}
