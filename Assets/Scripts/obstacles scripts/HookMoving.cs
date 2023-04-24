using UnityEngine;
using System.Collections;

public class HookMoving : MonoBehaviour
{
    [SerializeField] private GameObject gHook;
    [SerializeField] private Material mat;
    [SerializeField] private Transform posPl;
    [SerializeField] private Transform posCam;
    [SerializeField] private LayerMask layer;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float posSpawn;
    private Transform arm4Hook;
    private Transform body;
    private Transform _player;
    private Transform _cam;
    private Camera _camMain;
    private Holder _holder;
    private GrappleHuman _grap;
    private bool IsEnter;
    private bool IsConnected;
    private bool IsTook;
    private bool IsEnemy;
    private bool IsSpawned;
    private RaycastHit _hit;
    private Ray _ray;
    private LineRenderer _lineRenderer;
    private Transform hookRef;
    private Transform currentObj;
    private Rigidbody currentObjRigid;

    private void Start()
    {
        _holder = FindObjectOfType<Holder>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            _player = other.transform;
            _player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _player.position = posPl.position;
            _cam.GetComponent<FollowHuman>().enabled = false;
            _cam.parent = null;
            _cam.position = posCam.position;
            _cam.eulerAngles = posCam.eulerAngles;
            _camMain.fieldOfView = 30f;
            _grap = _player.GetComponent<GrappleHuman>();
            _player.GetComponent<Animator>().SetBool("RifleBool", true);
            arm4Hook = _grap.GetConnection();
            body = _grap.GetColliders()[0].transform;
            if (_grap.GetHooking())
            {
                _grap.UnGrapple(false);
            }
            foreach(Collider r in _grap.GetColliders())
            {
                Rigidbody rr = r.GetComponent<Rigidbody>();
                rr.useGravity = false;
                rr.isKinematic = true;
            }
            _grap.enabled = false;
            IsEnter = true;
        }
    }

    private void FixedUpdate()
    {
        if(_cam == null)
        {
            _camMain = Camera.main;
            _cam = _camMain.transform;
        }
        if (IsEnter)
        {
            if (!IsSpawned)
            {
                StartCoroutine(SpawnCor());
            }
            if(_holder.GetPosition() != Vector2.zero && _holder.IsTapped())
            {
                _ray = _camMain.ScreenPointToRay(_holder.GetPosition());
            }
            if (IsTook || Physics.Raycast(_ray, out _hit, Mathf.Infinity, layer))
            {
                
                if (_holder.IsTapped())
                {
                    if (currentObj == null && !IsEnemy && _hit.collider.GetComponent<EnemyBonusLevel>() != null)
                    {
                        _hit.collider.GetComponent<EnemyBonusLevel>().IsGrabbed = true;
                        IsEnemy = true;
                    }
                    if (!IsTook)
                    {
                        hookRef = Instantiate(gHook).transform;
                        hookRef.transform.position = arm4Hook.position;
                        //_hit.collider.transform.position += Vector3.up;
                        _player.GetComponent<Animator>().enabled = false;
                        currentObj = IsEnemy ? _hit.collider.transform.GetComponent<EnemyBonusLevel>().GetPelvis() : _hit.collider.transform;
                        Debug.Log(currentObj.name);
                        currentObjRigid = currentObj.gameObject.GetComponent<Rigidbody>();
                        if (IsEnemy) currentObjRigid.AddForce(Vector3.up * 100f, ForceMode.Impulse);
                    }
                    _lineRenderer = _player.gameObject.GetComponent<LineRenderer>();
                    if (_lineRenderer == null)
                    {
                        _lineRenderer = _player.gameObject.AddComponent<LineRenderer>();
                    }
                    _lineRenderer.startWidth = 0.03f;
                    _lineRenderer.endWidth = 0.05f;
                    _lineRenderer.material = mat;
                    _lineRenderer.SetPosition(0, arm4Hook.position);
                    _lineRenderer.SetPosition(1, hookRef.position);
                    if(Mathf.Abs(hookRef.position.z - _hit.transform.position.z) > 0.01f && !IsConnected)
                    {
                        hookRef.position = Vector3.Lerp(hookRef.position, currentObj.position, Time.deltaTime * 5f);
                    }
                    else
                    {
                        hookRef.position = currentObj.position;
                        Vector2 dir = _holder.GetDirection() * Time.deltaTime;
                        currentObjRigid.AddForce(dir.x * currentObjRigid.mass * (IsEnemy ? 3f : 1f), /*(IsEnemy ? 300f : 0f)*/0f, dir.y * currentObjRigid.mass * (IsEnemy ? 3f : 1f), ForceMode.Acceleration);
                        IsConnected = true;
                    }
                    Vector3 arrow = (hookRef.position - arm4Hook.position).normalized;
                    arm4Hook.parent.up = arrow;
                    //body.forward = arrow;
                    IsTook = true;
                }
                else
                {
                    IsTook = false;
                    IsConnected = false;
                    IsEnemy = false;
                    currentObj = null;
                    currentObjRigid.AddForce(Vector3.forward * 10f, ForceMode.Impulse);
                    currentObjRigid = null;
                    Destroy(_lineRenderer);
                    Destroy(hookRef.gameObject);
                }
            }
        }
    }

    private IEnumerator SpawnCor()
    {
        GameObject enemy1 = Instantiate(enemyPrefab);
        GameObject enemy2 = Instantiate(enemyPrefab);
        GameObject enemy3 = Instantiate(enemyPrefab);
        enemy1.transform.position = Vector3.right * -2f + Vector3.forward * posSpawn + Vector3.up * 3f;
        enemy2.transform.position = Vector3.forward * posSpawn + Vector3.up * 3f;
        enemy3.transform.position = Vector3.right * 2f + Vector3.forward * posSpawn + Vector3.up * 3f;
        IsSpawned = true;
        yield return new WaitForSeconds(7f);
        IsSpawned = false;
    }
}
