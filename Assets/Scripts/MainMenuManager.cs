using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Sahne yönetimi için bu kütüphaneyi eklemeliyiz!

public class MainMenuManager : MonoBehaviour
{
    // Bu fonksiyonu butona bağlayacağız.
    // 'public' olması önemli, böylece Unity editöründen erişebiliriz.
    public void PlayGame()
    {
        // "BaseScene" ismindeki sahneyi yükle.
        // Tırnak içindeki ismin, birazdan oluşturacağımız sahne dosyasının adıyla aynı olması GEREKİR.
        SceneManager.LoadScene("BaseScene");
    }
}