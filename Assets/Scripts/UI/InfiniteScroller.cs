using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroller : MonoBehaviour
{
    private RawImage _img;
    [SerializeField] private Vector2 offset;

    private void Start()
    {
        _img = GetComponent<RawImage>();
    }
    private void Update()
    {
        Vector2 pos = _img.uvRect.position + offset * 0.01f;//Time.deltaTime;
        _img.uvRect = new Rect(pos, _img.uvRect.size);
    }
}