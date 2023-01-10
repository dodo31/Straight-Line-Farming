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
        SpecBase.SpecBaseHolder.Instance = new SpecBase();
        specBase = SpecBase.GetInstance();
    }
    protected void Start()
    {
        GenerateSpecs();
    }
    public void Update()
    {
        if (GetContainer().GetSpecCards().Length < ShopVars.GetInstance().visibleSpecAmount)
        {
            SpawnNextSpec();
        }
    }

    public void GenerateSpecs()
    {
        // Update props
        int[] props = new int[specGenerator.unlockTimes.Length];


        for (int specID = 0; specID < 2000; specID++)
        {
            for (int food = 0; food < specGenerator.unlockTimes.Length; food++)
            {
                if (specID >= specGenerator.unlockTimes[food])
                {
                    props[food]++;
                }
            }

            string clientName = specGenerator.nameGenerator.GenName() + " " + specGenerator.surnameGenerator.GenName();
            Sprite sprite = specGenerator.sprites[Random.Range(0,specGenerator.sprites.Length)];
            int deadline = ShopVars.GetInstance().baseDays;
            int gain = 0;
            float minAmount = specGenerator.minAmountAtFirst + specGenerator.minAmountEvolution * specID;
            float maxAmount = specGenerator.maxAmountAtFirst + specGenerator.maxAmountEvolution * specID;
            if (minAmount > specGenerator.minAmountAtEnd)
            {
                minAmount = specGenerator.minAmountAtEnd;
            }

            if (maxAmount > specGenerator.maxAmountAtEnd)
            {
                maxAmount = specGenerator.maxAmountAtEnd;
            }

            int commandAm = (int)(Random.Range(minAmount, maxAmount));
            int[] amountOfFood = new int[specGenerator.unlockTimes.Length];

            for (int wanted = 0; wanted < commandAm; wanted++)
            {
                bool done = false;
                for (int food = 0; food < specGenerator.unlockTimes.Length; food++)
                {
                    if (specID == specGenerator.unlockTimes[food])
                    {
                        gain += specGenerator.basePrices[food];
                        amountOfFood[food]++;
                        done = true;
                    }
                }

                if (done)
                {
                    continue;
                }

                int total = 0;
                int i;
                for (i = 0; i < specGenerator.unlockTimes.Length; i++)
                {
                    total += props[i];
                }
                int rando = Random.Range(0, total);
                for (i = 0; i < specGenerator.unlockTimes.Length; i++)
                {
                    rando -= props[i];
                    if (rando < 0)
                    {
                        break;
                    }
                }

                gain += specGenerator.basePrices[i];
                amountOfFood[i]++;
                //Console.Write((char)('A' + i));
            }
            //Console.WriteLine("buying price: " + commandValue + " ; +200: " + (commandValue + 200));
            gain += Random.Range(0, 10) * 10;
            int difAmount = 0;
            int amount = 0;
            for (int food = 0; food < amountOfFood.Length; food++)
            {

                if (amountOfFood[food] > 0)
                {
                    difAmount++;
                }

                for (int am = 0; am < amountOfFood[food]; am++)
                {
                    amount++;
                    //Debug.Log((char)('A' + food));
                }
            }
            gain += (difAmount - 1) * 80 + amount * amount * 10 + 140;
            //Debug.Log($" : {gain}$");
            Spec spec = new(clientName, sprite, deadline, gain);
            for (int food = 0; food < amountOfFood.Length; food++)
            {
                if (amountOfFood[food] > 0)
                {
                    PlantCount plantCount = new((PlantTypes)food, amountOfFood[food]);
                    spec.AddPlantCount(plantCount);
                }
            }
            specBase.PutSpec(spec);
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
    public void IncreaseDeadlines()
    {
        SpecCard[] specCards = GetSpecCards();
        foreach (SpecCard specs in specCards)
        {
            specs.IncreaseDeadline();
        }
    }
    public void DecreaseDeadlines()
    {
        SpecCard[] specCards = GetSpecCards();
        foreach (SpecCard specs in specCards)
        {
            specs.DecreaseDeadline();
        }
    }
    public SpecCard[] GetSpecCards()
    {
        return specCardsContainer.GetSpecCards();
    }
}
