using System.Collections.Generic;
using UnityEngine;

public class FXPool : MonoBehaviour
{
    [Header("FX Settings")]
    public GameObject fxPrefab;
    public int poolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fx = Instantiate(fxPrefab);
            fx.SetActive(false);
            pool.Enqueue(fx);
        }
    }

    public void PlayFX(Vector3 position, Quaternion rotation)
    {
        GameObject fx = pool.Count > 0 ? pool.Dequeue() : Instantiate(fxPrefab, transform);

        fx.transform.SetPositionAndRotation(position, rotation);
        fx.SetActive(true);

        StartCoroutine(ReturnToPool(fx, 1f)); // Adjust duration as needed
    }

    private System.Collections.IEnumerator ReturnToPool(GameObject fx, float delay)
    {
        yield return new WaitForSeconds(delay);
        fx.SetActive(false);
        pool.Enqueue(fx);
    }
}