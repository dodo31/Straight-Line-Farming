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
    

}
