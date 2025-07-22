using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayersController : MonoBehaviour
{
    [SerializeField] private GameObject _playerPanel;
    
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
    
    public void OpenPlayerPanel(PlayerStats playerStats)
    {
        _playerStats = playerStats;

        _playerPanel.SetActive(true);
        
        _playerNameText.text = playerStats.characterName;
        
        _playerSpeedText.text = $"{playerStats.speed}/10";
        _playerStaminaText.text = $"{playerStats.stamina}/10";
        _playerAttackText.text = $"{playerStats.attack}/10";
        _playerDefenseText.text = $"{playerStats.defense}/10";
        
        _playerSpeedFill.fillAmount = playerStats.speed / 10f;
        _playerStaminaFill.fillAmount = playerStats.stamina / 10f;
        _playerAttackFill.fillAmount = playerStats.attack / 10f;
        _playerDefenseFill.fillAmount = playerStats.defense / 10f;
    }
}
