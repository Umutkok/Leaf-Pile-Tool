using System.Collections.Generic;
using UnityEngine;

public class LeafPool : MonoBehaviour
{
    public static LeafPool Instance;

    public GameObject YellowleafPrefab;
    public GameObject RedleafPrefab;
    public int poolSize = 50;

    private List<GameObject> pool;

    void Awake()
    {
        Instance = this;

        // Pool
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            //yellow
            GameObject YellowObj = Instantiate(YellowleafPrefab);
            YellowObj.SetActive(false);
            pool.Add(YellowObj);
            //red
            GameObject RedObj = Instantiate(RedleafPrefab);
            RedObj.SetActive(false);
            pool.Add(RedObj);
        }
    }

    // Pooldan kullanılabilir leaf al
    public GameObject GetLeaf(Vector3 position, Quaternion rotation)
    {
        foreach (var leaf in pool)
        {
            if (!leaf.activeInHierarchy)
            {
                leaf.transform.position = position;
                leaf.transform.rotation = rotation;
                leaf.SetActive(true);

                // Kendini belli süre sonra deactivate et
                leaf.GetComponent<Leaf>().Activate();

                return leaf;
            }
        }

        // Eğer poolda yoksa, opsiyonel olarak yeni oluşturabilirsin
        GameObject newLeaf = Instantiate(YellowleafPrefab, position, rotation);
        pool.Add(newLeaf);
        newLeaf.GetComponent<Leaf>().Activate();
        return newLeaf;
    }
}
