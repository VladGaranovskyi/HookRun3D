using System.Collections;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] private Transform connectPos;
    [SerializeField] private Vector3 forceARun;
    private Holder holder;
    private GameObject player;
    private Rigidbody rb;
    private GrappleHuman grap;
    private Animator anim;
    private Transform cam;
    private bool IsEnter;
    private bool IsGoing;
    private Vector2 direction;

    private void Start()
    {
        holder = FindObjectOfType<Holder>();
        cam = Camera.main.transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            player = GameObject.FindWithTag("player");
            anim = player.GetComponent<Animator>();
            grap = player.GetComponent<GrappleHuman>();
            rb = player.GetComponent<Rigidbody>();
            if (grap.GetHooking())
            {
                grap.UnGrapple(false);
            }
            grap.SetIsStart(false);
            IsEnter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player")) CancelRun();      
    }

    private void FixedUpdate()
    {
        if (IsEnter)
        {
            if ((holder.IsTapped() || IsGoing) && !grap.GetHooking())
            {
                if (!IsGoing) player.transform.position = new Vector3(connectPos.position.x, player.transform.position.y, player.transform.position.z);
                anim.SetBool("WallRunBool", true);
                IsGoing = true;
                direction = holder.GetDirection();
                if (direction.y > 777f)
                {
                    direction.y = 777f;
                }
                else if (direction.y < -777f)
                {
                    direction.y = -777f;
                }
                rb.velocity = Vector3.up * direction.y * Time.deltaTime / 2 + Vector3.forward * 1000f * Time.deltaTime;
            }
            else if(!holder.IsTapped() && IsGoing && !grap.GetHooking()) CancelRun();
        }
    }

    private void CancelRun()
    {
        rb.AddForce(forceARun, ForceMode.Impulse);
        anim.SetBool("WallRunBool", false);
        StartCoroutine(JumpCor());
        IsGoing = false;
        IsEnter = false;
    }

    private IEnumerator JumpCor()
    {
        yield return new WaitForSeconds(0.777f);
        grap.SetIsStart(true);
    }
}
