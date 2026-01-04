using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    House,
    WindTurbine,
    Solar,
    WaterEnergy,
    Road,
    Fun,
    Factory
}
public class Building : MonoBehaviour  
{
    public BuildingType buildingType;

    public int width = 1;
    public int height = 1;
    public int cost = 100;

    public int energy = 0;
    public int nature = 0;
    public int water = 0;
    public int security = 0;
    public int happiness = 0;
    public int population = 0;

    public int taxIncome = 0;

    public bool requiresRoad = false;
}
