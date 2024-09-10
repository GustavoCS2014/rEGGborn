using EditorAttributes;
using TMPro;
using Reggborn.UI;
using UnityEngine;


public class FadingText : MonoBehaviour, IUserInterface
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float fadeDuration;
    [SerializeField] private bool startOff;
    [SerializeField, ReadOnly] private float activeAlpha;
    private bool _showing;

    private void Awake()
    {
        activeAlpha = text.alpha;
        text.color = SetAlpha(text.color, startOff ? 0 : text.alpha);
    }
    public void Close()
    {
        if (!_showing) return;
        text.alpha = 0;
        // text.DOFade(0, fadeDuration).SetEase(Ease.OutSine);
        _showing = false;
    }

    public void Show()
    {
        if (_showing) return;
        text.alpha = 1;
        // text.DOFade(activeAlpha, fadeDuration).SetEase(Ease.OutSine);
        _showing = true;
    }


    private Color SetAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}