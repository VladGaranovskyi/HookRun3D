using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public GameObject[] Skins;
    private ParticleSystem psd;
    private GameObject Player;
    //[SerializeField] private float Height = -33f;
    private Vector3 checkPoint;
    private bool IsStarted;
    [SerializeField] private Coins_Menu coinsText;
    private Holder holder;
    private GameObject RestartMenu;
    private bool IsRestarted;
    [SerializeField] private GameObject[] people;
    private GameObject human;
    //[SerializeField] private GameObject cameraBall;
    [SerializeField] private GameObject cameraHuman;
    private GameObject currentCamera;
    private GameObject currentPlayer;
    private GameObject gameMusic;
    private int groundCount;
    [SerializeField] private GameObject[] HookUpHint;

    void Start()
    {
        if (PlayerPrefs.HasKey("newLevel"))
        {
            if(PlayerPrefs.GetInt("newLevel") == 1)
            {
                int setLevel;
                if (PlayerPrefs.HasKey("CurrentLevel"))
                {
                    setLevel = PlayerPrefs.GetInt("CurrentLevel");
                }
                else
                {
                    setLevel = PlayerPrefs.GetInt("Level");
                }
                YsoCorp.GameUtils.YCManager.instance.OnGameStarted(setLevel);
                PlayerPrefs.SetInt("newLevel", 0);
            } 
        }
        RestartMenu = GameObject.Find("TouchPad").transform.Find("RestartMenu").gameObject;
        //Height = -33f;
        gameMusic = GameObject.Find("GameMusic");
        holder = FindObjectOfType<Holder>();
        psd = GameObject.Find("DeathParticles").GetComponent<ParticleSystem>();
        SpawnBall();
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "Off" || PlayerPrefs.GetString("Sound") == "Vib")
            {
                gameMusic.SetActive(false);
            }           
        }
        Player.GetComponent<Grapple>().enabled = false;
        coinsText.key = "Coins";           
    }

    void FixedUpdate()
    {
        if (!IsStarted)
        {
            if (holder.IsTapped())
            {
                IsStarted = true;
                Player.GetComponent<Grapple>().enabled = true;
                GameObject.Find("StartMenu").SetActive(false);
                GameObject.Find("TouchPad").transform.Find("Restart").gameObject.SetActive(true);
                coinsText.key = "Coins_Game";
                //GameObject.Find("TouchPad").transform.Find("Dash").gameObject.SetActive(true);
                //GameObject.Find("TouchPad").transform.Find("Jump").gameObject.SetActive(true);
                GameObject.Find("TouchPad").transform.Find("CurrentLevelGUI").gameObject.SetActive(true);
            }
        }
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "Off" || PlayerPrefs.GetString("Sound") == "Vib")
            {
                gameMusic.SetActive(false);
            }
            else
            {
                gameMusic.SetActive(true);
            }
        }
        if (currentPlayer == null) currentPlayer = GameObject.FindWithTag("player");
        if (currentCamera == null) currentCamera = Camera.main.gameObject;
        //if(Player.transform.position.y <= Height)
        //{
        //PlayerPrefs.SetFloat("Coins_Game", 0);
        //EndGame(1f);
        //    StartCoroutine(CoroutineRespawn());
        //}
    }

    public void EndGame(float restartDelay, bool IsMenu = false)
    {
        if (gameHasEnded == false)
        {
            if (IsMenu)
            {
                Restart();
            }
            else
            {
                gameHasEnded = true;
                GameObject.Find("Pause").SetActive(false);
                GameObject.Find("FinishTrigger").SetActive(false);
                psd.transform.position = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, currentPlayer.transform.position.z);
                psd.Play();
                StartCoroutine(CoroutineDeath());
            }
        }
    }

    private void Restart()
    {
        YsoCorp.GameUtils.YCManager.instance.OnGameFinished(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator CoroutineDeath()
    {
        yield return new WaitForSeconds(0.2f);
        Restart();
    }

    private IEnumerator CoroutineRespawn()
    {
        yield return new WaitForSeconds(1f);
        if(checkPoint != Vector3.zero)
        {           
            currentPlayer.transform.position = checkPoint;
            //Camera.main.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            //Camera.main.GetComponent<FollowPlayer>().offset = new Vector3(0f, 1.5f, -5f);
        }
        else
        {
            EndGame(1f);
        }
    }

    public IEnumerator CoroutineRestartMenu()
    {
        if(checkPoint == Vector3.zero)
        {
            EndGame(1f, true);
        }
        else
        {
            RestartMenu.SetActive(true);
            //currentPlayer.GetComponent<MeshRenderer>().enabled = false;
            psd.transform.position = new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, currentPlayer.transform.position.z);
            psd.Play();
            RestartMenu.transform.Find("Ad").GetComponent<Button>().enabled = false;
            RestartMenu.transform.Find("not").GetComponent<Button>().enabled = false;
            RestartMenu.transform.Find("coins").GetComponent<Button>().enabled = false;
            yield return new WaitForSeconds(0.3f);
            IsRestarted = true;
            RestartMenu.transform.Find("Ad").GetComponent<Button>().enabled = true;
            RestartMenu.transform.Find("not").GetComponent<Button>().enabled = true;
            RestartMenu.transform.Find("coins").GetComponent<Button>().enabled = true;
        }
    }

    public void RespawnPlayer()
    {
        if (checkPoint != Vector3.zero)
        {
            if (currentPlayer == Player)
            {
                Destroy(currentPlayer);
                SpawnBall();
                Player.transform.position = checkPoint;
            }
            else
            {
                Destroy(currentPlayer);
                Destroy(currentCamera);
                SpawnHuman();
                Instantiate(cameraHuman);
                human.transform.position = checkPoint;
            }
        }
    }

    public GameObject[] GetHints()
    {
        return HookUpHint;
    }

    public void SetCheckPoint(Vector3 pos)
    {
        checkPoint = pos;
    }

    public void ChangeIsRestarted()
    {
        IsRestarted = !IsRestarted;
    }

    public bool GetIsRestarted()
    {
        return IsRestarted;
    }

    public GameObject GetHumanOrBall(bool IsBall)
    {
        if (IsBall)
        {
            return Player;
        }
        else
        {
            return human;
        }
    }

    public void SetCurrentCameraAndPlayer(GameObject cam, GameObject pl)
    {
        currentCamera = cam;
        currentPlayer = pl;
    }

    public GameObject GetCurrentCamera()
    {
        return currentCamera;
    }

    public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public void ChangeGrounds(int n)
    {
        groundCount += n;
    }

    public int GetGrounds()
    {
        return groundCount;
    }

    public void SpawnBall()
    {
        if (PlayerPrefs.HasKey("Skin"))
        {
            foreach (GameObject Skin in Skins)
            {
                if (PlayerPrefs.GetString("Skin") + "Ball" == Skin.name)
                {
                    Player = Instantiate(Skin);
                    Player.transform.position = new Vector3(-0.01f, 4.52f, -34.91f);
                    Player.tag = "player";
                }
            }
        }
        else
        {
            Player = Instantiate(Skins[0]);
            Player.transform.position = new Vector3(-0.01f, 4.52f, -34.91f);
            Player.tag = "player";
        }
    }
    public void SpawnHuman()
    {
        if (PlayerPrefs.HasKey("Skin"))
        {
            foreach (GameObject hum in people)
            {
                if (PlayerPrefs.GetString("Skin") == hum.name)
                {
                    human = Instantiate(hum);
                    //Player.transform.position = new Vector3(-0.01f, 4.52f, -340.36123f);
                    human.tag = "player";
                }
            }
        }
        else
        {
            human = Instantiate(people[0]);
            //Player.transform.position = new Vector3(-0.01f, 4.52f, -340.36123f);
            human.tag = "player";
        }
    }
}
