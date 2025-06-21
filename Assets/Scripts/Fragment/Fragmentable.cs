using UnityEngine;

public class Fragmentable : MonoBehaviour
{
    [SerializeField] private Transform fragmentedPrefab;

    public void Fragment() 
    {
        Transform fragmentedClone = Instantiate(fragmentedPrefab, transform.position, transform.rotation);

        // foreach (Transform child in fragmentedClone) {
        //     child.SetParent(null);
        // }

        // Destroy(fragmentedClone.gameObject);
        Destroy(gameObject);
    }
}