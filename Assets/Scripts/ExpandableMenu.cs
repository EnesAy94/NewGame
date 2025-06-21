using UnityEngine;

public class ExpandableMenu : MonoBehaviour
{
    // Bu slota, küçük butonları içeren paneli sürükleyeceğiz.
    public GameObject actionButtonsPanel; 

    // Unity'ye bu fonksiyonun varlığını söylemek için script'i en başta bir kere çalıştır.
    private void Start()
    {
        // Oyun başladığında menünün kapalı olduğundan emin ol.
        actionButtonsPanel.SetActive(false);
    }
    
    // Ana butona basıldığında bu fonksiyon çalışacak.
    public void ToggleMenu()
    {
        // Panel aktifse kapat, kapalıysa aç.
        // ! işareti, bool değerinin tersini alır (true ise false, false ise true).
        actionButtonsPanel.SetActive( !actionButtonsPanel.activeSelf );
    }
}