using System.Collections;
using UnityEngine;

public class DeathBall : MonoBehaviour
{
    [SerializeField] private bool IsBullet;
    private bool IsRifle;

    private void Start()
    {
        if (this.gameObject.CompareTag("Untagged"))
        {
            IsRifle = true;
            IsBullet = false;
        }
        StartCoroutine(DelCor());
    }

    private void FixedUpdate()
    {
        if (IsBullet)
        {
            transform.Translate(0f, 0.5f, 0f);
        }
        else if (IsRifle)
        {
            transform.Translate(0f, 0f, 0.5f);
        }
    }

    private IEnumerator DelCor()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
