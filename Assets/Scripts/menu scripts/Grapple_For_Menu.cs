using UnityEngine;

public class Grapple_For_Menu : MonoBehaviour
{
    public Material line_color;
    public GameObject player;
    public GameObject Connect;
    private string skinName;
    private bool HasSkin;
    [SerializeField] private GameObject[] Skins;

    void Start()
    {
        if (PlayerPrefs.HasKey("Skin"))
        {
            foreach (GameObject Skin in Skins)
            {
                if (PlayerPrefs.GetString("Skin") == Skin.name)
                {
                    player.GetComponent<MeshRenderer>().material = Skin.GetComponent<MeshRenderer>().sharedMaterial;
                    skinName = Skin.name;
                    HasSkin = true;
                }
            }
        }
        player.AddComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        if (HasSkin)
        {
            if(skinName != PlayerPrefs.GetString("Skin"))
            {
                foreach (GameObject Skin in Skins)
                {
                    if (PlayerPrefs.GetString("Skin") == Skin.name)
                    {
                        player.GetComponent<MeshRenderer>().material = Skin.GetComponent<MeshRenderer>().sharedMaterial;
                        skinName = Skin.name;
                    }
                }
            }
        }
        LineRenderer lineRenderer = player.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = line_color;
        lineRenderer.SetPosition(0, new Vector3(Connect.transform.position.x, Connect.transform.position.y, Connect.transform.position.z));
        lineRenderer.SetPosition(1, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
    }
}
