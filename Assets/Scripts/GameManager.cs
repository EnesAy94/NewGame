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

    [Header("Oyuncu Kapasiteleri")]
    public int foodCapacity = 1000;
    public int woodCapacity = 1000;
    public int stoneCapacity = 1000;
    public int populationCapacity = 100;

    // Kaynaklar her değiştiğinde bu olay tetiklenecek.
    public static event Action OnResourcesChanged;

    // Sahnedeki tüm üretim binalarının bir listesi.
    private List<BuildingInstance> productionBuildings = new List<BuildingInstance>();
    private float productionTimer = 0f;

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

    void Update()
    {
        // Her saniye üretim yap.
        productionTimer += Time.deltaTime;
        if (productionTimer >= 1f)
        {
            ProduceResources(1f); // 1 saniyelik üretim
            productionTimer = 0f;
        }
    }

    // Bina inşa edildiğinde veya yok edildiğinde bu listeyi güncelleyeceğiz.
    public void RegisterProductionBuilding(BuildingInstance building)
    {
        if (!productionBuildings.Contains(building))
            productionBuildings.Add(building);
    }

    public void UnregisterProductionBuilding(BuildingInstance building)
    {
        if (productionBuildings.Contains(building))
            productionBuildings.Remove(building);
    }

    // Belirli bir süre için kaynak üretir.
    private void ProduceResources(float seconds)
    {
        // Artık bu fonksiyondan sonra OnResourcesChanged çağırmıyoruz,
        // çünkü kaynaklar sadece binaların içinde birikiyor.

        foreach (var building in productionBuildings)
        {
            // Binanın mevcut kapasitesini al.
            int capacity = building.GetCurrentCapacity();

            // Eğer bina zaten tam kapasitede ise, üretim yapma.
            if (building.storedAmount >= capacity)
            {
                continue; // Döngünün bir sonraki elemanına geç.
            }

            // Üretim miktarını hesapla.
            float productionRatePerHour = building.GetProductionRate();
            float productionPerSecond = productionRatePerHour / 3600f;
            float producedAmount = productionPerSecond * seconds;

            // Üretilen miktarı binanın deposuna ekle.
            building.storedAmount += producedAmount;

            // Eğer ekleme sonrası kapasite aşıldıysa, kapasiteye eşitle.
            if (building.storedAmount > capacity)
            {
                building.storedAmount = capacity;
            }
        }
    }

    public void AddFood(int amount)
    {
        food += amount;
        // Eğer ekleme sonrası kapasite aşıldıysa, kapasiteye eşitle.
        if (food > foodCapacity)
        {
            food = foodCapacity;
        }
        OnResourcesChanged?.Invoke();
    }
    public void AddWood(int amount)
    {
        wood += amount;
        if (wood > woodCapacity)
        {
            wood = woodCapacity;
        }
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
        if (population > populationCapacity)
        {
            population = populationCapacity;
        }
        OnResourcesChanged?.Invoke();
    }
    public void AddStone(int amount)
    {
        stone += amount;
        if (stone > stoneCapacity)
        {
            stone = stoneCapacity;
        }
        OnResourcesChanged?.Invoke();
    }

    public void NotifyResourcesChanged()
    {
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