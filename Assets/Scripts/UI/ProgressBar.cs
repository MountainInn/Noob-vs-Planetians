using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;
using System;
using UniRx;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] protected bool negateRatio;
    [Space]
    [SerializeField] protected Sprite borderSprite;
    [SerializeField] protected Sprite maskSprite;
    [SerializeField] protected Sprite fillSprite;
    [SerializeField] protected float underFillDelay;
    [Space]
    [SerializeField] [Range(0,1f)] protected float fillAmount;
    [SerializeField] protected float pixelsPerUnit;
    [SerializeField] protected Color borderColor;
    [SerializeField] protected Color fillColor;
    [Space]
    [SerializeField] protected Slider slider;
    [SerializeField] protected Image borderImage;
    [SerializeField] protected Image maskImage;
    [SerializeField] protected Image fillImage;
    [Space]
    [SerializeField] protected TextMeshProUGUI label;
    [Header("Afterimage")]
    [SerializeField] protected ProgressBar afterimage;
    [SerializeField] protected Sprite afterimageSprite;
    [SerializeField] protected Color afterimageColor;

    Volume volume;

    protected Queue<Tween> queue = new();

    protected void OnValidate()
    {

        if (slider)
        {
            slider.value = fillAmount;
        }

        if (afterimage)
        {
            afterimage.fillSprite = afterimageSprite;
            afterimage.pixelsPerUnit = pixelsPerUnit;
            afterimage.fillColor = afterimageColor;
            afterimage.borderColor = borderColor;
            afterimage.fillAmount = fillAmount + .1f;
            afterimage.maskSprite = maskSprite;
            afterimage.borderSprite = borderSprite;

            afterimage.OnValidate();
        }

        if (fillImage)
        {
            fillImage.sprite = fillSprite;
            fillImage.pixelsPerUnitMultiplier = pixelsPerUnit;
            fillImage.color = fillColor;
            fillImage.type = Image.Type.Sliced;
        }

        if (maskImage)
        {
            maskImage.sprite = maskSprite;
            maskImage.pixelsPerUnitMultiplier = pixelsPerUnit;
            maskImage.type = Image.Type.Sliced;
        }

        if (borderImage)
        {
            borderImage.sprite = borderSprite;
            borderImage.pixelsPerUnitMultiplier = pixelsPerUnit;
            borderImage.color = borderColor;
            borderImage.type = Image.Type.Sliced;
        }
    }

    public IDisposable Subscribe(GameObject volumeOwner, Volume volume)
    {
        return
            volume
            .ObserveRatio()
            .TakeWhile(_ => volumeOwner != null && volumeOwner.activeSelf)
            .Subscribe(ratio =>
            {
                if (negateRatio)
                    ratio = (1f - ratio);

                if (label)
                    label.text = volume.ToString();

                if (afterimage)
                    QueueTween(afterimage.slider.DOValue(ratio, underFillDelay));

                QueueTween(slider.DOValue(ratio, underFillDelay));
            });
    }

    public void SetFull()
    {
        slider.value = 1f;
    }

    // public void SetVolume(Volume volume)
    // {
    //     this.volume = volume;
    // }

    // void Update()
    // {
    //     if (volume == null
    //         || float.IsNaN(volume.Ratio))
    //         return;

    //     if (label)
    //         label.text = volume.ToString();

    //     /// TODO: Add Volume.HasChanged to update slider only when needed
    //     slider.value = volume.Ratio;

    //     if (afterimage != null)
    //     {
    //         var tween = afterimage.slider.DOValue(volume.Ratio, underFillDelay);

    //         QueueTween(tween);
    //     }
    // }

    protected void QueueTween(Tween tween)
    {
        tween
            .Pause()
            .OnKill(() =>
            {
                queue.Dequeue();

                if (queue.TryPeek(out Tween next)
                    && next != null)
                {
                    next.Play();
                }
            });

        queue.Enqueue(tween);

        if (queue.TryPeek(out Tween next)
            && next == tween)
        {
            tween.Play();
        }
    }
}
