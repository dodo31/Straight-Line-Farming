using System.Linq;
using UnityEngine;

public class ActionPanel : MonoBehaviour
{
    [SerializeField]
    private Sprite sowBackgroundSprite;
    [SerializeField]
    private Sprite collectBackgroundSprite;

    [SerializeField]
    private ActionItem ActionItemPrefab;

    [SerializeField]
    private PlantsDescription PlantsDescription;

    private ActionItem[] actionItems;
    private int lastPromo = 0;

    protected void Awake()
    {
        AddSowButtons();
        actionItems = GetComponentsInChildren<ActionItem>(true);
    }

    protected void Start()
    {
        foreach (ActionItem actionItem in actionItems)
        {
            actionItem.OnClicked += Handle_OnItemClicked;
        }

        if (actionItems.Length > 0)
        {
            ActionItem firstItem = actionItems.First();
            ActionItem lastItem = actionItems.Last();

            SelectItem(firstItem);
            firstItem.SetAvailable();

            lastItem.SetTargetAction(new UserCollectAction());
            lastItem.SetAvailable();
            lastItem.Unselect();
        }
    }

    public void AddSowButtons()
    {
        for (int i = 0; i < PlantsDescription.Descriptions.Count; i++)
        {
            PlantDescription plantDescription = PlantsDescription.Descriptions[i];

            ActionItem plantSowItem = Instantiate(ActionItemPrefab);
            plantSowItem.name = $"{plantDescription.Name} Spawn Item";
            plantSowItem.SetPrice(plantDescription.price);
            plantSowItem.SetIcon(plantDescription.UiSprite);
            plantSowItem.SetBackgroundSprite(sowBackgroundSprite);

            plantSowItem.transform.SetParent(transform, false);
            plantSowItem.transform.SetSiblingIndex(i);

            UserSowAction sowAction = new UserSowAction(plantDescription.Type);
            plantSowItem.SetTargetAction(sowAction);
            plantSowItem.SetAvailable();
        }
    }
    public void Update()
    {
        if(ShopVars.GetInstance().seedPromo != lastPromo)
        {
            lastPromo = ShopVars.GetInstance().seedPromo;
            UpdateSowButtons();
        }
    }
    public void UpdateSowButtons()
    {
        ActionItem[] pop = this.GetComponentsInChildren<ActionItem>();
        for (int i = 0; i < PlantsDescription.Descriptions.Count; i++)
        {
            pop[i].SetPrice(PlantsDescription.Descriptions[i].price - ShopVars.GetInstance().seedPromo * 10);
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