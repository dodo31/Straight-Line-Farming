using System;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer truckRenderer;

    [SerializeField]
    private Animator truckAnimator;

    [SerializeField]
    private float speed = 1;

    private TruckStates truckState;

    private Vector2 currentStartPosition;
    private Vector2 currentEndPosition;

    public event Action OnTravelUpdated;
    public event Action OnTravelCompleted;

    protected void Awake()
    {
        truckState = TruckStates.HIDDEN;
        currentStartPosition = Vector2.zero;
        currentEndPosition = Vector2.zero;
    }

    protected void FixedUpdate()
    {
        AnimatorStateInfo animatorStateInfo = truckAnimator.GetCurrentAnimatorStateInfo(0);

        switch (truckState)
        {
            case TruckStates.HIDDEN:

                break;
            case TruckStates.SPAWNING:
                if (animatorStateInfo.IsName("TRAVELING"))
                {
                    truckState = TruckStates.TRAVELING;
                }
                break;
            case TruckStates.TRAVELING:
                OnTravelUpdated?.Invoke();

                UpdateTravelPosition();

                float totalTravelDistance = Vector2.Distance(currentStartPosition, currentEndPosition);
                float currentTravelDistance = Vector2.Distance(currentStartPosition, transform.position);

                if (currentTravelDistance >= totalTravelDistance)
                {
                    truckAnimator.SetTrigger("LEAVE");
                    truckState = TruckStates.LEAVING;
                }
                break;
            case TruckStates.LEAVING:
                UpdateTravelPosition();

                if (animatorStateInfo.IsName("IDLE"))
                {
                    OnTravelCompleted?.Invoke();
                    EndTravelRow();
                }
                break;
        }
    }

    private void UpdateTravelPosition()
    {
        Vector2 travelDirection = currentEndPosition - currentStartPosition;

        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + travelDirection * speed;

        transform.position = newPosition;
    }

    public void SowRow(Vector2 startPosition, Vector2 endPosition)
    {
        StartTravelRow(startPosition, endPosition);
    }

    public void CollectRow(Vector2 startPosition, Vector2 endPosition)
    {
        StartTravelRow(startPosition, endPosition);
    }

    private void StartTravelRow(Vector2 startPosition, Vector2 endPosition)
    {
        truckRenderer.enabled = true;

        transform.position = startPosition;

        currentStartPosition = startPosition;
        currentEndPosition = endPosition;

        truckAnimator.SetTrigger("SPAWN");
        truckState = TruckStates.SPAWNING;
    }

    private void EndTravelRow()
    {
        truckRenderer.enabled = false;
        truckState = TruckStates.HIDDEN;
    }
}