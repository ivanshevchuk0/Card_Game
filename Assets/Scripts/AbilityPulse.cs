using UnityEngine;

public class AbilityPulse : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    
    private SpriteRenderer sr;
    private float timer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime * pulseSpeed;
        float scale = Mathf.Lerp(minScale, maxScale, Mathf.PingPong(timer, 1));
        transform.localScale = new Vector3(scale, scale, 1);
    }
}