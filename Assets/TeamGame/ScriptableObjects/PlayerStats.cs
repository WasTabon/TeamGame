using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Player/Stats", order = 1)]
public class PlayerStats : ScriptableObject
{
    [Header("Основные характеристики")]
    public string characterName;
    public int speed;
    public int stamina;
    public int attack;
    public int defense;
}