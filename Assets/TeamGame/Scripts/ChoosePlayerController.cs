using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayerController : MonoBehaviour
{
    [Header("UI: Общая информация")]
    [SerializeField] private TextMeshProUGUI _matchScoreText;
    
    [Header("UI: Характеристики Врага")]
    [SerializeField] private TextMeshProUGUI _enemyAttackText1;
    [SerializeField] private TextMeshProUGUI _enemyDefenseText1;
    [SerializeField] private TextMeshProUGUI _enemyStaminaText1;
    [SerializeField] private TextMeshProUGUI _enemySpeedText1;

    [Header("UI: Характеристики Игрока")]
    [SerializeField] private TextMeshProUGUI _attackText;
    [SerializeField] private TextMeshProUGUI _defenseText;
    [SerializeField] private TextMeshProUGUI _staminaText;
    [SerializeField] private TextMeshProUGUI _speedText;

    [Header("UI: Характеристики Врага")]
    [SerializeField] private TextMeshProUGUI _enemyAttackText;
    [SerializeField] private TextMeshProUGUI _enemyDefenseText;
    [SerializeField] private TextMeshProUGUI _enemyStaminaText;
    [SerializeField] private TextMeshProUGUI _enemySpeedText;

    [Header("UI: Панель выбора")]
    [SerializeField] private GameObject _choosePlayerPanel;

    [Header("UI: Выбранные игроки")]
    [SerializeField] private TextMeshProUGUI _text1;
    [SerializeField] private TextMeshProUGUI _text2;
    [SerializeField] private TextMeshProUGUI _text3;

    [SerializeField] private Image _image1;
    [SerializeField] private Image _image2;
    [SerializeField] private Image _image3;

    int enemySpeed;
    int enemyStamina;
    int enemyAttack;
    int enemyDefense;
    
    private bool _choose1;
    private bool _choose2;
    private bool _choose3;

    private bool _wasGenerated;

    private PlayerStats _selectedPlayer1;
    private PlayerStats _selectedPlayer2;
    private PlayerStats _selectedPlayer3;

    public void ChoosePlayer1()
    {
        _choosePlayerPanel.SetActive(true);
        _choose1 = true;
        _choose2 = false;
        _choose3 = false;
    }

    public void ChoosePlayer2()
    {
        _choosePlayerPanel.SetActive(true);
        _choose2 = true;
        _choose3 = false;
        _choose1 = false;
    }

    public void ChoosePlayer3()
    {
        _choosePlayerPanel.SetActive(true);
        _choose3 = true;
        _choose2 = false;
        _choose1 = false;
    }

    public void ChoosePlayer(PlayerStats playerStats)
    {
        if (_choose1)
        {
            _text1.text = playerStats.characterName;
            _image1.sprite = playerStats.icon;
            _selectedPlayer1 = playerStats;
        }
        else if (_choose2)
        {
            _text2.text = playerStats.characterName;
            _image2.sprite = playerStats.icon;
            _selectedPlayer2 = playerStats;
        }
        else if (_choose3)
        {
            _text3.text = playerStats.characterName;
            _image3.sprite = playerStats.icon;
            _selectedPlayer3 = playerStats;
        }

        _choosePlayerPanel.SetActive(false);
    }

    public void GenerateStats()
    {
        if (!_wasGenerated)
        {
            enemySpeed = Random.Range(10, 31);
            enemyStamina = Random.Range(10, 31);
            enemyAttack = Random.Range(10, 31);
            enemyDefense = Random.Range(10, 31);

            _enemyAttackText1.text = enemyAttack.ToString();
            _enemySpeedText1.text = enemySpeed.ToString();
            _enemyStaminaText1.text = enemyStamina.ToString();
            _enemyDefenseText1.text = enemyDefense.ToString();
        
            _wasGenerated = true;
        }
    }
    
    public void StartGame()
    {
        _matchScoreText.text = "0 - 0";

        _text1.text = "Choose Player";
        _text2.text = "Choose Player";
        _text3.text = "Choose Player";

        _image1.sprite = null;
        _image2.sprite = null;
        _image3.sprite = null;
        
        CalculateTotalStats(out int totalSpeed, out int totalStamina, out int totalAttack, out int totalDefense);
        _attackText.text = totalAttack.ToString();
        _speedText.text = totalSpeed.ToString();
        _staminaText.text = totalStamina.ToString();
        _defenseText.text = totalDefense.ToString();
        
        _selectedPlayer1 = null;
        _selectedPlayer2 = null;
        _selectedPlayer3 = null;
        
        if (!_wasGenerated)
        {
            enemySpeed = Random.Range(10, 31);
            enemyStamina = Random.Range(10, 31);
            enemyAttack = Random.Range(10, 31);
            enemyDefense = Random.Range(10, 31);
        }

        _enemyAttackText.text = enemyAttack.ToString();
        _enemySpeedText.text = enemySpeed.ToString();
        _enemyStaminaText.text = enemyStamina.ToString();
        _enemyDefenseText.text = enemyDefense.ToString();

        _wasGenerated = false;
    }

    public void CalculateTotalStats(out int totalSpeed, out int totalStamina, out int totalAttack, out int totalDefense)
    {
        totalSpeed = 0;
        totalStamina = 0;
        totalAttack = 0;
        totalDefense = 0;

        if (_selectedPlayer1 != null)
        {
            totalSpeed += _selectedPlayer1.speed;
            totalStamina += _selectedPlayer1.stamina;
            totalAttack += _selectedPlayer1.attack;
            totalDefense += _selectedPlayer1.defense;
        }

        if (_selectedPlayer2 != null)
        {
            totalSpeed += _selectedPlayer2.speed;
            totalStamina += _selectedPlayer2.stamina;
            totalAttack += _selectedPlayer2.attack;
            totalDefense += _selectedPlayer2.defense;
        }

        if (_selectedPlayer3 != null)
        {
            totalSpeed += _selectedPlayer3.speed;
            totalStamina += _selectedPlayer3.stamina;
            totalAttack += _selectedPlayer3.attack;
            totalDefense += _selectedPlayer3.defense;
        }
    }
}
