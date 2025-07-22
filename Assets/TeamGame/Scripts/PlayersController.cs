using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType
{
    Speed,
    Stamina,
    Attack,
    Defense
}

public class PlayersController : MonoBehaviour
{
    public static PlayersController Instance;
    
    [SerializeField] private GameObject _playerPanel;
    [SerializeField] private GameObject _upgradePanel;

    [SerializeField] private TextMeshProUGUI _upgradeSkillText;

    [Header("Icon")] 
    [SerializeField] private Image _playerIcon;
    
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private TextMeshProUGUI _playerSpeedText;
    [SerializeField] private TextMeshProUGUI _playerStaminaText;
    [SerializeField] private TextMeshProUGUI _playerAttackText;
    [SerializeField] private TextMeshProUGUI _playerDefenseText;

    [Header("Fill Bars")]
    [SerializeField] private Image _playerSpeedFill;
    [SerializeField] private Image _playerStaminaFill;
    [SerializeField] private Image _playerAttackFill;
    [SerializeField] private Image _playerDefenseFill;
    
    private PlayerStats _playerStats;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenPlayerPanel(PlayerStats playerStats)
    {
        _playerStats = playerStats;

        _playerPanel.SetActive(true);
        
        _playerNameText.text = playerStats.characterName;

        _playerIcon.sprite = playerStats.icon;
        
        _playerSpeedText.text = $"{playerStats.speed}/10";
        _playerStaminaText.text = $"{playerStats.stamina}/10";
        _playerAttackText.text = $"{playerStats.attack}/10";
        _playerDefenseText.text = $"{playerStats.defense}/10";
        
        _playerSpeedFill.fillAmount = playerStats.speed / 10f;
        _playerStaminaFill.fillAmount = playerStats.stamina / 10f;
        _playerAttackFill.fillAmount = playerStats.attack / 10f;
        _playerDefenseFill.fillAmount = playerStats.defense / 10f;
    }

    public void UpgradeSpeed()
    {
        
    }
    public void UpgradeStamina()
    {
        
    }
    public void UpgradeAttack()
    {
        
    }
    public void UpgradeDefense()
    {
        
    }
}
