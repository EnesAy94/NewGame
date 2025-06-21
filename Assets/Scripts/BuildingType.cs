using UnityEngine;

// Bu satır sayesinde Unity'nin "Create" menüsünde bu objeyi oluşturma seçeneği çıkar.
[CreateAssetMenu(fileName = "New Building Type", menuName = "Game/Building Type")]
public class BuildingType : ScriptableObject
{
    [Header("Genel Bilgiler")]
    public string buildingName; // "Ana Merkez", "Çiftlik" gibi
    public int maxBuildCount = 5; // En fazla kaç tane inşa edilebilir?
    
    [Header("Seviye ve Görsel")]
    public GameObject buildingPrefab; // Bu binanın temel prefab'ı
    public Sprite[] levelSprites; // Seviyeye göre değişen görseller (0: Seviye 1-4, 1: Seviye 5-14, 2: Seviye 15+)
}