using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{


    public event Action<string,int> AmmoAdded;

    // Dictionary gun name  reserve ammo count
    private Dictionary<string, int> ammoInventory = new Dictionary<string, int>();

    // Add ammo for a gun type
    public void AddAmmo(string ammoName, int amount)
    {
        if (!ammoInventory.ContainsKey(ammoName))
            ammoInventory[ammoName] = 0;

        ammoInventory[ammoName] += amount;
        
        AmmoAdded?.Invoke(ammoName,amount);
    }

    // Get reserve ammo for a gun type
    public int GetReserveAmmo(string gunName)
    {
        //return ammoInventory.ContainsKey(gunName) ? ammoInventory[gunName] : 0;
        var am= ammoInventory.ContainsKey(gunName) ? ammoInventory[gunName] : 0;
        ammoInventory[gunName] = 0;
        return am;


    }

    // Consume ammo when reloading
    public int ConsumeAmmo(string gunName, int amount)
    {
        if (!ammoInventory.ContainsKey(gunName)) return 0;

        int available = ammoInventory[gunName];
        int toConsume = Mathf.Min(available, amount);

        ammoInventory[gunName] -= toConsume;
        return toConsume;
    }


}
