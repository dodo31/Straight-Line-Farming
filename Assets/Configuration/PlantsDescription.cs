using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Plants", menuName = "d-lan-3/Plants Description", order = 0)]
public class PlantsDescription : ScriptableObject
{
    public List<PlantDescription> Descriptions;
}