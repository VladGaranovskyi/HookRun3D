using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Skin scriptShop;
    [SerializeField] private bool IsAd;
    public string nameItem;
    public bool isBought;
    public GameObject theSkin;

    public void BuyItem()
    {
        if(isBought == false)
        {
            scriptShop.nameItem = nameItem;
            scriptShop.BuyItem(IsAd);
        }
    }
}
