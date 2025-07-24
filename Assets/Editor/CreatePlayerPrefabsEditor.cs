using UnityEngine;
using UnityEditor;
using TMPro;

public class CreatePlayerPrefabsEditor : EditorWindow
{
    GameObject prefabTemplate;
    Transform parentUI;

    string[] playerFullNames = new string[]
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

    [MenuItem("Tools/Create Player UI Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<CreatePlayerPrefabsEditor>("Create Player UI");
    }

    void OnGUI()
    {
        GUILayout.Label("Player UI Prefab Generator", EditorStyles.boldLabel);

        prefabTemplate = (GameObject)EditorGUILayout.ObjectField("Player Prefab Template", prefabTemplate, typeof(GameObject), false);
        parentUI = (Transform)EditorGUILayout.ObjectField("Parent UI Transform", parentUI, typeof(Transform), true);

        if (GUILayout.Button("Generate Players"))
        {
            if (prefabTemplate == null || parentUI == null)
            {
                Debug.LogError("Assign both Player Prefab Template and Parent UI!");
                return;
            }

            GeneratePlayers();
        }
    }

    void GeneratePlayers()
    {
        foreach (string fullName in playerFullNames)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabTemplate);
            instance.transform.SetParent(parentUI, false);
            instance.name = fullName;

            TMP_Text tmp = instance.GetComponentInChildren<TMP_Text>();
            if (tmp != null)
            {
                tmp.text = fullName;
            }
            else
            {
                Debug.LogWarning($"TMP_Text component not found in prefab instance for {fullName}");
            }
        }

        Debug.Log("Player UI elements created.");
    }
}
