using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PowerLines.Scripts;

public class ScrollViewSnapController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private int totalItems = 5;
    [SerializeField] private float scrollDuration = 0.3f;
    [SerializeField] private float scrollDurationAnimated = 0.1f;
    [SerializeField] private AudioClip _scrollSound;
    
    private int visibleIndex = 0;
    private float stepSize;
    private Tween scrollTween;

    private bool _isWin;

    private void Start()
    {
        stepSize = 1f / (totalItems - 1);
        visibleIndex = Mathf.Clamp(visibleIndex, 0, totalItems - 1);
        
        RectTransform activeItem = content.GetChild(0) as RectTransform;
        WalletController.Instance.currentItem = activeItem;
    }

    public void ScrollLeft()
    {
        if (visibleIndex > 0)
        {
            visibleIndex--;
            SnapToIndex(visibleIndex);
            MusicController.Instance.PlaySpecificSound(_scrollSound);
        }
    }

    public void ScrollRight()
    {
        if (visibleIndex < totalItems - 1)
        {
            visibleIndex++;
            SnapToIndex(visibleIndex);
            MusicController.Instance.PlaySpecificSound(_scrollSound);
        }
    }

    public void SetNextItem()
    {
        RectTransform activeItem = content.GetChild(0) as RectTransform;
        WalletController.Instance.currentItem = activeItem;
    }

    private void SnapToIndex(int index, bool instant = false)
    {
        float targetPosition = stepSize * index;

        if (scrollTween != null && scrollTween.IsActive())
            scrollTween.Kill();

        if (instant)
        {
            scrollRect.horizontalNormalizedPosition = targetPosition;
        }
        else
        {
            scrollTween = DOTween.To(
                () => scrollRect.horizontalNormalizedPosition,
                x => scrollRect.horizontalNormalizedPosition = x,
                targetPosition,
                scrollDuration
            ).SetEase(Ease.OutCubic);
        }
        
        if (content != null && content.childCount > index)
        {
            RectTransform activeItem = content.GetChild(index) as RectTransform;
            WalletController.Instance.currentItem = activeItem;
        }
    }
    
    
    private System.Collections.IEnumerator ScrollToIndexAnimated(int targetIndex)
    {
        for (int i = 0; i <= targetIndex; i++)
        {
            SnapToIndex(i);
            yield return new WaitForSeconds(scrollDurationAnimated * 0.75f);
        }
    }
}