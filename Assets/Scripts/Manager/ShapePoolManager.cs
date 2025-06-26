using UnityEngine;
using System.Collections.Generic;

public class ShapePoolManager : MonoBehaviour
{
    public static ShapePoolManager Instance;

    public GameObject shapePrefab;
    public GameObject tilePrefab;
    public Transform poolParent;
    public int initialPoolSize = 10;

    private Queue<BlockShape> pool = new Queue<BlockShape>();

    void Awake()
    {
        Instance = this;
        FillPool();
    }

    void FillPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            var obj = Instantiate(shapePrefab, poolParent);
            obj.SetActive(false);
            var shape = obj.GetComponent<BlockShape>();
            shape.tilePrefab = tilePrefab;
            pool.Enqueue(shape);
        }
    }

    public BlockShape GetShape(Transform parent)
    {
        if (pool.Count == 0)
            FillPool();

        var shape = pool.Dequeue();
        shape.isPlaced = false;
        shape.transform.SetParent(parent, false);
        shape.gameObject.SetActive(true);
        return shape;
    }

    public void ReturnShape(BlockShape shape)
    {
        shape.gameObject.SetActive(false);
        shape.transform.SetParent(poolParent, false);
        pool.Enqueue(shape);
    }
}