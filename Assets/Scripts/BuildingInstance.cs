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
        if (currentLevel >= 25)
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

        Debug.Log(buildingType.buildingName + " binası " + currentLevel + ". seviyeye yükseltildi.");
    }
}