using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton: Diğer script'lerin kolayca erişmesi için.

    [Header("Oyuncu Verileri")]
    public int townHallLevel = 0; // Başlangıçta Ana Merkez yok, seviyesi 0.

    // Hangi binadan kaç tane olduğunu saymak için bir sözlük (Dictionary).
    public Dictionary<BuildingType, int> buildingCounts = new Dictionary<BuildingType, int>();

    [Header("Oyuncu Hammaddeleri")]
    public int food = 500;
    public int wood = 500;
    public int stone = 500;
    public int population = 50;
    public int gold = 100;

    // Kaynaklar her değiştiğinde bu olay tetiklenecek.
    public static event Action OnResourcesChanged;

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

    public void AddFood(int amount)
    {
        food += amount;
        // Arayüze "kaynaklar değişti, kendini güncelle!" diye haber ver.
        OnResourcesChanged?.Invoke();
    }
    public void AddWood(int amount)
    {
        wood += amount;
        OnResourcesChanged?.Invoke();
    }
    public void AddGold(int amount)
    {
        gold += amount;
        OnResourcesChanged?.Invoke();
    }
    public void AddPopulation(int amount)
    {
        population += amount;
        OnResourcesChanged?.Invoke();
    }
    public void AddStone(int amount)
    {
        population += amount;
        OnResourcesChanged?.Invoke();
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