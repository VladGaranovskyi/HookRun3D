using UnityEngine;

public class NonHookZone : MonoBehaviour
{
    private GrappleHuman _grap;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("player"))
        {
            _grap = collision.gameObject.GetComponent<GrappleHuman>();
            _grap.LetHook = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("player"))
        {
            _grap.LetHook = true;
            _grap = null;
        }
    }
}
