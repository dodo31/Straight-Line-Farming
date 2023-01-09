using UnityEngine;
using UnityEngine.UI;

public class Screen : MonoBehaviour
{
    protected Image _screenImage;

    private RectTransform _rectTransform;

    private Canvas _parentCanvas;
    private float _imageRatio;

    protected virtual void Start()
    {
        _screenImage = this.GetComponent<Image>();
        _rectTransform = (RectTransform)transform;

        _parentCanvas = this.GetComponentInParent<Canvas>();

        Rect imageRect = _screenImage.sprite.textureRect;
        _imageRatio = imageRect.width / imageRect.height;
    }

    protected virtual void Update()
    {
        float canvasScale = _parentCanvas.transform.localScale.x;

        float screenWidth = UnityEngine.Screen.width / canvasScale;
        float screenHeight = UnityEngine.Screen.height / canvasScale;

        float screenRatio = screenWidth / screenHeight;

        if (_imageRatio < screenRatio)
        {
            _rectTransform.sizeDelta = new Vector2(screenWidth, screenWidth / _imageRatio);
        }
        else
        {
            _rectTransform.sizeDelta = new Vector2(screenHeight * _imageRatio, screenHeight);
        }
    }

    public void Toggle(bool enable)
    {
        gameObject.SetActive(enable);
    }
}
