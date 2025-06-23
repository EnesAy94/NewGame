using UnityEngine;
using UnityEngine.UI; // Image için bu gerekli.
using TMPro; // TextMeshPro için bu gerekli.

public class ResourcesUI : MonoBehaviour
{
    [Header("Metin Alanları")]
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI goldText;

    [Header("Doluluk Barları")]
    public Image foodFillBar;
    public Image woodFillBar;
    public Image stoneFillBar;
    public Image populationFillBar;

    // Bu script aktif olduğunda...
    private void OnEnable()
    {
        // GameManager'daki olaya abone ol.
        GameManager.OnResourcesChanged += UpdateResourceTexts;
    }

    // Bu script devre dışı kaldığında...
    private void OnDisable()
    {
        // Aboneliği iptal et (hafıza sızıntılarını önlemek için çok önemli!).
        GameManager.OnResourcesChanged -= UpdateResourceTexts;
    }

    private void Start()
    {
        // Oyun başladığında ilk değerleri göstermek için bir kere çalıştır.
        UpdateResourceTexts();
    }

    // GameManager'dan gelen "güncelle" sinyaliyle bu fonksiyon çalışır.
    private void UpdateResourceTexts()
    {
        if (GameManager.Instance == null) return;
        
        var gm = GameManager.Instance;

        // Food
        foodText.text = $"{gm.food}";
        // Doluluk oranını hesapla (0 ile 1 arasında bir değer)
        // (float) dönüşümü, tamsayı bölmesi yerine ondalıklı bölme yapılmasını sağlar.
        foodFillBar.fillAmount = (float)gm.food / gm.foodCapacity;

        // Wood
        woodText.text = $"{gm.wood}";
        woodFillBar.fillAmount = (float)gm.wood / gm.woodCapacity;

        // Stone
        stoneText.text = $"{gm.stone}";
        stoneFillBar.fillAmount = (float)gm.stone / gm.stoneCapacity;

        // Population
        populationText.text = $"{gm.population}";
        populationFillBar.fillAmount = (float)gm.population / gm.populationCapacity;
    }
}