using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _ratingText;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _moneyText.text = $"{WalletController.Instance.money}$";
        _ratingText.text = $"{WalletController.Instance.rating}";
    }
}
