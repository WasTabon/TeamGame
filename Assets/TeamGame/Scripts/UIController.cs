using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private TextMeshProUGUI _seasonDayText;
    
    [SerializeField] private TextMeshProUGUI _skillText;
    [SerializeField] private TextMeshProUGUI _ratingPanelText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    
    [SerializeField] private GameObject _rewardPanel;
    [SerializeField] private GameObject _matchPanel;
    
    [SerializeField] private CanvasGroup _canvasGroup;
    
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _ratingText;
    [SerializeField] private RectTransform _firstElement;
    [SerializeField] private RectTransform _secondElement;
    [SerializeField] private RectTransform _thirdElement;
    [SerializeField] private TextMeshProUGUI _countdownText;

    [Header("Audio")]
    [SerializeField] private AudioClip appearSound;
    [SerializeField] private AudioClip countdownSound;

    [SerializeField] private Canvas _canvas;

    private int _seasonDay;

    private void Awake()
    {
        Instance = this;
        _seasonDay = PlayerPrefs.GetInt("seasonDay", 1);
    }

    private void Start()
    {
        // пофіксити continue після завершення матчу бо не всі панелі правильно закриваються
    }

    private void Update()
    {
        _moneyText.text = $"{WalletController.Instance.money}$";
        _ratingText.text = $"{WalletController.Instance.rating}";
        _seasonDayText.text = _seasonDay.ToString();
    }

    public void FinishMatch()
    {
        List<string> result = new List<string>();
        result = MatchResultSystem.Instance.GenerateRandomMatchResult();

        _skillText.text = $"+{result[0]} Skill Points";
        _ratingPanelText.text = $"+{result[1]} Rating";
        _descriptionText.text = result[2];
        _seasonDay++;
        PlayerPrefs.SetInt("seasonDay", _seasonDay);
        PlayerPrefs.Save();
        
        _rewardPanel.SetActive(true);
        _canvasGroup.DOFade(1f, 3f);
    }

    public void PlayIntroSequence()
    {
        // Получаем размер экрана в пикселях с учётом Canvas Scale
        float screenWidth = _canvas.pixelRect.width;

        // Сброс позиций и прозрачности — за экран
        ResetElement(_firstElement, new Vector2(-screenWidth * 1.1f, 0));
        ResetElement(_secondElement, new Vector2(screenWidth * 1.1f, 0));
        ResetElement(_thirdElement, Vector2.zero, 0);
        _thirdElement.localScale = Vector3.zero;
        _countdownText.text = "";
        _countdownText.alpha = 0;

        Sequence sequence = DOTween.Sequence();

        // Первый элемент (слева)
        sequence.Append(_firstElement.DOAnchorPos(Vector2.zero, 0.35f).SetEase(Ease.OutExpo));
        sequence.Join(_firstElement.GetComponent<CanvasGroup>().DOFade(1, 0.25f));
        sequence.AppendCallback(() => MusicController.Instance.PlaySpecificSound(appearSound));

        // Второй элемент (справа)
        sequence.AppendInterval(0.2f);
        sequence.Append(_secondElement.DOAnchorPos(Vector2.zero, 0.35f).SetEase(Ease.OutExpo));
        sequence.Join(_secondElement.GetComponent<CanvasGroup>().DOFade(1, 0.25f));
        sequence.AppendCallback(() => MusicController.Instance.PlaySpecificSound(appearSound));

        // Третий элемент (масштаб)
        sequence.AppendInterval(0.2f);
        sequence.Append(_thirdElement.DOScale(1, 0.35f).SetEase(Ease.OutBack));
        sequence.Join(_thirdElement.GetComponent<CanvasGroup>().DOFade(1, 0.25f));
        sequence.AppendCallback(() => MusicController.Instance.PlaySpecificSound(appearSound));

        // Обратный отсчёт
        sequence.AppendInterval(0.2f);
        sequence.AppendCallback(() => StartCoroutine(PlayCountdown()));
    }

    private void ResetElement(RectTransform rect, Vector2 anchoredPos, float alpha = 0)
    {
        rect.anchoredPosition = anchoredPos;
        rect.localScale = Vector3.one;
        var cg = rect.GetComponent<CanvasGroup>();
        if (cg != null)
            cg.alpha = alpha;
    }

    private System.Collections.IEnumerator PlayCountdown()
    {
        _countdownText.alpha = 1;
        for (int i = 3; i >= 0; i--)
        {
            _countdownText.text = i.ToString();
            _countdownText.transform.localScale = Vector3.zero;
            _countdownText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
            MusicController.Instance.PlaySpecificSound(countdownSound);
            yield return new WaitForSeconds(1f);
        }

        OnCountdownComplete();
    }

    private void OnCountdownComplete()
    {
        _canvasGroup.DOFade(0f, 1f)
            .OnComplete((() =>
            {
                GameManager.Instance.StartGame();
                _matchPanel.gameObject.SetActive(false);
            }));
    }
}
