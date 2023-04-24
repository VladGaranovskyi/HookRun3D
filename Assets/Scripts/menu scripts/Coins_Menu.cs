using UnityEngine;
using UnityEngine.UI;

public class Coins_Menu : MonoBehaviour
{
    private Text text;
    public string key;
    public int i = 1;
    public bool IsEnd;
    public bool IsSelf;
    public float multiply = 1f;

    void Start()
    {
        if (IsSelf || key == "Dash" || key == "Jump")
        {
            text = this.gameObject.GetComponent<Text>();
        }
        else
        {
            text = this.transform.GetChild(0).gameObject.GetComponent<Text>();
        }
    }

    void Update()
    {
        if (PlayerPrefs.HasKey(key))
        {
            text.text = (PlayerPrefs.GetFloat(key) * i * multiply).ToString();
        }
        else
        {
            PlayerPrefs.SetFloat(key, 0);
        }
    }
}
