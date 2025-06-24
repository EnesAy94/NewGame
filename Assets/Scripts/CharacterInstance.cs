using UnityEngine; // [System.Serializable] için gerekli

[System.Serializable] // Bu satır, bu sınıfın Unity Inspector'unda görülebilmesini sağlar.
public class CharacterInstance
{
    public CharacterData baseData; // Karakterin şablonu (CD_Rick gibi)

    public int currentLevel;
    public int currentTier; // Aşama (1'den 4'e kadar)

    // Daha sonra silah yükseltme gelince bu WeaponData yerine WeaponInstance olacak.
    public WeaponData equippedWeapon;

    // Constructor: Yeni bir karakter örneği oluşturulduğunda çalışır.
    public CharacterInstance(CharacterData data)
    {
        baseData = data;
        currentLevel = 1;
        currentTier = 1;
        equippedWeapon = data.defaultWeapon;
    }

    // Karakterin o anki toplam istatistiklerini hesaplayan fonksiyonlar.
    // Şimdilik basitçe temel değerleri ve silah bonusunu toplayalım.
    public int GetTotalHealth()
    {
        // Daha sonra seviye ve aşama bonusları da buraya eklenecek.
        return baseData.baseHealth + equippedWeapon.bonusHealth;
    }

    public int GetTotalAttack()
    {
        return baseData.baseAttack + equippedWeapon.bonusAttack;
    }

    public int GetTotalDefense()
    {
        return baseData.baseDefense + equippedWeapon.bonusDefense;
    }
}