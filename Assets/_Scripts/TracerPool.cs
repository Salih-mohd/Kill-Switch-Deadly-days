using System.Collections.Generic;
using UnityEngine;

public class TracerPool : MonoBehaviour
{
    [SerializeField] private LineRenderer tracerPrefab;
    [SerializeField] private int poolSize = 20;

    private Queue<LineRenderer> pool = new Queue<LineRenderer>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var tracer = Instantiate(tracerPrefab, transform);
            tracer.gameObject.SetActive(false);
            pool.Enqueue(tracer);
        }
    }

    public LineRenderer GetTracer()
    {
        if (pool.Count > 0)
        {
            var tracer = pool.Dequeue();
            tracer.gameObject.SetActive(true);
            return tracer;
        }
        else
        {
            // Optional: expand pool if needed
            var tracer = Instantiate(tracerPrefab, transform);
            return tracer;
        }
    }

    public void ReturnTracer(LineRenderer tracer)
    {
        tracer.gameObject.SetActive(false);
        pool.Enqueue(tracer);
    }
}