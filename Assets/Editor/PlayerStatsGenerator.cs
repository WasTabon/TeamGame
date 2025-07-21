using UnityEngine;
using UnityEditor;

public class PlayerStatsGenerator : EditorWindow
{
    private string[] names = new string[]
    {
        "Leonardo Vega",
        "Dmitry Sokolov",
        "Carlos Mendez",
        "Anders Olsson",
        "Victor Ivanenko",
        "Tariq Al-Hassan",
        "Nico Duarte",
        "Yusuf Benali",
        "Markus Ritter",
        "Takumi Hayashi",
        "Dwayne Carter",
        "Rafael Moreno",
        "Luka Vranješ"
    };

    [MenuItem("Tools/Generate PlayerStats")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PlayerStatsGenerator), false, "Generate PlayerStats");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Создать ScriptableObject для каждого игрока"))
        {
            GenerateStatsAssets();
        }
    }

    private void GenerateStatsAssets()
    {
        string folderPath = "Assets/PlayerStats";

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets", "PlayerStats");
        }

        foreach (string name in names)
        {
            PlayerStats stats = ScriptableObject.CreateInstance<PlayerStats>();
            stats.characterName = name;
            stats.speed = Random.Range(1, 11);
            stats.stamina = Random.Range(1, 11);
            stats.attack = Random.Range(1, 11);
            stats.defense = Random.Range(1, 11);

            // Удаляем потенциально проблемные символы
            string safeName = name.Replace("č", "c").Replace("š", "s").Replace("ž", "z").Replace("ć", "c");

            string assetPath = $"{folderPath}/{safeName}.asset";

            AssetDatabase.CreateAsset(stats, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("ScriptableObject игроки успешно созданы с пробелами в имени файла.");
    }

}