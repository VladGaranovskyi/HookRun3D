using UnityEngine;

public class HookHelper : MonoBehaviour
{
    [SerializeField] private Transform hookHelperMark;
    private Transform camera;

    private void Awake()
    {
        camera = GameObject.Find("Main Camera").transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            Destroy(hookHelperMark.gameObject);
        }
    }

    void FixedUpdate()
    {
        hookHelperMark.LookAt(camera);
    }
}
