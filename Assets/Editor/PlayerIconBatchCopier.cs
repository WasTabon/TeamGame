using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerIconBatchCopier : EditorWindow
{
    private List<GameObject> sourcePrefabs = new List<GameObject>();
    private List<GameObject> targetPrefabs = new List<GameObject>();
    private Vector2 scroll;

    [MenuItem("Tools/Copy PlayerIcon Between Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<PlayerIconBatchCopier>("Copy PlayerIcon Sprite");
    }

    private void OnGUI()
    {
        GUILayout.Label("Source Prefabs (содержат правильный PlayerIcon)", EditorStyles.boldLabel);

        int newSourceCount = Mathf.Max(0, EditorGUILayout.IntField("Source Count", sourcePrefabs.Count));
        while (newSourceCount > sourcePrefabs.Count) sourcePrefabs.Add(null);
        while (newSourceCount < sourcePrefabs.Count) sourcePrefabs.RemoveAt(sourcePrefabs.Count - 1);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(150));
        for (int i = 0; i < sourcePrefabs.Count; i++)
        {
            sourcePrefabs[i] = (GameObject)EditorGUILayout.ObjectField(sourcePrefabs[i], typeof(GameObject), true);
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        GUILayout.Label("Target Prefabs (куда вставлять иконки)", EditorStyles.boldLabel);

        int newTargetCount = Mathf.Max(0, EditorGUILayout.IntField("Target Count", targetPrefabs.Count));
        while (newTargetCount > targetPrefabs.Count) targetPrefabs.Add(null);
        while (newTargetCount < targetPrefabs.Count) targetPrefabs.RemoveAt(targetPrefabs.Count - 1);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(150));
        for (int i = 0; i < targetPrefabs.Count; i++)
        {
            targetPrefabs[i] = (GameObject)EditorGUILayout.ObjectField(targetPrefabs[i], typeof(GameObject), true);
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(20);

        if (GUILayout.Button("Скопировать PlayerIcon спрайты"))
        {
            CopySprites();
        }
    }

    private void CopySprites()
    {
        int successCount = 0;

        foreach (var source in sourcePrefabs)
        {
            if (source == null) continue;

            GameObject target = targetPrefabs.Find(t => t != null && t.name == source.name);
            if (target == null)
            {
                Debug.LogWarning($"Нет соответствующего target префаба для: {source.name}");
                continue;
            }

            // Найти PlayerIcon -> Image в source
            Transform sourceIconTransform = source.transform.Find("PlayerIcon");
            if (sourceIconTransform == null)
            {
                Debug.LogWarning($"Source prefab {source.name} не содержит PlayerIcon");
                continue;
            }

            Image sourceImage = sourceIconTransform.GetComponent<Image>();
            if (sourceImage == null)
            {
                Debug.LogWarning($"У PlayerIcon в source prefab {source.name} нет компонента Image");
                continue;
            }

            // Найти PlayerIcon -> Image в target
            Transform targetIconTransform = target.transform.Find("PlayerIcon");
            if (targetIconTransform == null)
            {
                Debug.LogWarning($"Target prefab {target.name} не содержит PlayerIcon");
                continue;
            }

            Image targetImage = targetIconTransform.GetComponent<Image>();
            if (targetImage == null)
            {
                Debug.LogWarning($"У PlayerIcon в target prefab {target.name} нет компонента Image");
                continue;
            }

            // Копирование
            Undo.RecordObject(targetImage, "Copy PlayerIcon Sprite");
            targetImage.sprite = sourceImage.sprite;

            EditorUtility.SetDirty(targetImage);
            EditorUtility.SetDirty(target);
            successCount++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Скопировано иконок: {successCount}");
    }
}
