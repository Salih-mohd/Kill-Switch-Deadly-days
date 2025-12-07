using System.Collections;
using Photon.Pun;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public string botPrefabName = "Bot";  
    private void Start()
    {
         
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnBots());
        }
    }

    IEnumerator SpawnBots()
    {
        while (true)
        {
            //Debug.Log("Spawning bot");
            int pos = Random.Range(0, spawnPoints.Length);
            PhotonNetwork.Instantiate(botPrefabName, spawnPoints[pos].position, Quaternion.identity);

            yield return new WaitForSeconds(40);
        }              
        
    }
}