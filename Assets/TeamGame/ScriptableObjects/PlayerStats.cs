using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerStatsData
{
    public string characterName;
    public int speed;
    public int stamina;
    public int attack;
    public int defense;
}

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player/Stats", order = 1)]
public class PlayerStats : ScriptableObject
{
    [Header("Основные характеристики")]
    public string characterName;
    public Sprite icon;
    public int speed;
    public int stamina;
    public int attack;
    public int defense;
    public string specialSkill;
    
    private const string SaveFolder = "PlayerStatsSaves";

    public void SaveToFile()
    {
        var data = new PlayerStatsData()
        {
            characterName = this.characterName,
            speed = this.speed,
            stamina = this.stamina,
            attack = this.attack,
            defense = this.defense
        };

        string json = JsonUtility.ToJson(data, true);

        string dir = Path.Combine(Application.persistentDataPath, SaveFolder);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string path = Path.Combine(dir, $"{characterName}.json");
        File.WriteAllText(path, json);
        Debug.Log($"Сохранено в файл: {path}");
    }
    
    public void LoadFromFile()
    {
        string path = Path.Combine(Application.persistentDataPath, SaveFolder, $"{characterName}.json");

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerStatsData data = JsonUtility.FromJson<PlayerStatsData>(json);

            this.speed = data.speed;
            this.stamina = data.stamina;
            this.attack = data.attack;
            this.defense = data.defense;

            Debug.Log($"Загружено из файла: {path}");
        }
        else
        {
            Debug.LogWarning("Файл не найден: " + path);
        }
    }
}