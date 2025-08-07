using UnityEngine;
using UnityEditor;
using TMPro;

public class CreateEnemyUIEditor : EditorWindow
{
    GameObject uiPrefab;
    Transform uiParent;

    string[] enemyTeamNames = new string[]
    {
        "Iron Vulture Syndicate", "Crimson Gear Rebels", "Toxic Phantom Crew",
        "Black Nitro Devils", "Chrome Cobra Unit", "Acid Thunder Squad",
        "Venom Drift Kings", "Shadow Voltage Mafia", "Dead Engine Gang",
        "Plasma Ghost Division", "Nuclear Turbo Force", "Psycho Wheel Bandits",
        "Bloody Rocket Corps", "Solar Doom Company", "Omega Street Slashers",
        "Savage Nitro Legion", "Rust Tornado Raiders", "Dark Plasma Mob",
        "Turbo Vortex Snakes", "Red Venom Brigade", "Cyber Wreck Troopers",
        "Ashblade Riot Crew", "Thunder Skull Pack", "Hellfire Drift Syndicate",
        "Quantum Flame Reapers", "Steel Horn Cult", "Razor Gear Commandos",
        "Electric Fang Squad", "Nocturne War Drive", "Blazing Chrome Hunters"
    };

    [MenuItem("Tools/Create Enemy UI List")]
    public static void ShowWindow()
    {
        GetWindow<CreateEnemyUIEditor>("Enemy UI Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Enemy UI Generator", EditorStyles.boldLabel);
        uiPrefab = (GameObject)EditorGUILayout.ObjectField("UI Prefab", uiPrefab, typeof(GameObject), false);
        uiParent = (Transform)EditorGUILayout.ObjectField("Parent UI Object", uiParent, typeof(Transform), true);

        if (GUILayout.Button("Generate Enemy UI List"))
        {
            if (uiPrefab == null || uiParent == null)
            {
                Debug.LogError("Assign both UI Prefab and Parent UI Object.");
                return;
            }

            GenerateEnemies();
        }
    }

    void GenerateEnemies()
    {
        for (int i = 0; i < enemyTeamNames.Length; i++)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(uiPrefab);
            instance.transform.SetParent(uiParent, false);
            instance.name = $"Enemy_{i + 1}";

            // Set TeamName
            SetTMPText(instance, "TeamName", enemyTeamNames[i]);

            // Set DayText
            SetTMPText(instance, "DayText", $"Day {i + 1}");

            // Set random stats
            SetTMPText(instance, "AttackText", $"Total Attack: {Random.Range(10, 36)}");
            SetTMPText(instance, "DefenseText", $"Total Defense: {Random.Range(10, 36)}");
            SetTMPText(instance, "StaminaText", $"Total Stamina: {Random.Range(10, 36)}");
            SetTMPText(instance, "SpeedText", $"Total Speed: {Random.Range(10, 36)}");
        }

        Debug.Log("Enemy UI elements created successfully.");
    }

    void SetTMPText(GameObject obj, string childName, string value)
    {
        Transform child = obj.transform.Find(childName);
        if (child != null)
        {
            TMP_Text tmp = child.GetComponent<TMP_Text>();
            if (tmp != null)
                tmp.text = value;
            else
                Debug.LogWarning($"TMP_Text missing on {childName}");
        }
        else
        {
            Debug.LogWarning($"Child '{childName}' not found in prefab.");
        }
    }
}
