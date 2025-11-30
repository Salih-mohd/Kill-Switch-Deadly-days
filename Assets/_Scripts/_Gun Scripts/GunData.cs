using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Guns/GunData")]
public class GunData : ScriptableObject
{
    public float fireRate = 0.2f;
    public float damage = 10f;
    public float maxRange = 100f;
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public int maxAmmo = 30;
    public int reserveAmmo = 90;
    public string ammoName;
    public Image gunImg;
}

