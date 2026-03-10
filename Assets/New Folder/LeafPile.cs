using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LeafPile : MonoBehaviour
{
    public GameObject leafPrefab1;
    public GameObject leafPrefab2;
    public int leafCount = 200;          // Toplam yaprak sayısı
    public float radius = 2f;            // Pile yarıçapı
    public float heightOffset = 0.02f;   // Yere gömülmesin
    public bool autoRegenerate = false;  // Editor'de değişince yeniden üret

    // Yoğunluk eğrisi → ortada yoğun, kenarda seyrek
    public AnimationCurve densityCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    public void GenerateLeaves()
    {
        // Eski yaprakları temizle
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);

        for (int i = 0; i < leafCount; i++)
        {
            // 1. Rastgele nokta
            Vector2 random = Random.insideUnitCircle;
            float dist01 = random.magnitude;     // 0–1 arası uzaklık
            float density = densityCurve.Evaluate(dist01);

            // 2. Kenarlarda çok seyrek yapmak için:
            if (Random.value > density)
                continue;

            Vector3 pos = new Vector3(random.x, 0, random.y) * radius;
            pos += transform.position;
            pos.y += heightOffset;

            // 3. Rastgele rotasyon
            Quaternion rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            // 4. Leaf spawnla (child olarak)
            if(i % 2 == 0)
                Instantiate(leafPrefab1, pos, rot, transform);
            else
                Instantiate(leafPrefab2, pos, rot, transform);
        }
    }

    public void CombineLeavesMultiMaterial()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        // İki ayrı liste
        List<CombineInstance> combines1 = new List<CombineInstance>();
        List<CombineInstance> combines2 = new List<CombineInstance>();

        // Materyaller
        Material mat1 = leafPrefab1.GetComponent<MeshRenderer>().sharedMaterial;
        Material mat2 = leafPrefab2.GetComponent<MeshRenderer>().sharedMaterial;

        foreach (var mf in meshFilters)
        {
            if (mf.transform == this.transform) continue;

            var rend = mf.GetComponent<MeshRenderer>();
            CombineInstance ci = new CombineInstance();
            ci.mesh = mf.sharedMesh;
            ci.transform = mf.transform.localToWorldMatrix;

            if (rend.sharedMaterial == mat1)
                combines1.Add(ci);
            else if (rend.sharedMaterial == mat2)
                combines2.Add(ci);
        }

        // Eskileri temizle
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);

        // Mesh 1
        if (combines1.Count > 0)
        {
            GameObject go1 = new GameObject("Leaves_Mat1");
            go1.transform.SetParent(transform);
            go1.transform.localPosition = Vector3.zero;
            go1.transform.localRotation = Quaternion.identity;

            MeshFilter mf1 = go1.AddComponent<MeshFilter>();
            MeshRenderer mr1 = go1.AddComponent<MeshRenderer>();
            Mesh mesh1 = new Mesh();
            mesh1.CombineMeshes(combines1.ToArray(), true, true);
            mf1.sharedMesh = mesh1;
            mr1.sharedMaterial = mat1;
        }

        // Mesh 2
        if (combines2.Count > 0)
        {
            GameObject go2 = new GameObject("Leaves_Mat2");
            go2.transform.SetParent(transform);
            go2.transform.localPosition = Vector3.zero;
            go2.transform.localRotation = Quaternion.identity;

            MeshFilter mf2 = go2.AddComponent<MeshFilter>();
            MeshRenderer mr2 = go2.AddComponent<MeshRenderer>();
            Mesh mesh2 = new Mesh();
            mesh2.CombineMeshes(combines2.ToArray(), true, true);
            mf2.sharedMesh = mesh2;
            mr2.sharedMaterial = mat2;
        }

        Debug.Log("LeafPile → Combined into 2 meshes (2 materials)!");
    }
}
