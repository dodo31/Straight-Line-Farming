using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    public ActionItem ActionItemPrefab;
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

            ActionItem plantSpawnItem = Instantiate(ActionItemPrefab);
            plantSpawnItem.name = $"{plantDescription.Name} Spawn Item";
            plantSpawnItem.SetTile(plantDescription.Name);
            plantSpawnItem.SetIcon(plantDescription.Sprite);

            plantSpawnItem.transform.SetParent(transform);
            plantSpawnItem.transform.SetSiblingIndex(i);
        }
    }
}