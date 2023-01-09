using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpecGenerator")]
public class SpecGenerator : ScriptableObject
{
    public NameGenerator nameGenerator;
    public NameGenerator surnameGenerator;
    public Sprite[] sprites;
    public int[] unlockTimes;
    public int[] basePrices;
    public float minAmountAtFirst;
    public float minAmountEvolution;
    public float minAmountAtEnd;
    public float maxAmountAtFirst;
    public float maxAmountEvolution;
    public float maxAmountAtEnd;
    public Spec GenerateSpec(int commandID)
    {
        string clientName = nameGenerator.GenName() + " " + surnameGenerator.GenName();
        Sprite sprite = sprites[0];
        int deadline = ShopVars.GetInstance().baseDays;
        int gain = 0;
        float minAmount = minAmountAtFirst + minAmountEvolution * commandID;
        float maxAmount = maxAmountAtFirst + maxAmountEvolution * commandID;
        if (minAmount > minAmountAtEnd) minAmount = minAmountAtEnd;
        if (maxAmount > maxAmountAtEnd) maxAmount = maxAmountAtEnd;
        int commandAm = (int)(Random.Range(minAmount, maxAmount));
        int[] amountOfFood = new int[unlockTimes.Length];
        
        for (int wanted = 0; wanted < commandAm; wanted++)
        {

            bool done = false;
            // Update props
            int[] props = new int[unlockTimes.Length];
            for (int food = 0; food < unlockTimes.Length; food++)
            {
                if (commandID >= unlockTimes[food])
                {
                    props[food]++;
                }
                if (commandID == unlockTimes[food])
                {
                    gain += basePrices[food];
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
            for (i = 0; i < unlockTimes.Length; i++)
            {
                total += props[i];
            }
            int rando = Random.Range(0, total);
            for (i = 0; i < unlockTimes.Length; i++)
            {
                rando -= props[i];
                if (rando < 0)
                {
                    break;
                }
            }

            gain += basePrices[i];
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
            }
        }
        gain += (difAmount - 1) * 80 + amount * amount * 10 + 120;
        Spec spec = new(clientName, sprite, deadline, gain);
        for(int food = 0; food < amountOfFood.Length; food++)
        {
            if (amountOfFood[food] > 0)
            {
                PlantCount plantCount = new((PlantTypes)food, amountOfFood[food]);
                spec.AddPlantCount(plantCount);
            }
        }
        return spec;
    }

}
