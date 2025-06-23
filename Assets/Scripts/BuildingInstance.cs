using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInstance : MonoBehaviour, IPointerClickHandler
{
    public BuildingType buildingType;
    public int currentLevel = 1;

    // Bu binanın SpriteRenderer bileşenine referans.
    private SpriteRenderer spriteRenderer;
    private static BuildManager buildManager;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Eğer buildManager değişkenimiz boşsa (yani daha önce hiç bulunmadıysa)...
        if (buildManager == null)
        {
            // ...sahnedeki BuildManager'ı bul ve değişkene ata.
            // Artık modern ve daha hızlı fonksiyonu kullanıyoruz.
            buildManager = FindAnyObjectByType<BuildManager>();
        }
    }

    // Tıklanma olayını yakalamak için bu fonksiyonu ekliyoruz.
    public void OnPointerClick(PointerEventData eventData)
    {
        // Artık her seferinde sahneyi taramak yerine, hafızadaki referansı kullanıyoruz.
        if (buildManager != null)
        {
            buildManager.OpenBuildingInfoPanel(this);
        }
        else
        {
            Debug.LogError("Sahnede BuildManager bulunamadı!");
        }
    }

    public float GetProductionRate()
    {
        if (buildingType.producedResource == ResourceType.None)
            return 0;

        // Üretim = Temel Üretim + (Seviye - 1) * Seviye Başı Artış
        return buildingType.baseProductionPerHour + ((currentLevel - 1) * buildingType.productionIncreasePerLevel);
    }

    // Bir sonraki seviyeye yükseltmenin odun maliyetini hesaplar.
    public int GetNextUpgradeWoodCost()
    {
        // Maliyet = Temel Maliyet * (Maliyet Artış Faktörü ^ (Mevcut Seviye - 1))
        return Mathf.FloorToInt(buildingType.baseUpgradeWoodCost * Mathf.Pow(buildingType.costIncreaseFactor, currentLevel - 1));
    }

    // Bir sonraki seviyeye yükseltmenin taş maliyetini hesaplar.
    public int GetNextUpgradeStoneCost()
    {
        return Mathf.FloorToInt(buildingType.baseUpgradeStoneCost * Mathf.Pow(buildingType.costIncreaseFactor, currentLevel - 1));
    }
    public float storedAmount = 0f;
    public int GetCurrentCapacity()
    {
        return buildingType.baseStorageCapacity + ((currentLevel - 1) * buildingType.capacityIncreasePerLevel);
    }

    // Bu fonksiyon, binanın seviyesine göre doğru görseli ayarlayacak.
    public void UpdateBuildingVisuals()
    {
        // Eğer spriteRenderer bulunamadıysa hata vermemesi için kontrol.
        if (spriteRenderer == null)
        {
            Debug.LogError("Bu binada SpriteRenderer bileşeni bulunamadı!");
            return;
        }

        // Seviyeye göre hangi sprite index'ini kullanacağımızı belirle.
        int spriteIndex = 0;
        if (currentLevel >= 15)
        {
            spriteIndex = 2; // Seviye 15 ve üstü için 3. görsel
        }
        else if (currentLevel >= 5)
        {
            spriteIndex = 1; // Seviye 5-14 için 2. görsel
        }
        // Seviye 1-4 için 0. görsel (default)

        // BuildingType asset'imizdeki görsellerin sayısı yeterli mi diye kontrol et.
        if (buildingType.levelSprites.Length > spriteIndex)
        {
            // Sprite'ı değiştir.
            spriteRenderer.sprite = buildingType.levelSprites[spriteIndex];
        }
        else
        {
            Debug.LogWarning(buildingType.name + " için yeterli seviye görseli atanmamış!");
        }
    }

    // Yükseltme fonksiyonu
    public void UpgradeBuilding()
    {
        // Max seviye kontrolü
        if (currentLevel >= 20)
        {
            Debug.Log(buildingType.buildingName + " zaten maksimum seviyede!");
            return;
        }

        currentLevel++; // Seviyeyi bir artır.
        UpdateBuildingVisuals(); // Görseli yeni seviyeye göre güncelle.

        // Eğer bu ana bina ise, GameManager'daki seviyeyi de güncelle.
        if (buildingType.name.Contains("AnaMerkez"))
        {
            GameManager.Instance.townHallLevel = currentLevel;
        }
        // Yükseltme sonrası bonusu uygula.
        ApplyCapacityBonus();
        Debug.Log(buildingType.buildingName + " binası " + currentLevel + ". seviyeye yükseltildi.");
    }

    // BuildingInstance.cs'in içine bu yeni fonksiyonu ekle
    public void CollectResources()
    {
        // Toplanacak miktarı tam sayı olarak al.
        int amountToCollect = (int)storedAmount;

        // Eğer toplanacak bir şey yoksa, hiçbir şey yapma.
        if (amountToCollect <= 0) return;

        // İlgili kaynağı GameManager'a ekle.
        switch (buildingType.producedResource)
        {
            case ResourceType.Food:
                GameManager.Instance.AddFood(amountToCollect);
                break;
            case ResourceType.Wood:
                GameManager.Instance.AddWood(amountToCollect);
                break;
            case ResourceType.Stone:
                GameManager.Instance.AddStone(amountToCollect);
                break;
        }

        // Binanın içindeki birikmiş miktarı sıfırla.
        storedAmount = 0f;

        Debug.Log($"{amountToCollect} {buildingType.producedResource} toplandı!");
    }
    
    public void OnConstructed()
    {
        ApplyCapacityBonus();
    }

private void ApplyCapacityBonus()
    {
        // Eğer bu bina bir kapasite bonusu vermiyorsa, hiçbir şey yapma.
        if (buildingType.capacityBonus <= 0) return;

        // Hangi kapasiteyi artıracağımızı binanın adına göre belirleyelim.
        // Bu yöntem basit ama etkilidir.
        if (buildingType.name.Contains("Depo"))
        {
            GameManager.Instance.foodCapacity += buildingType.capacityBonus;
            GameManager.Instance.woodCapacity += buildingType.capacityBonus;
            GameManager.Instance.stoneCapacity += buildingType.capacityBonus;
            Debug.Log($"Depo bonusu uygulandı! Yeni hammadde kapasitesi: {GameManager.Instance.foodCapacity}");
        }
        else if (buildingType.name.Contains("Ev"))
        {
            GameManager.Instance.populationCapacity += buildingType.capacityBonus;
            Debug.Log($"Ev bonusu uygulandı! Yeni nüfus kapasitesi: {GameManager.Instance.populationCapacity}");
        }

        // Arayüzün de kapasiteyi göstermesi için güncelleme sinyali gönderelim.
        GameManager.Instance.NotifyResourcesChanged();
    }
}