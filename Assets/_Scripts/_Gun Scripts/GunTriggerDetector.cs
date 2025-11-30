using UnityEngine;

public class GunTriggerDetector : MonoBehaviour
{
    // public variables
    public GunPickupHandler pickupHandler;
    public string equipSocketName;
    public GameObject socket;





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            pickupHandler = other.GetComponent<GunPickupHandler>();
            if (pickupHandler != null)
            {
                pickupHandler.SetNearbyGun(this.gameObject);
                //socket=GameObject.FindWithTag(equipSocketName);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            pickupHandler = other.GetComponent<GunPickupHandler>();
            if (pickupHandler != null)
            {
                pickupHandler.ClearNearbyGun();
                //socket=null;
            }

            pickupHandler = null;   
        }
    }
}