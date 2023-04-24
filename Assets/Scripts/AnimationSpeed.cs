using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeed : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float speed = 1f;

    private void Awake()
    {
        anim.speed = speed;
    }
}
