using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EventCameraEffects : MonoBehaviour
{
    [Tooltip("The ID of the event to trigger camera effects.")]
    public int eventID = 0;

    [Tooltip("The amount of time the camera will shake.")]
    public float shakeDuration = 0.5f;
    [Tooltip("The intensity of the camera shake.")]
    public float shakeIntensity = 0.05f;

    private Transform target;
    private Vector3 targetOffset;

    private bool isFollowingTarget = false;
    [Tooltip("The speed at which the camera will move to the target.")]
    public float followSpeed = 5f;

    [Tooltip("The position to move the camera to when this event is triggered.")]
    public Vector3 eventPosition = Vector3.zero;
    [Tooltip("The amount of time it will take for the camera to move to the event position.")]
    public float moveToEventDuration = 1f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetOffset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (isFollowingTarget)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + targetOffset, followSpeed * Time.deltaTime);
        }
    }

    public void TriggerCameraEffects(int id)
    {
        if (id == eventID)
        {
            StartCoroutine(ShakeCamera());
            StartCoroutine(MoveToEventPosition());
        }
    }

    private IEnumerator ShakeCamera()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;

            transform.position += randomOffset;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position -= transform.position - (target.position + targetOffset);
    }

    private IEnumerator MoveToEventPosition()
    {
        isFollowingTarget = false;

        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < moveToEventDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, eventPosition, elapsedTime / moveToEventDuration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = eventPosition;
        isFollowingTarget = true;
    }
}