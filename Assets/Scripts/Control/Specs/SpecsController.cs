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
    }
    protected void Start()
    {
        for (int specID = 0; specID < 2000; specID++)
        {
            specBase.PutSpec(specGenerator.GenerateSpec(specID));
        }
    }
    public void Update()
    {
        if (GetContainer().GetSpecCards().Length < ShopVars.GetInstance().visibleSpecAmount)
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
    public void DecreaseDeadlines()
    {
        SpecCard[] specCards = GetSpecCards();
        foreach(var specs in specCards)
        {
            specs.DecreaseDeadline();
        }
    }
    public SpecCard[] GetSpecCards()
    {
        return specCardsContainer.GetSpecCards();
    }
}
