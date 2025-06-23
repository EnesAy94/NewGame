using UnityEngine;
using UnityEngine.UI; // Button gibi UI elemanları için bu gerekli.
using TMPro;         // TextMeshPro kullanacağımız için bu gerekli.
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{

    [Header("Genel Ayarlar")]
    public List<BuildingType> allBuildingTypes; // Bütün bina asset'lerini buraya sürükleyeceğiz.
    public GameObject buildingButtonPrefab; // Butonlarımızı oluşturacağımız prefab.
    public Transform buttonContainer; // Butonların içine oluşturulacağı panel (GridLayoutGroup'lu).

    [Header("İnşaat Menüsü")]
    public GameObject buildMenuPanel;

    [Header("Bina Bilgi Paneli")]
    public GameObject buildingInfoPanel;
    public TextMeshProUGUI buildingNameText;
    public TextMeshProUGUI levelText;
    public Button upgradeButton; // Değişken türünü Button olarak değiştirdik.

    // Hafızada tutulan seçili objeler
    private BuildingPlot selectedPlot;
    private BuildingInstance selectedBuilding;

    #region Unity Yaşam Döngüsü (Awake)

    private void Awake()
    {
        // Başlangıçta tüm panellerin kapalı olduğundan emin ol.
        CloseAllPanels();

        // Sahnedeki tüm BuildingPlot'ları bul ve onlara bu manager'ı tanıt.
        BuildingPlot[] plots = FindObjectsByType<BuildingPlot>(FindObjectsSortMode.None);
        foreach (BuildingPlot plot in plots)
        {
            plot.buildManager = this;
        }
    }

    #endregion

    #region Panel Yönetimi

    // Herhangi bir panel açılmadan önce çağrılacak merkezi kapatma fonksiyonu.
    private void CloseAllPanels()
    {
        if (buildMenuPanel != null)
            buildMenuPanel.SetActive(false);

        if (buildingInfoPanel != null)
            buildingInfoPanel.SetActive(false);

        // Seçimleri temizle
        selectedPlot = null;
        selectedBuilding = null;
    }

    // İnşaat menüsünü açar.
    public void OpenBuildMenu(BuildingPlot plot)
    {
        CloseAllPanels();
        selectedPlot = plot;
        buildMenuPanel.SetActive(true);

        UpdateBuildMenu(); // Yeni dinamik menü oluşturma fonksiyonu.
    }

    void UpdateBuildMenu()
    {
        // 1. Konteynerdeki eski butonları temizle.
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. Tüm bina tipleri için yeni butonlar oluştur.
        foreach (BuildingType type in allBuildingTypes)
        {
            // Buton prefab'ından yeni bir buton oluştur ve konteynerin içine koy.
            GameObject buttonGO = Instantiate(buildingButtonPrefab, buttonContainer);
            Button newButton = buttonGO.GetComponent<Button>();

            Image iconImage = buttonGO.transform.Find("IconImage").GetComponent<Image>();
            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            // İkonu ve yazıyı ata.
            iconImage.sprite = type.icon;
            buttonText.text = type.buildingName;

            // 3. Butonun tıklanabilir olup olmadığını kontrol et.
            int currentCount = 0;
            if (GameManager.Instance.buildingCounts.ContainsKey(type))
            {
                currentCount = GameManager.Instance.buildingCounts[type];
            }

            bool canBuild = true;
            if (currentCount >= type.requiredTownHallLevels.Length)
            {
                canBuild = false; // Max sayıya ulaşıldı.
            }
            else
            {
                int requiredLevel = type.requiredTownHallLevels[currentCount];
                if (GameManager.Instance.townHallLevel < requiredLevel)
                {
                    canBuild = false; // Ana merkez seviyesi yetersiz.
                }
            }

            // Butonun durumunu ayarla.
            newButton.interactable = canBuild;

            // 4. Butona doğru tıklama olayını ata.
            // Bu kısım önemli, döngüdeki 'type' değişkenini doğrudan kullanamayız.
            BuildingType currentType = type;
            newButton.onClick.AddListener(() => BuildBuilding(currentType));
        }
    }

    // Bina bilgi/yükseltme panelini açar.
    public void OpenBuildingInfoPanel(BuildingInstance building)
    {
        CloseAllPanels(); // Önce diğer tüm panelleri kapat.

        selectedBuilding = building;
        buildingInfoPanel.SetActive(true);

        // Paneldeki UI elemanlarını seçilen binanın bilgileriyle doldur.
        UpdateBuildingInfoUI();

        // Yükseltme butonunun tıklama olayını ayarla.
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(UpgradeSelectedBuilding);
    }

    public void CloseActivePanel()
    {
        CloseAllPanels();
    }

    #endregion

    #region İnşaat ve Yükseltme Mantığı

    // İnşaat menüsündeki butonlar tarafından çağrılır.
    public void BuildBuilding(BuildingType buildingToBuild)
    {
        // 1. Mevcut bina sayısını al.
        int currentCount = 0;
        if (GameManager.Instance.buildingCounts.ContainsKey(buildingToBuild))
        {
            currentCount = GameManager.Instance.buildingCounts[buildingToBuild];
        }

        // 2. Maksimum inşaat limitine ulaşılmış mı? (Dizinin uzunluğu max limiti belirler)
        if (currentCount >= buildingToBuild.requiredTownHallLevels.Length)
        {
            Debug.Log(buildingToBuild.buildingName + " için maksimum inşaat sayısına ulaştınız!");
            return;
        }

        // 3. Bir sonraki binayı inşa etmek için gereken Ana Merkez seviyesini al.
        // Dizinin 'currentCount' index'indeki eleman, bir sonraki binanın kuralıdır.
        int requiredLevel = buildingToBuild.requiredTownHallLevels[currentCount];

        // 4. Oyuncunun Ana Merkez seviyesi yeterli mi?
        if (GameManager.Instance.townHallLevel < requiredLevel)
        {
            Debug.Log(buildingToBuild.buildingName + " inşa etmek için Ana Merkezinizin " + requiredLevel + ". seviye olması gerekiyor!");
            return;
        }

        // --- Tüm kontrollerden geçti, inşa et! ---

        GameObject newBuildingObject = Instantiate(buildingToBuild.buildingPrefab, selectedPlot.transform.position, Quaternion.identity);
        BuildingInstance newBuildingInstance = newBuildingObject.GetComponent<BuildingInstance>();
        newBuildingInstance.buildingType = buildingToBuild;
        newBuildingInstance.currentLevel = 1;
        newBuildingInstance.UpdateBuildingVisuals();

        GameManager.Instance.OnBuildingConstructed(buildingToBuild);
        Destroy(selectedPlot.gameObject);

        CloseAllPanels();
    }

    // Yükseltme butonu tarafından çağrılır.
    public void UpgradeSelectedBuilding()
    {
        if (selectedBuilding == null) return;

        // Yükseltme için gerekli kontroller
        // Ana Merkez değilse ve seviyesi Ana Merkez seviyesini AŞACAKSA, engelle.
        if (!selectedBuilding.buildingType.name.Contains("AnaMerkez"))
        {
            if (selectedBuilding.currentLevel >= GameManager.Instance.townHallLevel)
            {
                Debug.Log("Bu binayı yükseltmek için önce Ana Merkezinizi yükseltmelisiniz!");
                return;
            }
        }

        // Kontrolden geçerse yükseltme işlemini yap.
        selectedBuilding.UpgradeBuilding();

        // Yükseltme sonrası paneldeki bilgileri anında güncelle.
        UpdateBuildingInfoUI();
    }

    #endregion

    #region UI Güncelleme

    // Seçili binaya göre bilgi panelini günceller.
    private void UpdateBuildingInfoUI()
    {
        if (selectedBuilding == null) return;

        buildingNameText.text = selectedBuilding.buildingType.buildingName;
        levelText.text = "Seviye: " + selectedBuilding.currentLevel + " / 20";
        CheckUpgradeButtonState();
    }

    // Yükseltme butonunun tıklanabilir olup olmadığını kontrol eder.
    private void CheckUpgradeButtonState()
    {
        bool canUpgrade = true;

        // Max seviyeye ulaştıysa kapat.
        if (selectedBuilding.currentLevel >= 20)
        {
            canUpgrade = false;
        }
        // Ana Merkez değilse ve mevcut seviyesi Ana Merkez seviyesine eşit veya büyükse kapat.
        else if (!selectedBuilding.buildingType.name.Contains("AnaMerkez") && selectedBuilding.currentLevel >= GameManager.Instance.townHallLevel)
        {
            canUpgrade = false;
        }

        upgradeButton.interactable = canUpgrade;
    }

    #endregion
}