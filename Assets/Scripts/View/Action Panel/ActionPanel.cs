using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    public PlantSpawnButton SpawnButtonPrefab;
    public PlantsDescription PlantsDescription;

    protected void Start()
    {
        AddSpawnButtons();
    }

    public void AddSpawnButtons()
    {
        for (int i = 0; i < PlantsDescription.Descriptions.Count; i++)
        {
            PlantDescription plantDescription = PlantsDescription.Descriptions[i];

            PlantSpawnButton spawnButton = Instantiate(SpawnButtonPrefab);
            spawnButton.name = $"{plantDescription.Name} Spawn Button";
            spawnButton.Image.sprite = plantDescription.Sprite;

            spawnButton.transform.SetParent(transform);
            spawnButton.transform.SetSiblingIndex(i);
        }
    }
}