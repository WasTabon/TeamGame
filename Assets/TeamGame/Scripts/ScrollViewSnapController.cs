using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using PowerLines.Scripts;

public class ScrollViewSnapController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private float scrollDuration = 0.3f;
    [SerializeField] private float scrollDurationAnimated = 0.1f;
    [SerializeField] private AudioClip _scrollSound;

    private int visibleIndex = 0;
    private float stepSize;
    private Tween scrollTween;

    private void Start()
    {
        UpdateStepSize();

        visibleIndex = Mathf.Clamp(visibleIndex, 0, GetActiveItemCount() - 1);

        RectTransform activeItem = GetActiveChildAt(visibleIndex);
        if (activeItem != null)
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
        if (visibleIndex < GetActiveItemCount() - 1)
        {
            visibleIndex++;
            SnapToIndex(visibleIndex);
            MusicController.Instance.PlaySpecificSound(_scrollSound);
        }
    }

    public void SetNextItem()
    {
        RectTransform activeItem = GetActiveChildAt(visibleIndex);
        if (activeItem != null)
            WalletController.Instance.currentItem = activeItem;
    }

    private void SnapToIndex(int index, bool instant = false)
    {
        UpdateStepSize();
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

        RectTransform activeItem = GetActiveChildAt(index);
        if (activeItem != null)
            WalletController.Instance.currentItem = activeItem;
    }

    private IEnumerator ScrollToIndexAnimated(int targetIndex)
    {
        for (int i = 0; i <= targetIndex; i++)
        {
            SnapToIndex(i);
            yield return new WaitForSeconds(scrollDurationAnimated * 0.75f);
        }
    }

    // 🧩 Получить активный дочерний элемент по индексу
    private RectTransform GetActiveChildAt(int activeIndex)
    {
        int count = 0;
        foreach (Transform child in content)
        {
            if (child.gameObject.activeSelf)
            {
                if (count == activeIndex)
                    return child as RectTransform;
                count++;
            }
        }
        return null;
    }

    // 🧩 Получить количество активных элементов
    private int GetActiveItemCount()
    {
        int count = 0;
        foreach (Transform child in content)
        {
            if (child.gameObject.activeSelf)
                count++;
        }
        return count;
    }

    // 🧩 Обновить размер шага
    private void UpdateStepSize()
    {
        int activeCount = GetActiveItemCount();
        stepSize = activeCount > 1 ? 1f / (activeCount - 1) : 0f;
    }
}
