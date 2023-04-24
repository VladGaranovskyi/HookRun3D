using System.Collections;
using UnityEngine;

public class MoveObjects : MonoBehaviour
{
    private Vector3 _start;
    [SerializeField] private Vector3 end;
    [SerializeField] private float speed;
    private Transform _obj;
    private bool IsForward;
    private Vector3 prePos = Vector3.zero;
    private Transform player;
    private bool IsGoing = true;
    

    private void Start()
    {
        _obj = transform;
        _start = _obj.transform.position;
        if(end.x == _start.x)
        {
            IsForward = true;
        }
        _obj.position = new Vector3(IsForward ? _obj.position.x : Random.Range(_obj.position.x, end.x), _obj.position.y, IsForward ? Random.Range(_obj.position.z, end.z) : _obj.position.z);
    }

    void FixedUpdate()
    {
        if (IsGoing)
        {
            if (player != null)
            {
                player.position += (_obj.position - prePos) * Time.deltaTime;
            }
            if (IsForward)
            {
                if (Mathf.Abs(_obj.position.z - end.z) < 0.1f)
                {
                    StartCoroutine(ChangeCor());
                }
                else if (Mathf.Abs(_obj.position.z - _start.z) < 0.1f)
                {
                    StartCoroutine(ChangeCor());
                }
                _obj.Translate(new Vector3(0f, speed * Time.deltaTime, 0f));
            }
            else
            {
                if (Mathf.Abs(_obj.position.x - end.x) < 0.1f)
                {
                    StartCoroutine(ChangeCor());
                }
                else if (Mathf.Abs(_obj.position.x - _start.x) < 0.1f)
                {
                    StartCoroutine(ChangeCor());
                }
                _obj.Translate(new Vector3(speed * Time.deltaTime, 0f, 0f));
            }
            prePos = _obj.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            player = collision.transform;
        }
    }

    private IEnumerator ChangeCor()
    {
        _obj.transform.Rotate(0f, 0f, 180f);
        IsGoing = false;
        yield return new WaitForSeconds(1f);
        IsGoing = true;
    }
}
