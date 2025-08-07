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
    [SerializeField] private GameObject _haveNoSkillPointsPanel;

    [SerializeField] private TextMeshProUGUI _upgradeSkillText;
    [SerializeField] private TextMeshProUGUI _skillPointsText;

    [Header("Sounds")] 
    [SerializeField] private AudioClip _upgradeSkillSound;
    [SerializeField] private AudioClip _haveNoSkillPointsSound;
    
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
    private UpgradeType _currentSkill;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _skillPointsText.text = $"You have {WalletController.Instance.skillPoints} skill points";
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

    public void UpgradeSkill()
    {
        if (WalletController.Instance.skillPoints >= 5)
        {
            WalletController.Instance.skillPoints -= 5;
            if (_currentSkill == UpgradeType.Speed)
            {
                _playerStats.speed++;
                _playerSpeedFill.fillAmount = _playerStats.speed / 10f;
                _playerStats.SaveToFile();
            }
            else if (_currentSkill == UpgradeType.Stamina)
            {
                _playerStats.stamina++;
                _playerStaminaFill.fillAmount = _playerStats.stamina / 10f;
                _playerStats.SaveToFile();
            }
            else if (_currentSkill == UpgradeType.Attack)
            {
                _playerStats.attack++;
                _playerAttackFill.fillAmount = _playerStats.attack / 10f;
                _playerStats.SaveToFile();
            }
            else if (_currentSkill == UpgradeType.Defense)
            {
                _playerStats.defense++;
                _playerDefenseFill.fillAmount = _playerStats.defense / 10f;
                _playerStats.SaveToFile();
            }
            MusicController.Instance.PlaySpecificSound(_upgradeSkillSound);
            _playerSpeedText.text = $"{_playerStats.speed}/10";
            _playerStaminaText.text = $"{_playerStats.stamina}/10";
            _playerAttackText.text = $"{_playerStats.attack}/10";
            _playerDefenseText.text = $"{_playerStats.defense}/10";
        }
        else
        {
            MusicController.Instance.PlaySpecificSound(_haveNoSkillPointsSound);
            _haveNoSkillPointsPanel.SetActive(true);
        }
    }
    
    public void UpgradeSpeed()
    {
        _currentSkill = UpgradeType.Speed;
        OpenUpgradePanel(UpgradeType.Speed);
    }
    public void UpgradeStamina()
    {
        _currentSkill = UpgradeType.Stamina;
        OpenUpgradePanel(UpgradeType.Stamina);
    }
    public void UpgradeAttack()
    {
        _currentSkill = UpgradeType.Attack;
        OpenUpgradePanel(UpgradeType.Attack);
    }
    public void UpgradeDefense()
    {
        _currentSkill = UpgradeType.Defense;
        OpenUpgradePanel(UpgradeType.Defense);
    }

    private void OpenUpgradePanel(UpgradeType upgradeType)
    {
        string stat = "";
        
        switch (upgradeType)
        {
            case UpgradeType.Speed:
                stat = "SPEED";
                break;
            case UpgradeType.Stamina:
                stat = "STAMINA";
                break;
            case UpgradeType.Attack:
                stat = "ATTACK";
                break;
            case UpgradeType.Defense:
                stat = "DEFENSE";
                break;
        }
        
        _upgradePanel.SetActive(true);
        _upgradeSkillText.text = $"YOU SURE YOU WANT TO UPGRADE {stat}?";
        _skillPointsText.text = $"You have {WalletController.Instance.skillPoints} skill points";
    }
}
