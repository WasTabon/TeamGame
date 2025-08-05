using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerStatsIconSetter : EditorWindow
{
    private List<PlayerStats> playerStatsList = new List<PlayerStats>();
    private List<GameObject> uiPrefabs = new List<GameObject>();
    private Vector2 scroll;

    [MenuItem("Tools/Set PlayerStats Icons")]
    public static void ShowWindow()
    {
        GetWindow<PlayerStatsIconSetter>("PlayerStats Icon Setter");
    }

    private void OnGUI()
    {
        GUILayout.Label("PlayerStats ScriptableObjects", EditorStyles.boldLabel);

        int newStatsCount = Mathf.Max(0, EditorGUILayout.IntField("Count", playerStatsList.Count));
        while (newStatsCount > playerStatsList.Count) playerStatsList.Add(null);
        while (newStatsCount < playerStatsList.Count) playerStatsList.RemoveAt(playerStatsList.Count - 1);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
        for (int i = 0; i < playerStatsList.Count; i++)
        {
            playerStatsList[i] = (PlayerStats)EditorGUILayout.ObjectField(playerStatsList[i], typeof(PlayerStats), false);
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        GUILayout.Label("UI Prefabs with PlayerIcon", EditorStyles.boldLabel);

        int newPrefabCount = Mathf.Max(0, EditorGUILayout.IntField("Count", uiPrefabs.Count));
        while (newPrefabCount > uiPrefabs.Count) uiPrefabs.Add(null);
        while (newPrefabCount < uiPrefabs.Count) uiPrefabs.RemoveAt(uiPrefabs.Count - 1);

        scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
        for (int i = 0; i < uiPrefabs.Count; i++)
        {
            uiPrefabs[i] = (GameObject)EditorGUILayout.ObjectField(uiPrefabs[i], typeof(GameObject), true);
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(20);

        if (GUILayout.Button("Set Icons from UI Prefabs"))
        {
            SetIcons();
        }
    }

    private void SetIcons()
    {
        int updated = 0;

        foreach (var stats in playerStatsList)
        {
            if (stats == null) continue;

            GameObject matchedPrefab = uiPrefabs.Find(prefab => prefab != null && prefab.name == stats.name);

            if (matchedPrefab == null)
            {
                Debug.LogWarning($"Не найден префаб с именем: {stats.name}");
                continue;
            }

            Transform iconTransform = matchedPrefab.transform.Find("PlayerIcon");

            if (iconTransform == null)
            {
                Debug.LogWarning($"У префаба {matchedPrefab.name} нет объекта 'PlayerIcon'");
                continue;
            }

            Image image = iconTransform.GetComponent<Image>();
            if (image == null)
            {
                Debug.LogWarning($"У 'PlayerIcon' в {matchedPrefab.name} нет компонента Image");
                continue;
            }

            stats.icon = image.sprite;
            EditorUtility.SetDirty(stats);
            updated++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Обновлено {updated} ScriptableObject'ов.");
    }
}
