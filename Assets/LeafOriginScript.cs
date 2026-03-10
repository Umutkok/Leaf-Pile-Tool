using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;

public class LeafOriginScript : MonoBehaviour
{
    public ParticleSystem leafParticle;
    
    //public GameObject leafPrefab;
    public float spawnOffset = 0.05f;
    private Transform target;    // Player transformu
    public bool isFollowing = false;

    private Vector3 oppositeMoveDir = Vector3.zero; // Son frame’de aldığın finalMove'un tam tersi

    void Update()
    {
        if (isFollowing && target != null)
        {
            // LeafOrigin karakteri izler
            transform.position = target.position;
        }


        transform.rotation= Quaternion.LookRotation(oppositeMoveDir);
    }

    // LeafPileScript çağırır
    public void StartFollowing(Transform followTarget)
    {
        target = followTarget;
        isFollowing = true;

        if (leafParticle != null)
            leafParticle.Play();
    }

    public void StopFollowing()
    {
        isFollowing = false;
        target = null;

        if (leafParticle != null)
            leafParticle.Stop();
    }

    public void SetOppositeMove(Vector3 move)
    {
        oppositeMoveDir = -move;  // tam tersi
    }
    void OnParticleCollision(GameObject other)
    {
        // Çarpan partiküllerin pozisyonlarını al
        List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
        int count = leafParticle.GetCollisionEvents(other, events);

        for (int i = 0; i < count; i++)
        {
            Vector3 hitPos = events[i].intersection;

            // Hafif yukarı offset ekle
            hitPos.y += spawnOffset;
            // Rastgele açı ver
            float RandomOffset = Random.Range(0f, 360f);
            Quaternion rot = Quaternion.Euler(0f, RandomOffset, 0f);

            LeafPool.Instance.GetLeaf(hitPos, rot);
        }
    }
}