using UnityEngine;
using UnityEngine.UI; // Button gibi UI elemanları için bu gerekli.
using TMPro;         // TextMeshPro kullanacağımız için bu gerekli.

public class BuildManager : MonoBehaviour
{
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
        CloseAllPanels(); // Önce diğer tüm panelleri kapat.

        selectedPlot = plot;
        buildMenuPanel.SetActive(true);
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
        // Gerekli kontroller
        int currentCount = 0;
        if (GameManager.Instance.buildingCounts.ContainsKey(buildingToBuild))
        {
            currentCount = GameManager.Instance.buildingCounts[buildingToBuild];
        }

        if (currentCount >= buildingToBuild.maxBuildCount)
        {
            Debug.Log(buildingToBuild.buildingName + " için maksimum sayıya ulaştınız!");
            return;
        }

        if (!buildingToBuild.name.Contains("AnaMerkez") && currentCount >= GameManager.Instance.GetAllowedBuildCount(buildingToBuild))
        {
            Debug.Log("Daha fazla " + buildingToBuild.buildingName + " inşa etmek için Ana Merkez seviyenizi yükseltin!");
            return;
        }

        // Kontrollerden geçerse binayı oluştur.
        GameObject newBuildingObject = Instantiate(buildingToBuild.buildingPrefab, selectedPlot.transform.position, Quaternion.identity);
        BuildingInstance newBuildingInstance = newBuildingObject.GetComponent<BuildingInstance>();
        newBuildingInstance.buildingType = buildingToBuild;
        newBuildingInstance.currentLevel = 1;
        newBuildingInstance.UpdateBuildingVisuals();

        GameManager.Instance.OnBuildingConstructed(buildingToBuild);
        Destroy(selectedPlot.gameObject);

        CloseAllPanels(); // İşlem bitince paneli kapat.
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
        if(selectedBuilding == null) return;

        buildingNameText.text = selectedBuilding.buildingType.buildingName;
        levelText.text = "Seviye: " + selectedBuilding.currentLevel + " / 25";
        CheckUpgradeButtonState();
    }

    // Yükseltme butonunun tıklanabilir olup olmadığını kontrol eder.
    private void CheckUpgradeButtonState()
    {
        bool canUpgrade = true;

        // Max seviyeye ulaştıysa kapat.
        if (selectedBuilding.currentLevel >= 25)
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