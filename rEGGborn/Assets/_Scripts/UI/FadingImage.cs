using Attributes;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;


public class FadingImage : MonoBehaviour, IUserInterface
{
    [SerializeField] private Image image;
    [SerializeField] private float fadeDuration;
    [SerializeField] private bool startOff;
    [SerializeField, ReadOnly] private float activeAlpha;
    private bool _showing;

    private void Awake()
    {
        activeAlpha = image.color.a;
        image.color = SetAlpha(image.color, startOff ? 0 : image.color.a);
    }

    public void Close()
    {
        if (!_showing) return;
        image.DOFade(0, fadeDuration).SetEase(Ease.OutSine);
        _showing = false;
    }

    public void Show()
    {
        if (_showing) return;
        image.DOFade(activeAlpha, fadeDuration).SetEase(Ease.OutSine);
        _showing = true;
    }

    private Color SetAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}