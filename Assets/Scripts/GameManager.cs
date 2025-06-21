using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton: Diğer script'lerin kolayca erişmesi için.

    [Header("Oyuncu Verileri")]
    public int townHallLevel = 0; // Başlangıçta Ana Merkez yok, seviyesi 0.

    // Hangi binadan kaç tane olduğunu saymak için bir sözlük (Dictionary).
    public Dictionary<BuildingType, int> buildingCounts = new Dictionary<BuildingType, int>();

    private void Awake()
    {
        // Singleton kurulumu
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Bir bina inşa edildiğinde bu fonksiyon çağrılacak.
    public void OnBuildingConstructed(BuildingType type)
    {
        if (!buildingCounts.ContainsKey(type))
        {
            buildingCounts[type] = 0;
        }
        buildingCounts[type]++;

        // Eğer inşa edilen Ana Merkez ise, seviyesini 1 yap.
        if (type.name.Contains("AnaMerkez")) // Asset adından kontrol ediyoruz.
        {
            townHallLevel = 1;
        }
    }

    // Belli bir binadan kaç tane inşa edebiliriz?
    public int GetAllowedBuildCount(BuildingType type)
    {
        if (townHallLevel == 0 && !type.name.Contains("AnaMerkez")) return 0; // Ana Merkez yoksa başka bir şey yapılamaz.
        if (townHallLevel < 5) return 1;
        if (townHallLevel < 10) return 2;
        if (townHallLevel < 15) return 3;
        if (townHallLevel < 20) return 4;
        return 5; // Seviye 20 ve üstü
    }
}