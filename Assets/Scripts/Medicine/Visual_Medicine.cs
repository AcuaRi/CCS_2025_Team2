using UnityEngine;

public class Visual_Medicine : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color visual_Medicine_Color = Color.red;

    void Awake()
    {
        _spriteRenderer.color = visual_Medicine_Color;
    }

    public void SetColor(Color color)
    {
        this.visual_Medicine_Color = color;
        _spriteRenderer.color = visual_Medicine_Color;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        _spriteRenderer.color = visual_Medicine_Color;
    }
#endif
}