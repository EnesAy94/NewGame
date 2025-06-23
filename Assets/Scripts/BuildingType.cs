using UnityEngine;

public enum ResourceType { None, Food, Wood, Stone, Gold }

[CreateAssetMenu(fileName = "New Building Type", menuName = "Game/Building Type")]
public class BuildingType : ScriptableObject
{
    [Header("Genel Bilgiler")]
    public string buildingName;
    public Sprite icon;
    public GameObject buildingPrefab;

    [Header("Seviye ve Görsel")]
    public Sprite[] levelSprites;

    [Header("İnşaat Kuralları")]
    public int[] requiredTownHallLevels;

    [Header("Üretim & Seviye Atlama")]
    public ResourceType producedResource; // Bu bina hangi kaynağı üretiyor?
    public float baseProductionPerHour; // 1. seviyedeki saatlik üretim miktarı.
    public float productionIncreasePerLevel; // Her seviye atladığında üretime ne kadar eklenecek?

    public int baseUpgradeWoodCost; // 1. seviyeden 2'ye geçmenin odun maliyeti.
    public int baseUpgradeStoneCost; // 1. seviyeden 2'ye geçmenin taş maliyeti.
    public float costIncreaseFactor; // Her seviye atladığında maliyet ne kadar artacak? (Örn: 1.5 -> %50 artış)
    public int baseStorageCapacity; // 1. seviyedeki depolama kapasitesi.
    public int capacityIncreasePerLevel; // Her seviyede kapasite ne kadar artacak?
}