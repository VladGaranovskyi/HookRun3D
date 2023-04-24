using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using GoogleMobileAds.Api;

public class Skin : MonoBehaviour
{
    private string previousSkin = "Not Selected";
    private GameObject currentSkin;
    private string currentSkinName;
    private Image currentImageSkin;
    private GameObject previousObjSkin;
    private Image previousImageSkin;
    public Color StartColor;
    //private RewardedAd rewarded;
    //private const string TestRewId = "ca-app-pub-3940256099942544/5224354917";//ca-app-pub-2507976896790061/3769042472

    private Skin.DataPlayer dataPlayer = new Skin.DataPlayer();
    [SerializeField] private Text priceText;
    [SerializeField] private float startPrice;
    [HideInInspector]
    public string nameItem;
    private float price;

    [SerializeField] private Transform posHuman;
    [SerializeField] private Transform[] people;
    private Transform _currentHuman;
   
    [SerializeField] private GameObject[] allItem;

    public class DataPlayer
    {
        public List<string> buyItem = new List<string>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("SaveGame"))
        {
            LoadGame();
        }
        else
        {
            SaveGame();
            LoadGame();
        }
    }

    private void FixedUpdate()
    {
        if (PlayerPrefs.HasKey("Skin"))
        {
            currentSkinName = PlayerPrefs.GetString("Skin");
            if(_currentHuman == null)
            {
                foreach(Transform h in people)
                {
                    if (h.name == currentSkinName + "1") 
                        h.position = posHuman.position;_currentHuman = h; break;
                }
            }
            else if(_currentHuman.name != currentSkinName + "1")
            {
                foreach (Transform h in people)
                {
                    if (h.name == currentSkinName + "1")
                    {
                        h.position = posHuman.position; 
                        _currentHuman.position = Vector3.zero; 
                        _currentHuman = h; break;
                    }
                }
            }
        }
        else
        {
            people[0].position = posHuman.position; _currentHuman = people[0];
        }
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString("SaveGame", JsonUtility.ToJson(dataPlayer));
    }

    private void LoadGame()
    {
        dataPlayer = JsonUtility.FromJson<DataPlayer>(PlayerPrefs.GetString("SaveGame"));
        price = startPrice + dataPlayer.buyItem.Count * 100f;
        priceText.text = price.ToString();
        for (int i = 0; i < dataPlayer.buyItem.Count; i++)
        {
            for(int y = 0; y < allItem.Length; y++)
            {
                if(allItem[y].GetComponent<Item>().nameItem == dataPlayer.buyItem[i])
                {
                    //allItem[y].GetComponent<Item>().textItem.text = "Unlocked";
                    Item item = allItem[y].GetComponent<Item>();
                    item.isBought = true;
                    item.theSkin.GetComponent<Button>().enabled = true;
                    allItem[y].SetActive(false);
                }
            }
        }
        if (PlayerPrefs.HasKey("Skin"))
        {
            previousSkin = PlayerPrefs.GetString("Skin");
            previousObjSkin = GameObject.Find(previousSkin);
            previousImageSkin = previousObjSkin.GetComponent<Image>();
            previousImageSkin.color = Color.blue;
        }
    }

    public void BuyItem(bool IsAd)
    {
        if (IsAd)
        {
            //ShowRewAd();
        }
        else
        {
            if (PlayerPrefs.HasKey("Coins"))
            {
                if (PlayerPrefs.GetFloat("Coins") >= price)
                {
                    dataPlayer.buyItem.Add(nameItem);
                    float removemoney = PlayerPrefs.GetFloat("Coins") - price;
                    PlayerPrefs.SetFloat("Coins", removemoney);
                    SaveGame();
                    LoadGame();
                }
            }
        }
    }

    /*private void ShowRewAd()
    {
        rewarded = new RewardedAd(TestRewId);
        rewarded.OnUserEarnedReward += HandleUserEarnedReward;
        rewarded.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        rewarded.OnAdLoaded += HandleOnAdLoaded;
        AdRequest request = new AdRequest.Builder().Build();
        rewarded.LoadAd(request);
    }*/

    private void HandleUserEarnedReward(object sender, EventArgs args)
    {
        dataPlayer.buyItem.Add(nameItem);
        SaveGame();
        LoadGame();
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

    public void SetSkin(string name)
    {
        if(previousSkin != name)
        {
            PlayerPrefs.SetString("Skin", name);           
            currentSkin = GameObject.Find(name);
            currentImageSkin = currentSkin.GetComponent<Image>();
            currentImageSkin.color = Color.blue;
            if (previousSkin == "Not Selected")
            {
                previousSkin = name;
                previousObjSkin = currentSkin;
                previousImageSkin = currentImageSkin;
            }
            else
            {
                previousImageSkin.color = StartColor;
            }
            previousSkin = name;
            previousObjSkin = currentSkin;
            previousImageSkin = currentImageSkin;
            PlayerPrefs.Save();
        }      
    }
}
