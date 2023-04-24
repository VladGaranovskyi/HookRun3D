using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
//using GoogleMobileAds.Api;

public class Pause : MonoBehaviour
{
    private Rigidbody rb;
    private bool IsDashed;
    private bool IsJumped;
    //private const string TestRewId = "ca-app-pub-3940256099942544/5224354917";//ca-app-pub-2507976896790061/3769042472
    //private RewardedAd rewarded;
    private MeshRenderer playerMesh;
    private GameObject _player;
    private GameObject _cam;
    private GameManager _gm;
    [SerializeField] private Button runButton;
    [SerializeField] private Text runK;
    [SerializeField] private float startPriceRun;
    [SerializeField] private Button forceButton;
    [SerializeField] private Text forceK;
    [SerializeField] private float startPriceForce;
    private float _priceRun;
    private float _priceForce;

    private void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        if (PlayerPrefs.HasKey("RunK"))
        {
            if(PlayerPrefs.GetInt("RunK") >= 10)
            {
                runK.text = "";
                runButton.enabled = false;
            }
            else
            {
                _priceRun = startPriceRun * (PlayerPrefs.GetInt("RunK") + 1);
                runK.text = _priceRun.ToString();
            }
        }
        else
        {
            _priceRun = startPriceRun;
        }
        if (PlayerPrefs.HasKey("ForceK"))
        {
            if (PlayerPrefs.GetInt("ForceK") >= 10)
            {
                forceK.text = "";
                forceButton.enabled = false;
            }
            else
            {
                _priceForce = startPriceForce * (PlayerPrefs.GetInt("ForceK") + 1);
                forceK.text = _priceForce.ToString();
            }
        }
        else
        {
            _priceForce = startPriceForce;
        }
    }

    public void PauseGame(GameObject menu)
    {
        menu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame(GameObject menu)
    {
        menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame(GameObject menu)
    {
        menu.SetActive(false);
        Time.timeScale = 1;
        PlayerPrefs.SetFloat("Coins_Game", 0);
        FindObjectOfType<GameManager>().EndGame(0f, true);
    }

    public void ChangeSceneFromPause(string scene)
    {
        Time.timeScale = 1;
        PlayerPrefs.SetFloat("Coins_Game", 0);
        SceneManager.LoadScene(scene);
    }

    public void BuySpeed(string name)
    {
        if (PlayerPrefs.GetFloat("Coins") >= (name == "RunK" ? _priceRun : _priceForce))
        {
            if (PlayerPrefs.HasKey(name))
            {
                PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
            }
            else
            {
                PlayerPrefs.SetInt(name, 1);
            }
            PlayerPrefs.SetFloat("Coins", PlayerPrefs.GetFloat("Coins") - (name == "RunK" ? _priceRun : _priceForce));
            if (PlayerPrefs.GetInt(name) >= 10)
            {
                if (name == "RunK")
                {
                    runK.text = "";
                    runButton.enabled = false;
                }
                else
                {
                    forceK.text = "";
                    forceButton.enabled = false;
                }
            }
            else
            {
                if(name == "RunK")
                {
                    _priceRun = startPriceRun * (PlayerPrefs.GetInt(name) + 1);
                    runK.text = _priceRun.ToString();
                }
                else
                {
                    _priceForce = startPriceForce * (PlayerPrefs.GetInt(name) + 1);
                    forceK.text = _priceForce.ToString();
                }
            }
        }
    }

    public void ContinueGame(bool IsAd)
    {
        if (IsAd)
        {
            /*rewarded = new RewardedAd(TestRewId);
            rewarded.OnUserEarnedReward += HandleUserEarnedReward;
            rewarded.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            rewarded.OnAdLoaded += HandleOnAdLoaded;
            AdRequest request = new AdRequest.Builder().Build();
            rewarded.LoadAd(request);*/
        }
        else
        {
            if(PlayerPrefs.GetFloat("Coins") >= 200f)
            {
                PlayerPrefs.SetFloat("Coins", PlayerPrefs.GetFloat("Coins") - 200f);
                Resp();
            }
        }
    }

    private void HandleUserEarnedReward(object sender, EventArgs args)
    {
        Resp();
    }

    /*private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        if (rewarded.IsLoaded())
        {
            rewarded.Show();
        }
    }*/

    private void HandleOnAdFailedToLoad(object sender, EventArgs args)
    {
        Debug.Log("ad failed to load" + args);
    }

    private void FixedUpdate()
    {
        if (_cam == null) _cam = Camera.main.gameObject;
        if (_player == null) _player = GameObject.FindWithTag("player");
        if (rb == null)
        {
            rb = _player.GetComponent<Rigidbody>();
        }
        //if (playerMesh == null)
        //{
        //    playerMesh = _player.GetComponent<MeshRenderer>();
        //}
    }

    /*private IEnumerator CorResp()
    {
        float i = 0f;
        int j = 1;
        while(i <= 2f)
        {           
            playerMesh.enabled = (j % 2 == 0);
            i += 0.2f;
            j++;
            yield return new WaitForSeconds(0.2f);
        }
        _gm.ChangeIsRestarted();
        playerMesh.enabled = true;
    }*/
    private void Resp()
    {
        GameObject.Find("TouchPad").transform.Find("RestartMenu").gameObject.SetActive(false);
        //Time.timeScale = 1f;
        _gm.RespawnPlayer();
        //playerMesh.enabled = true;
        //StartCoroutine(CorResp());
    }
}
