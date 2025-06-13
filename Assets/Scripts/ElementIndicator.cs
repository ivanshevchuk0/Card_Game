using UnityEngine;

public class ElementIndicator : MonoBehaviour
{
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite earthSprite;
    
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetElement(ElementType element)
    {
        sr.sprite = element switch {
            ElementType.Fire => fireSprite,
            ElementType.Water => waterSprite,
            ElementType.Earth => earthSprite,
            _ => null
        };
        
        sr.color = element switch {
            ElementType.Fire => Color.red,
            ElementType.Water => Color.blue,
            ElementType.Earth => Color.green,
            _ => Color.white
        };
        
        gameObject.SetActive(sr.sprite != null);
    }
}