using UnityEngine;
using UnityEngine.SceneManagement;

public class SceeneChanger : MonoBehaviour
{
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
