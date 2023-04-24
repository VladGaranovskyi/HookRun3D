using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using GoogleMobileAds.Api;

public class CompletedLevel : MonoBehaviour
{
    //private InterstitialAd interstitial;
    //private RewardedInterstitialAd rewarded;
    //private const string TestInterstitialId = "ca-app-pub-3940256099942544/1033173712";//ca-app-pub-2507976896790061/1451846806
    //private const string TestRewId = "ca-app-pub-3940256099942544/5354046379";//ca-app-pub-2507976896790061/4680645371
    private GameObject CompleteUI;
    public float multiply = 1f;

    //void Start()
    //{
    //    MobileAds.Initialize(initStatus => { });
    //}

    /*private void adLoadCallback(RewardedInterstitialAd ad, EventArgs args)
    {
        if (args == null)
        {
            rewarded = ad;
        }
    }

    private void ShowInterstitialAd()
    {
        interstitial = new InterstitialAd(TestInterstitialId);
        interstitial.OnAdFailedToLoad += HandleOnAdClosed;
        interstitial.OnAdClosed += HandleOnAdClosed;
        interstitial.OnAdLoaded += HandleOnAdLoaded;
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);     
    }

    private void ShowRewAd()
    {
        AdRequest request = new AdRequest.Builder().Build();
        RewardedInterstitialAd.LoadAd(TestRewId, request, adLoadCallback);
        if (rewarded != null)
        {
            rewarded.Show(userEarnedRewardCallback);
        }
    }*/

    public void Leave3x(GameObject Complete)
    {
        CompleteUI = Complete;
        Destroy(GameObject.Find("GameMusic"));
        //ShowRewAd();
    }

    public void Leave(GameObject Complete)
    {
        CompleteUI = Complete;
        Destroy(GameObject.Find("GameMusic"));
        GameObject.Find("Back2Menu").GetComponent<Button>().enabled = false;
        /*System.Random rnd = new System.Random();
        int i = rnd.Next(1, 100);
        if (i >= 1 && i <= 50)
        {
            ShowInterstitialAd();
        }
        else
        {
            PlayerPrefs.SetFloat("Coins", PlayerPrefs.GetFloat("Coins") + PlayerPrefs.GetFloat("Coins_Game") * multiply);
            if (PlayerPrefs.HasKey("pass") == false)
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            }
            else
            {
                PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
                PlayerPrefs.SetString("IsLevelNew", "old");
            }
            if (PlayerPrefs.GetInt("Level") >= 11)
            {
                PlayerPrefs.SetString("pass", "passed 100 levels");
                PlayerPrefs.SetInt("CurrentLevel", 11);
            }
            PlayerPrefs.SetFloat("Coins_Game", 0f);
            CompleteUI.SetActive(false);
            Time.timeScale = 1;
            SceneManager.LoadScene("Menu");
        }*/
        PlayerPrefs.SetFloat("Coins", PlayerPrefs.GetFloat("Coins") + PlayerPrefs.GetFloat("Coins_Game") * multiply);
        if (PlayerPrefs.HasKey("pass") == false)
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
            PlayerPrefs.SetString("IsLevelNew", "old");
        }
        if (PlayerPrefs.GetInt("Level") >= 11)
        {
            PlayerPrefs.SetString("pass", "passed 100 levels");
            PlayerPrefs.SetInt("CurrentLevel", 11);
        }
        PlayerPrefs.SetFloat("Coins_Game", 0f);
        PlayerPrefs.SetInt("newLevel", 1);
        YsoCorp.GameUtils.YCManager.instance.OnGameFinished(true);
        CompleteUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    private void HandleOnAdClosed(object sender, EventArgs args)
    {
        PlayerPrefs.SetFloat("Coins", PlayerPrefs.GetFloat("Coins") + PlayerPrefs.GetFloat("Coins_Game") * multiply);
        if (PlayerPrefs.HasKey("pass") == false)
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
            PlayerPrefs.SetString("IsLevelNew", "old");
        }
        if (PlayerPrefs.GetInt("Level") >= 11)
        {
            PlayerPrefs.SetString("pass", "passed 100 levels");
            PlayerPrefs.SetInt("CurrentLevel", 11);
        }
        PlayerPrefs.SetFloat("Coins_Game", 0f);
        CompleteUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    /*private void HandleOnAdLoaded(object sender, EventArgs args)
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }*/
    
    /*private void userEarnedRewardCallback(Reward reward)
    {
        PlayerPrefs.SetFloat("Coins", PlayerPrefs.GetFloat("Coins") + (PlayerPrefs.GetFloat("Coins_Game") + 100f) * 3 * multiply);
        if (PlayerPrefs.HasKey("pass") == false)
        {
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
            PlayerPrefs.SetString("IsLevelNew", "old");
        }
        if (PlayerPrefs.GetInt("Level") >= 11)
        {
            PlayerPrefs.SetString("pass", "passed 100 levels");
            PlayerPrefs.SetInt("CurrentLevel", 11);
        }
        PlayerPrefs.SetFloat("Coins_Game", 0f);
        CompleteUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }*/
}
