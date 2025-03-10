using UnityEngine;

public class Target : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IWinner winner)) {
            winner.Win();
        }
    }
}
