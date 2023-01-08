using UnityEngine;

public class SpecsController : MonoBehaviour
{
    [SerializeField]
    private SpecCardsContainer specPanelsContainer;
    
    [SerializeField]
    private SpecGenerator specGenerator;
    private SpecBase specBase;

    private static SpecsController instance;
    public static SpecsController GetInstance()
    {
        return instance;
    }
    public SpecCardsContainer GetContainer()
    {
        return specPanelsContainer;
    }

    protected void Awake()
    {
        instance = this;
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
            SpecCard newSpecPanel = specPanelsContainer.AddSpecPanel(spec);

            foreach (PlantCount plantCount in spec.RequiredPlantCounts)
            {
                specPanelsContainer.AddCountIndicatorToPanel(newSpecPanel, plantCount.Type, plantCount.Count);
            }
        }
    }
}
