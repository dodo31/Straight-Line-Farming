using UnityEngine;

public class SpecsController : MonoBehaviour
{
    [SerializeField]
    private SpecPanelsContainer specPanelsContainer;

    private SpecBase specBase;

    protected void Awake()
    {
        specBase = SpecBase.GetInstance();
    }

    public void SpawnNextSpec()
    {
        Spec spec = specBase.TakeSpec();

        if (spec != null)
        {
            Sprite clientSprite = Resources.Load<Sprite>(spec.ClientSpritePath);
            SpecPanel newSpecPanel = specPanelsContainer.AddSpecPanel(spec.ClientName, clientSprite, spec.Deadline, spec.Gain);

            foreach (PlantCount plantCount in spec.RequiredPlantCounts)
            {
                specPanelsContainer.AddCountIndicatorToPanel(newSpecPanel, plantCount.Type, plantCount.Count);
            }
        }
    }
}
