using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public string ammoName; 
    public int amount = 30;

    private void OnTriggerEnter(Collider other)
    {
        Inventory inv = other.GetComponent<Inventory>();
        if (inv != null)
        {
            inv.AddAmmo(ammoName, amount);
            Destroy(gameObject);
        }
    }

}
