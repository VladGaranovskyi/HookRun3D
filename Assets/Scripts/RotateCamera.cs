using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    private GameObject camera;
    public Vector3 quat = new Vector3(0f, 0f, 0f);    
    public Vector3 newOffset = new Vector3(0f, 1.5f, -5f);
    private Vector3 Rotate_sum;
    private bool IsEnter;

    private void Awake()
    {
        camera = GameObject.Find("Main Camera");
    }

    void OnTriggerEnter()
    {      
        camera = GameObject.Find("Main Camera");
        camera.GetComponent<FollowPlayer>().offset = newOffset;
        IsEnter = true;
        this.gameObject.GetComponent<Collider>().enabled = false;
    }    

    void FixedUpdate()
    {
        if (Rotate_sum.x != quat.x && IsEnter)
        { 
            camera.transform.Rotate(quat.x / 20f, 0f, 0f);
            Rotate_sum += new Vector3(quat.x / 20f, 0f, 0f);
        }
        else if(Rotate_sum.y != quat.y && IsEnter)
        {
            camera.transform.Rotate(0f, quat.y / 20f, 0f);
            Rotate_sum += new Vector3(0f, quat.y / 20f, 0f);
        }       
    }
}
