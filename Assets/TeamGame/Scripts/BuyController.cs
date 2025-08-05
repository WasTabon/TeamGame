using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class BuyController : MonoBehaviour
{
    private string _donateId = "com.coachmax.skillpoints";
    
    public GameObject loadingButton;
    public AudioClip buySound;
    public TextMeshProUGUI buttonText;
    public GameObject panel;
    
    public void OnPurchaseComlete(Product product)
    {
        if (product.definition.id == _donateId)
        {
            Debug.Log("Complete");
            WalletController.Instance.skillPoints += 50;
            PlayerPrefs.SetInt("PlayerMoney", WalletController.Instance.money);
            PlayerPrefs.Save();
            MusicController.Instance.PlaySpecificSound(buySound);
            loadingButton.SetActive(false);
            panel.SetActive(true);
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription description)
    {
        if (product.definition.id == _donateId)
        {
            loadingButton.SetActive(false);
            Debug.Log($"Failed: {description.message}");
        }
    }
    
    public void OnProductFetched(Product product)
    {
        Debug.Log("Fetched");
        buttonText.text = product.metadata.localizedPriceString;
    }
}
