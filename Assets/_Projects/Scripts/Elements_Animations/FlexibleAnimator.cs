using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlexibleAnimator : MonoBehaviour
{
    // Enum for selecting animation type in Inspector
    public enum AnimationType
    {
        None,
        AnimateSprite,
        Floating,
        LeftRight,
        Both
    }

    [Header("General Settings")]
    public AnimationType animationType; // Select animation type in the Inspector

    [Header("Sprite Animation Settings")]
    public Image imageComponent;         // Reference to the UI Image component
    public Sprite[] sprites;             // Array of sprites for animation frames
    public float animationDuration = 1f; // Total animation time for one loop

    [Header("Floating Animation Settings")]
    public Transform floatTarget;        // Reference to the Transform to float
    public float floatDistance = 10f;    // Distance to float on Y-axis
    public float floatDuration = 1f;     // Time for one up-and-down float cycle
    
    [Header("LeftRight Animation Settings")]
    public Transform leftRightTarget;        // Reference to the Transform to float
    public float leftRightDistance = 10f;    // Distance to float on Y-axis
    public float leftRightDuration = 1f;     // Time for one up-and-down float cycle

    private void Start()
    {
        // Check animation type and start animations accordingly
        if (animationType == AnimationType.AnimateSprite || animationType == AnimationType.Both)
        {
            AnimateSprites();
        }

        if (animationType == AnimationType.Floating || animationType == AnimationType.Both)
        {
            AnimateFloating();
        }
        
        if (animationType == AnimationType.LeftRight || animationType == AnimationType.Both)
        {
            AnimateLeftWrite();
        }
    }

    private void AnimateSprites()
    {
        if (sprites.Length == 0 || imageComponent == null) return;

        // Calculate frame interval based on number of sprites and animation duration
        float frameDuration = animationDuration / sprites.Length;

        // Create a sequence for sprite animation with infinite looping
        Sequence spriteSequence = DOTween.Sequence();
        foreach (Sprite sprite in sprites)
        {
            spriteSequence.AppendCallback(() => imageComponent.sprite = sprite);
            spriteSequence.AppendInterval(frameDuration);
        }
        spriteSequence.SetLoops(-1); // Loop the sprite sequence indefinitely
    }

    private void AnimateFloating()
    {
        if (floatTarget == null) return;

        // Apply a floating animation on the Y-axis using a sequence
        floatTarget.DOMoveY(floatTarget.position.y + floatDistance, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // Move up and down in a Yoyo loop
    }

    private void AnimateLeftWrite()
    {
        if (leftRightTarget == null) return;

        // Apply a rotation animation on the Z-axis using a sequence
        leftRightTarget.DORotate(new Vector3(0, 0, leftRightDistance), leftRightDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // Rotate back and forth in a Yoyo loop
    }
}