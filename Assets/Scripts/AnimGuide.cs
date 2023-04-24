using UnityEngine;

public class AnimGuide : MonoBehaviour
{
    [SerializeField] private GameObject[] animations;
    private Holder _holder;
    private bool IsEnter;

    private void Start()
    {
        _holder = FindObjectOfType<Holder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            foreach(GameObject g in animations)
            {
                g.SetActive(true);
            }
            IsEnter = true;
        }
    }

    private void FixedUpdate()
    {
        if (IsEnter)
        {
            float directionY = _holder.GetDirection().y;
            if(directionY < -400f)
            {
                foreach (GameObject g in animations)
                {
                    Destroy(g);
                }
                IsEnter = false;
                Destroy(gameObject);
            }
        }
    }
}
