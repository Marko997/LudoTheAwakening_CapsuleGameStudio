using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveArrow : MonoBehaviour
{
    public RectTransform arrowTransform; // Reference to the arrow GameObject
    public RectTransform buttonTransform; // Reference to the button GameObject
    public float moveDuration = 1.5f; // Duration of the movement animation
    public float stopDistance = 25f;

    Vector3 dir;

    private static MoveArrow instance;
    public static MoveArrow Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MoveArrow>();
            }
            if (instance == null)
            {
                Debug.Log("There is no tutorial manager!");
            }
            return instance;
        }
    }

    private void Start()
    {
        //AnimateArrowToButton();
    }

    public void AnimateArrowToButton()
    {
        Vector3 targetPosition = buttonTransform.position;

        // Calculate the direction to the button
        Vector3 directionToButton = (targetPosition - arrowTransform.position).normalized;

        // Calculate the rotation angle to point towards the button (in degrees)
        float angleToButton = Mathf.Atan2(directionToButton.y, directionToButton.x) * Mathf.Rad2Deg;

        // Apply the rotation to the arrow (around the z-axis)
        arrowTransform.rotation = Quaternion.Euler(0f, 0f, angleToButton);

        // Calculate the distance between the arrow and the button
        float distanceToButton = Vector3.Distance(arrowTransform.position, targetPosition);

        // Check if the arrow is close enough to the button to stop the animation
        if (distanceToButton <= stopDistance)
        {
            // Arrow is close to the button, stop the animation
            LeanTween.cancel(arrowTransform.gameObject);
        }
        else
        {
            // Arrow is not close to the button, continue the animation
            // Animate the arrow to move towards the button
            LeanTween.move(arrowTransform.gameObject, targetPosition, moveDuration).setEase(LeanTweenType.easeOutQuad);
        }

    }
    void OnDrawGizmos()
    {
        if (arrowTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(arrowTransform.position, buttonTransform.position);
            Gizmos.DrawWireSphere(buttonTransform.position - new Vector3(0f, 2f, 0f), 3f);
            Gizmos.DrawWireSphere(arrowTransform.position, 3f);
        }
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}
