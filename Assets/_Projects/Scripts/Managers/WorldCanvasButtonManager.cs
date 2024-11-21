using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WorldCanvasButtonManager : MonoBehaviour
{
    [Header("Button Settings")]
    public List<GameObject> buttons; // List of all buttons
    public float activationDelay = 0.2f; // Delay between each button activation
    public float animationDuration = 0.5f; // Duration for scale animation

    private List<GameObject> inactiveButtons = new List<GameObject>();
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();

    private void Start()
    {
        // Deactivate all buttons initially, store original scales, and populate inactive list
        foreach (var button in buttons)
        {
            originalScales[button] = button.transform.localScale; // Store original scale
            button.SetActive(false); // Ensure all buttons start inactive
            inactiveButtons.Add(button);
        }
    }

    public void BringButtonForward()
    {
        // Start activating buttons randomly
        StartCoroutine(ActivateButtonsRandomly());
    }
    
    private IEnumerator ActivateButtonsRandomly()
    {
        while (inactiveButtons.Count > 0)
        {
            // Select a random button from the inactive list
            int randomIndex = Random.Range(0, inactiveButtons.Count);
            GameObject button = inactiveButtons[randomIndex];
            inactiveButtons.RemoveAt(randomIndex); // Remove from inactive list

            // Activate the button with animation
            ActivateButton(button);

            // Wait for the next activation
            yield return new WaitForSeconds(activationDelay);
        }
    }

    private void ActivateButton(GameObject button)
    {
        // Set the button active
        button.SetActive(true);

        // Animate scale-in with DoTween
        button.transform.localScale = Vector3.zero; // Reset scale
        button.transform.DOScale(originalScales[button], animationDuration).SetEase(Ease.OutBack);
    }
}
