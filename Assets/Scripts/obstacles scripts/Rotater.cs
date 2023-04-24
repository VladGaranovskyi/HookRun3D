using System.Collections;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    [SerializeField] private bool IsSaw;
    [SerializeField] private bool IsWheel;


    void Start()
    {
        if (!IsWheel) this.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
    }

    
    void FixedUpdate()
    {
        if (IsSaw)
        {
            this.transform.Rotate(0f, 0f, rotationSpeed);
        }
        else if (IsWheel)
        {
            this.transform.Rotate(rotationSpeed, 0f, 0f);
        }
        else
        {
            this.transform.Rotate(0f, rotationSpeed, 0f);
        }
    }   
}
