using UnityEngine;

public class LeafPileScript : MonoBehaviour
{
    public LeafOriginScript leafOrigin;   // LeafOrigin boş objesi
    public string playerTag = "Player";

    public bool InsidePile = false;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            InsidePile = true;
            leafOrigin.StartFollowing(other.transform);   
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            InsidePile = false;
            leafOrigin.StopFollowing();
        }
    }
}
