using System.Linq;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    [SerializeField]
    private ActionItem ActionItemPrefab;

    [SerializeField]
    private PlantsDescription PlantsDescription;

    private ActionItem[] actionItems;

    protected void Start()
    {
        AddSowButtons();
        actionItems = GetComponentsInChildren<ActionItem>(true);

        foreach (ActionItem actionItem in actionItems)
        {
            actionItem.OnClicked += Handle_OnItemClicked;
        }

        if (actionItems.Length > 0)
        {
            SelectItem(actionItems.First());
            actionItems.Last().SetTargetAction(new UserCollectAction());
        }
    }

    public void AddSowButtons()
    {
        for (int i = 0; i < PlantsDescription.Descriptions.Count; i++)
        {
            PlantDescription plantDescription = PlantsDescription.Descriptions[i];

            ActionItem plantSowItem = Instantiate(ActionItemPrefab);
            plantSowItem.name = $"{plantDescription.Name} Spawn Item";
            plantSowItem.SetTile(plantDescription.Name);
            plantSowItem.SetIcon(plantDescription.Sprite);

            plantSowItem.transform.SetParent(transform);
            plantSowItem.transform.SetSiblingIndex(i);

            UserSowAction sowAction = new UserSowAction(plantDescription.Type);
            plantSowItem.SetTargetAction(sowAction);
        }
    }

    private void SelectItem(ActionItem targetItem)
    {
        UnselectAllItems();
        targetItem.Select();
    }

    private void UnselectAllItems()
    {
        foreach (ActionItem actionItem in actionItems)
        {
            actionItem.Unselect();
        }
    }

    public UserAction GetSelectedAction()
    {
        ActionItem selectedItem = GetSelectedItem();
        return selectedItem?.TargetAction;
    }

    private ActionItem GetSelectedItem()
    {
        return actionItems.FirstOrDefault(item => item.IsSelected);
    }

    private void Handle_OnItemClicked(ActionItem clickedItem)
    {
        SelectItem(clickedItem);
    }
}