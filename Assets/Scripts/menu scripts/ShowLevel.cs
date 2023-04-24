using UnityEngine;
using UnityEngine.UI;

public class ShowLevel : MonoBehaviour
{
    private Text textt;
    [SerializeField] private bool IsShowThing;
    [SerializeField] private string key;

    void Awake()
    {
        textt = this.gameObject.GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if (IsShowThing)
        {
            if (PlayerPrefs.HasKey(key))
            {
                textt.text = $"Level {PlayerPrefs.GetInt(key) + 1}";
            }
            else
            {
                textt.text = $"Level 1";
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("Level"))
            {
                if (PlayerPrefs.HasKey("pass"))
                {
                    textt.text = $"Level {PlayerPrefs.GetInt("CurrentLevel")}";
                }
                else
                {
                    textt.text = $"Level {PlayerPrefs.GetInt("Level")}";
                }
            }
            else
            {
                textt.text = "Level 1";
            }
        }
    }
}
