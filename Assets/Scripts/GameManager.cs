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
}