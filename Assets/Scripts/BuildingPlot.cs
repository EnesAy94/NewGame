using UnityEngine;
using UnityEngine.EventSystems; // Bu kütüphane çok önemli!

// IPointerClickHandler bir "interface"dir.
// Anlamı: "Bu script'i taşıyan obje, üzerine tıklandığında ne yapacağını bilir."
public class BuildingPlot : MonoBehaviour, IPointerClickHandler
{
    // Bu referans aynı kalıyor.
    [HideInInspector]
    public BuildManager buildManager;

    // IPointerClickHandler'ı kullandığımız için, Unity bizden bu fonksiyonu
    // yazmamızı zorunlu kılar. Bu fonksiyon, collider'a tıklandığında otomatik çalışır.
    public void OnPointerClick(PointerEventData eventData)
    {
        // buildManager'a haber ver.
        buildManager.OpenBuildMenu(this);
    }
}