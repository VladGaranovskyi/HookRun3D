using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHuman : MonoBehaviour
{
    private GameObject player;
    private GrappleHuman grapple;
    private Camera camera;
    [SerializeField] private bool IsSmooth;
    public Vector3 offset;
    private Vector3 prePos = Vector3.zero;
    private bool IsNotReset;
    private Transform pivot;
    private float currentAngle;
    private Vector3 direction;
    private bool IsRotate = true;
    private Rigidbody _rb;
    [SerializeField] private ParticleSystem warp;
    private bool IsIncreasing;
    private bool IsIncr;
    private bool IsRot;

    void Awake()
    {
        if (this.gameObject == GameObject.Find("Main Camera"))
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            camera = this.gameObject.GetComponent<Camera>();
            pivot = GameObject.Find("Pivot").transform;
        }
    }

    void LateUpdate()
    {
        if (camera == null)
        {
            camera = this.gameObject.GetComponent<Camera>();
        }
        if(pivot == null)
        {
            pivot = GameObject.Find("Pivot").transform;
        }
        if (player == null)
        {
            player = GameObject.FindWithTag("player");
            _rb = player.GetComponent<Rigidbody>();
            pivot.position = player.transform.position + new Vector3(0f, 0.8f, 0f);
            this.transform.position = pivot.transform.position + offset;
            this.transform.LookAt(pivot);
        }
        if (grapple == null)
        {
            grapple = player.GetComponent<GrappleHuman>();
        }
        pivot.position = player.transform.position + new Vector3(0f, 0.8f, 0f);
        if (camera.transform.parent != pivot.transform && !IsRot)
            camera.transform.SetParent(pivot.transform);

        if (IsIncreasing)
        {
            if(camera.fieldOfView < 90f && !IsIncr)
            {
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 100f, Time.deltaTime * 3f);
                var em = warp.emission;
                em.rateOverTime = 27f;
                var shape = warp.shape;
                shape.radius = 2f;
                warp.startSpeed = 7f;
            }
            else if(camera.fieldOfView > 60f)
            {
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 50f, Time.deltaTime * 3f);
                IsIncr = true;
            }
            else
            {
                var emm = warp.emission;
                emm.rateOverTime = 17f;
                var shapee = warp.shape;
                shapee.radius = 3f;
                warp.startSpeed = 5f;
                IsIncreasing = false;
                IsIncr = false;
            }
        }
        else if (IsRot)
        {
            if(!IsIncr)
            {
                Invoke("ResetIsRot", 1f); 
                pivot.eulerAngles = Vector3.zero; 
                IsIncr = true;
            }
            Time.timeScale = 0.3f;
            if (Mathf.Abs(pivot.eulerAngles.y - 135f) > 1f)
                pivot.eulerAngles = new Vector3(0f, Mathf.Lerp(pivot.eulerAngles.y, 135f, Time.deltaTime * 5f), 0f);
        }
        else camera.fieldOfView = 60f + Mathf.Abs(_rb.velocity.z / 4f);

        direction = pivot.transform.position - prePos;
        if (IsNotReset)
        {
            pivot.eulerAngles = Vector3.zero;
            IsNotReset = false;
        }
        else if (IsRotate && !IsRot)
        {
            if (grapple.GetIsUngr())
            {
                pivot.eulerAngles = Vector3.zero;
            }
            else
            {
                if (!grapple.GetIsGr())
                {
                    currentAngle = pivot.eulerAngles.x + GetAngle(transform.forward, new Vector3(0f, direction.y, direction.z), Vector3.right);
                    if (direction.z >= 0f)
                    {
                        pivot.eulerAngles = new Vector3(Mathf.Lerp(pivot.eulerAngles.x, currentAngle, Time.deltaTime), 0f, 0f);
                    }
                }              
            }                
        }
        prePos = pivot.transform.position;
        
    }

    private float GetAngle(Vector3 v1, Vector3 v2, Vector3 axis)
    {
        v1 -= Vector3.Project(v1, axis);
        v2 -= Vector3.Project(v2, axis);
        float angle = Vector3.Angle(v1, v2);
        return angle * (Vector3.Dot(axis, Vector3.Cross(v1, v2)) < 0 ? -1 : 1);
    }

    private void ResetIsRot()
    {
        IsRot = false;
        IsIncr = false;
        Time.timeScale = 1f;
    }

    public void ResetPivot(bool IsIncrr)
    {
        IsNotReset = true;
        if (IsIncrr) IsIncreasing = true;       
        else IsRot = true;
    }

    public void ForbideRotate()
    {
        IsRotate = false;
    }
}
