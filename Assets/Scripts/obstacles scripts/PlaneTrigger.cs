using System.Collections;
using UnityEngine;

public class PlaneTrigger : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    [SerializeField] private float distance;
    [SerializeField] private Transform Start;
    [SerializeField] private Transform End;
    [SerializeField] private float speed;
    private Transform _start;
    private Transform _end;
    private Vector3 startPos;
    private GameObject _plane;
    private Zipline zip;
    private Transform player;
    private Holder holder;
    private bool IsEnter;
    private Rigidbody _rb;
    private GrappleHuman _grap;
    private bool IsLetHook;

    private void Awake()
    {
        holder = FindObjectOfType<Holder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            player = other.transform;
            //_grap = player.GetComponent<GrappleHuman>();
            //if (_grap.GetHooking())
            //{
            //    _grap.UnGrapple();
            //}
           // _grap.SetIsStart(false);
            //_rb = player.GetComponent<Rigidbody>();
            _plane = Instantiate(plane);
            zip = _plane.GetComponent<Zipline>();
            zip.enabled = false;
            _plane.transform.position = Start.transform.position + (Vector3.up * 7) + (Vector3.forward * (-7));
            //_plane.transform.parent = this.transform;
            IsEnter = true;
            //Time.timeScale = 0.3f;
            //StartCoroutine(HookCor());
        }
    }

    private void FixedUpdate()
    {
        if (IsEnter)
        {
            _plane.transform.Translate(0f, 0f, speed);
            _plane.transform.LookAt(Start);
            if((_plane.transform.position - Start.position).sqrMagnitude <= 0.5f)
            {
                _plane.transform.position = Start.position;
                _plane.transform.eulerAngles = Vector3.zero;
                zip.enabled = true;
                Start.parent = null;
                _start = Instantiate(Start.gameObject).transform;
                End.parent = null;
                _end = Instantiate(End.gameObject).transform;
                _start.position = _plane.transform.position;
                _end.position = _plane.transform.position + (Vector3.forward * distance);
                zip.SetSE(_start, _end);
                //zip.HookUp();
                Start.parent = this.transform;
                End.parent = this.transform;
                IsEnter = false;
            }
        }
    }
}
