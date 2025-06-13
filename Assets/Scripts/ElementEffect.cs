using UnityEngine;

public class ElementEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;
    
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}