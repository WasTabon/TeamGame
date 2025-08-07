using UnityEditor;
using UnityEngine;
using TMPro;

public class RatingAssignerEditor : Editor
{
    [MenuItem("Tools/Set Rating Text Incremental")]
    public static void SetRatingTextIncremental()
    {
        var selectedObjects = Selection.gameObjects;

        if (selectedObjects == null || selectedObjects.Length == 0)
        {
            Debug.LogError("No GameObjects selected.");
            return;
        }

        int rating = 50;
        int step = 150;

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            GameObject obj = selectedObjects[i];
            if (obj == null) continue;

            Transform ratingTransform = obj.transform.Find("RatingText");
            if (ratingTransform == null)
            {
                Debug.LogWarning($"RatingText not found in '{obj.name}'. Skipping.");
                continue;
            }

            TMP_Text tmp = ratingTransform.GetComponent<TMP_Text>();
            if (tmp == null)
            {
                Debug.LogWarning($"TMP_Text missing on '{obj.name}/RatingText'. Skipping.");
                continue;
            }

            tmp.text = rating.ToString();
            rating += step;
        }

        Debug.Log("Incremental RatingText assigned.");
    }
}