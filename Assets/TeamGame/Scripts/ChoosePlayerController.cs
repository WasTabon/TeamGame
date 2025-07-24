using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosePlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _choosePlayerPanel;
    
    [SerializeField] private TextMeshProUGUI _text1;
    [SerializeField] private TextMeshProUGUI _text2;
    [SerializeField] private TextMeshProUGUI _text3;

    [SerializeField] private Image _image1;
    [SerializeField] private Image _image2;
    [SerializeField] private Image _image3;

    private bool _choose1;
    private bool _choose2;
    private bool _choose3;
    
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
        }
        else if (_choose2)
        {
            _text2.text = playerStats.characterName;
            _image2.sprite = playerStats.icon;
        }
        else if (_choose3)
        {
            _text3.text = playerStats.characterName;
            _image3.sprite = playerStats.icon;
        }
        
        _choosePlayerPanel.SetActive(false);
    }

    public void StartGame()
    {
        _text1.text = $"Choose Player";
        _text2.text = $"Choose Player";
        _text3.text = $"Choose Player";

        _image1.sprite = null;
        _image2.sprite = null;
        _image3.sprite = null;
    }
}
