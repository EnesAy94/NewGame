using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public Sprite weaponIcon;
    
    [Header("Bonus İstatistikler")]
    public int bonusAttack;
    public int bonusDefense;
    public int bonusHealth;
    public float bonusCritChance; // % olarak (örneğin 15.0f -> %15)
}