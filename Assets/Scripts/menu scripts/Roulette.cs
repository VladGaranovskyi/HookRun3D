using UnityEngine;

public class Roulette : MonoBehaviour
{
    [SerializeField] private float borderLeft;
    [SerializeField] private float borderRight;
    [SerializeField] private float step;
    private float currentX;
    private bool IsRight;

    private void FixedUpdate()
    {
        currentX = transform.position.x - 540f;
        if (IsRight)
        {
            transform.position += new Vector3(step, 0f);
        }
        else
        {
            transform.position += new Vector3(-step, 0f);
        }
        if (currentX <= borderLeft)
        {
            IsRight = true;
        }
        else if (currentX >= borderRight)
        {
            IsRight = false;
        }
    }
}
