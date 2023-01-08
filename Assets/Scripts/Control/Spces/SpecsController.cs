using UnityEngine;

public class SpecsController : MonoBehaviour
{
    [SerializeField]
    private SpecPanelsContainer specPanelsContainer;
    [SerializeField]
    private SpecGenerator specGenerator;
    private SpecBase specBase;

    protected void Awake()
    {
        specBase = SpecBase.GetInstance();
        for(int specID = 0; specID < 2000; specID++)
        {
            specBase.PutSpec(specGenerator.GenerateSpec(specID));
        }
    }
    public void Update()
    {
        if(Time.frameCount % 1000 == 0)
        {
            SpawnNextSpec();
        }
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
