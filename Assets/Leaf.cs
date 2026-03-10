using UnityEngine;

public class Leaf : MonoBehaviour
{
    public float lifeTime = 4f;

    public void Activate()
    {
        CancelInvoke(); // Önceki deactivate çağrısı varsa iptal et
        Invoke(nameof(Deactivate), lifeTime);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
