using UnityEngine;

public class SpecsController : MonoBehaviour
{
    [SerializeField]
    private SpecCardsContainer specCardsContainer;

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
        return specCardsContainer;
    }

    protected void Awake()
    {
        instance = this;
        specBase = SpecBase.GetInstance();

        for (int specID = 0; specID < 2000; specID++)
        {
            specBase.PutSpec(specGenerator.GenerateSpec(specID));
        }
    }
    public void Update()
    {
        if (Time.frameCount % 1000 == 0)
        {
            SpawnNextSpec();
        }
    }
    public void SpawnNextSpec()
    {
        Spec spec = specBase.TakeSpec();

        if (spec != null)
        {
            specCardsContainer.AddSpecCard(spec);
        }
    }

    public SpecCard[] GetSpecCards()
    {
        return specCardsContainer.GetSpecCards();
    }
}
