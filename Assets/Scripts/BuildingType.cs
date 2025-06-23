using UnityEngine;

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
    // Bu dizi, her bir binanın hangi Ana Merkez seviyesinde inşa edilebileceğini belirler.
    // Örn: [1, 6, 12] -> 1. bina için sv. 1, 2. için sv. 6, 3. için sv. 12 gerekir.
    // Dizinin eleman sayısı, o binadan en fazla kaç tane inşa edilebileceğini de belirler.
    public int[] requiredTownHallLevels;
}