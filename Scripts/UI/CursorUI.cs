using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CursorUI : MonoBehaviour
{
    public Grabber grabber;

    Image img;

    Vector3 originalCursorScale;
    Sprite originalCursor;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    void Start()
    {
        originalCursor = img.sprite;
        originalCursorScale = img.rectTransform.localScale;

        grabber.onUseItem += (heldItem) => { ResetCursor(); };
    }

    // Update is called once per frame
    void Update()
    {
        var heldItem = grabber.GetHeldItem();
        if (heldItem != null)
        {
            img.sprite = heldItem.ItemSprite;
            img.rectTransform.localScale = originalCursorScale * 30.0f;
        }
    }

    public void ResetCursor()
    {
        img.sprite = originalCursor;
        img.rectTransform.localScale = originalCursorScale;
    }
}
