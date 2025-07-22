using UnityEngine;
using UnityEditor;
using TMPro;

public class CreateItemPrefabsEditor : EditorWindow
{
    GameObject prefabTemplate;
    Transform parentUI;
    string[] itemNames = new string[]
    {
        "Premium Jersey Set",
        "Golden Boots",
        "Tactic Board Skin",
        "Custom Team Banner",
        "Energy Drink Pack",
        "Legendary Captain Armband",
        "Stadium Fireworks",
        "VIP Substitution Bench",
        "Goal Celebration Emotes",
        "Retro Ball Pack"
    };

    [MenuItem("Tools/Create Item UI Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<CreateItemPrefabsEditor>("Create Item UI");
    }

    void OnGUI()
    {
        GUILayout.Label("Item UI Prefab Generator", EditorStyles.boldLabel);

        prefabTemplate = (GameObject)EditorGUILayout.ObjectField("Prefab Template", prefabTemplate, typeof(GameObject), false);
        parentUI = (Transform)EditorGUILayout.ObjectField("Parent UI Transform", parentUI, typeof(Transform), true);

        if (GUILayout.Button("Generate Items"))
        {
            if (prefabTemplate == null || parentUI == null)
            {
                Debug.LogError("Please assign both Prefab Template and Parent UI.");
                return;
            }

            GenerateItems();
        }
    }

    void GenerateItems()
    {
        foreach (string itemName in itemNames)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabTemplate);
            instance.transform.SetParent(parentUI, false);
            instance.name = itemName;

            TMP_Text tmp = instance.GetComponentInChildren<TMP_Text>();
            if (tmp != null)
            {
                tmp.text = itemName;
            }
            else
            {
                Debug.LogWarning($"TMP_Text not found in {itemName}");
            }

            // Optionally, save prefab to Assets
            // string path = $"Assets/Prefabs/Items/{itemName}.prefab";
            // PrefabUtility.SaveAsPrefabAsset(instance, path);
        }

        Debug.Log("Item UI elements created.");
    }
}
