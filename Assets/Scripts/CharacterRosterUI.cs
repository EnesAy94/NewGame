using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class CharacterRosterUI : MonoBehaviour
{
    public GameObject characterCardPrefab; // Az önce oluşturduğumuz prefab
    public Transform cardContainer; // Scroll View'ın içindeki 'Content' objesi

    // Panel açıldığında bu fonksiyon çağrılacak.
    public void PopulateRoster()
    {
        // Önce eski kartları temizle.
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // GameManager'dan oyuncunun sahip olduğu karakter listesini al.
        List<CharacterInstance> characters = GameManager.Instance.ownedCharacters;

        // Her bir karakter için bir kart oluştur.
        foreach (CharacterInstance character in characters)
        {
            GameObject cardGO = Instantiate(characterCardPrefab, cardContainer);

            // Kartın içindeki UI elemanlarını bul ve doldur.
            // İsimleri prefab'dakiyle aynı olmalı.
            Image charIcon = cardGO.transform.Find("CharacterIcon").GetComponent<Image>();
            TextMeshProUGUI charNameText = cardGO.transform.Find("CharacterNameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI charLevelText = cardGO.transform.Find("CharacterLevelText").GetComponent<TextMeshProUGUI>();

            charIcon.sprite = character.baseData.characterIcon;
            charNameText.text = character.baseData.characterName;
            charLevelText.text = "Lvl: " + character.currentLevel;

            // Daha sonra buraya yıldıızları doldurma kodu da gelecek.
        }
    }
}