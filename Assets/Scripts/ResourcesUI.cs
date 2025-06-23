using UnityEngine;
using TMPro; // TextMeshPro için bu gerekli.

public class ResourcesUI : MonoBehaviour
{
    [Header("Metin Alanları")]
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI goldText;

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
        // GameManager'daki güncel verileri alıp metinlere yaz.
        // .ToString() bir sayıyı metne çevirir.
        if (GameManager.Instance != null)
        {
            foodText.text = GameManager.Instance.food.ToString();
            woodText.text = GameManager.Instance.wood.ToString();
            populationText.text = GameManager.Instance.population.ToString();
            goldText.text = GameManager.Instance.gold.ToString();
            stoneText.text = GameManager.Instance.stone.ToString();
        }
    }
}