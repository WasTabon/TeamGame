using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class WalletController : MonoBehaviour
{
    public static WalletController Instance;

    [SerializeField] private ScrollViewSnapController _scrollViewSnapController;
    [SerializeField] private RectTransform _items;

    public int skillPoints;
    public int money;

    private const int ItemPrice = 500;
    private HashSet<string> purchasedItems = new HashSet<string>();

    public RectTransform currentItem;

    private void Awake()
    {
        Instance = this;
        
        LoadPurchasedItems();
    }

    public void BuyItem()
    {
        string itemId = currentItem.name;

        if (purchasedItems.Contains(itemId))
        {
            Debug.Log($"Item '{itemId}' already purchased.");
            return;
        }

        if (money < ItemPrice)
        {
            Debug.Log("Not enough money!");
            return;
        }

        money -= ItemPrice;
        purchasedItems.Add(itemId);
        SavePurchasedItems();

        AnimateAndRemove(currentItem);
        
        _scrollViewSnapController.SetNextItem();
    }

    private void AnimateAndRemove(RectTransform itemRect)
    {
        itemRect.DOScale(Vector3.zero, 0.4f)
            .SetEase(Ease.InBack)
            .OnComplete(() => itemRect.gameObject.SetActive(false));
    }

    private void SavePurchasedItems()
    {
        string savedData = string.Join(",", purchasedItems);
        PlayerPrefs.SetString("PurchasedItems", savedData);
        PlayerPrefs.SetInt("PlayerMoney", money);
        PlayerPrefs.Save();
    }

    private void LoadPurchasedItems()
    {
        purchasedItems.Clear();

        string savedData = PlayerPrefs.GetString("PurchasedItems", "");
        money = PlayerPrefs.GetInt("PlayerMoney", money);

        if (!string.IsNullOrEmpty(savedData))
        {
            var items = savedData.Split(',');
            foreach (var item in items)
            {
                purchasedItems.Add(item);
            }
        }
        foreach (Transform child in _items) 
        {
            if (purchasedItems.Contains(child.name))
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
