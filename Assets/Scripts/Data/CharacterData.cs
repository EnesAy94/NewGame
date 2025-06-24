using UnityEngine;

// Bu enum'ları daha sonra yetenek sisteminde kullanacağız.
public enum AbilityTarget { Self, Enemy, AllEnemies, AllAllies }

[CreateAssetMenu(fileName = "New Character", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Temel Bilgiler")]
    public string characterName;
    public Sprite characterFullArt; // Tam boy karakter görseli
    public Sprite characterIcon;    // Küçük ikon
    [Range(1, 5)] public int starRating; // 1-5 arası yıldız

    [Header("Temel İstatistikler (1. Seviye Değerleri)")]
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    
    [Header("Varsayılan Silah")]
    public WeaponData defaultWeapon; // Her karakterin başlangıç silahı

    [Header("Özel Yetenek (Adrenalin)")]
    public string adrenalineAbilityName;
    [TextArea] public string adrenalineAbilityDescription;
    public int adrenalineCost; // Kullanmak için gereken adrenalin puanı
    // Yeteneğin ne yapacağını (hasar, iyileştirme vb.) daha sonra ekleyeceğiz.

    [Header("Lider Yeteneği (Pasif)")]
    public string leaderAbilityName;
    [TextArea] public string leaderAbilityDescription;
    // Lider yeteneğinin bonuslarını da daha sonra ekleyeceğiz.
}