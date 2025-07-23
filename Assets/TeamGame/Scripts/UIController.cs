using System;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private TextMeshProUGUI _moneyText;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _moneyText.text = $"{WalletController.Instance.money}$";
    }
}
