using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    private string previousSound = "Not Selected";
    private GameObject currentSound;
    private GameObject previousObjSound;
    [SerializeField] private Transform bckgr;

    void Awake()
    {
        previousSound = PlayerPrefs.GetString("Sound");
        previousObjSound = bckgr.Find(previousSound).gameObject;
        previousObjSound.GetComponent<Image>().color = Color.blue;
    }

    public void OpenSettings(GameObject settings)
    {
        settings.SetActive(!settings.activeSelf);
    }

    public void SetSound(string status)
    {
        if(previousSound != status)
        {
            PlayerPrefs.SetString("Sound", status);
            currentSound = bckgr.Find(status).gameObject;
            Image currentSoundImg = currentSound.GetComponent<Image>();
            currentSoundImg.color = Color.blue;
            if(previousSound == "Not Selected")
            {
                previousSound = status;
                previousObjSound = currentSound;
            }
            else
            {
                previousObjSound.GetComponent<Image>().color = Color.white;
            }
            previousSound = status;
            previousObjSound = currentSound;
            PlayerPrefs.Save();
        }
    }
}
