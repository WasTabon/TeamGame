using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridCellWidthFitter : MonoBehaviour
{
    private RectTransform _viewportRect;
    private GridLayoutGroup _gridLayout;
    private RectTransform _rectTransform;

    private void Start()
    {
        StartCoroutine(DelayedSetup());
    }

    private IEnumerator DelayedSetup()
    {
        yield return null; // Ждём один кадр, чтобы размеры Viewport успели установиться

        _gridLayout = GetComponent<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();

        // Пытаемся найти Viewport через ScrollRect
        _viewportRect = GetComponentInParent<ScrollRect>()?.viewport;
        if (_viewportRect == null)
        {
            Debug.LogWarning("Viewport не найден! Убедитесь, что объект находится внутри ScrollRect.");
            yield break;
        }

        UpdateCellWidth();
    }

    private void UpdateCellWidth()
    {
        float paddingHorizontal = _gridLayout.padding.left + _gridLayout.padding.right;
        float spacingX = _gridLayout.spacing.x;

        int constraintCount = _gridLayout.constraintCount;
        if (constraintCount <= 0)
        {
            Debug.LogWarning("Constraint count должен быть больше 0!");
            constraintCount = 1; // подстраховка
        }

        // Общая ширина, доступная под ячейки
        float totalWidth = _viewportRect.rect.width - paddingHorizontal - spacingX * (constraintCount - 1);
        float cellWidth = totalWidth / constraintCount;

        _gridLayout.cellSize = new Vector2(cellWidth, _gridLayout.cellSize.y);
    }
}