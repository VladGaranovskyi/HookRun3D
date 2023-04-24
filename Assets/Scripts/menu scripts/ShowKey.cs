using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowKey : MonoBehaviour
{
    [SerializeField] private string key;
    private Text txt;

    void Start()
    {
        txt = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        txt.text = PlayerPrefs.GetInt(key).ToString();
    }
}
