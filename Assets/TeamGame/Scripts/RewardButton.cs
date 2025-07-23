using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour
{
    [SerializeField] private GameObject _rewardPanel;
    
    public int neededRating;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void OnClick()
    {
        if (WalletController.Instance.rating >= neededRating)
        {
            WalletController.Instance.money += 500;
            _rewardPanel.gameObject.SetActive(true);
            _button.gameObject.SetActive(false);
        }
    }
}