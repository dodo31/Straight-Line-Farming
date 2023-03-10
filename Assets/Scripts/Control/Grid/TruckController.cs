using System;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    [SerializeField]
    private TruckSpritePack spritePack;

    [SerializeField]
    private SpriteRenderer truckRenderer;

    [SerializeField]
    private Animator truckAnimator;

    [SerializeField]
    private float speed = 1;

    private TruckStates truckState;

    private Vector2 previousTravelPosition;

    private Vector2 currentStartPosition;
    private Vector2 currentEndPosition;

    public event Action OnTravelUpdated;
    public event Action OnTravelCompleted;
    public PlantCount[] plantsHarvested;

    protected void Awake()
    {
        truckState = TruckStates.HIDDEN;

        previousTravelPosition = Vector2.zero;

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
                    EndTravelRow();
                }
                break;
        }
    }

    private void UpdateTravelPosition()
    {
        previousTravelPosition = transform.position;

        Vector2 currentPosition = transform.position;
        Vector2 newPosition = currentPosition + TravelDirection * speed;

        transform.position = newPosition;

        OnTravelUpdated?.Invoke();
    }

    public void StartTravelRow(Vector2 startPosition, Vector2 endPosition)
    {
        truckRenderer.enabled = true;
        SoundPlayer.PlaySound(SoundPlayer.SoundType.VROOM);


        Vector2 direction = (endPosition - startPosition).normalized;
        transform.position = startPosition - direction * 0.5f;

        currentStartPosition = transform.position;
        currentEndPosition = endPosition;

        truckAnimator.SetTrigger("SPAWN");
        truckState = TruckStates.SPAWNING;
    }

    private void EndTravelRow()
    {
        truckRenderer.enabled = false;
        truckState = TruckStates.HIDDEN;
        OnTravelCompleted?.Invoke();
    }

    public Directions currentDirection = Directions.UP;

    public void SetDirection(Directions direction)
    {
        currentDirection = direction;
        OrientedTruckSprite orientedTruckSprite = spritePack.GetSprite(direction);
        truckRenderer.sprite = orientedTruckSprite.Sprite;
        truckRenderer.flipX = orientedTruckSprite.Direction > 0;
    }

    public Vector2 TravelDirection
    {
        get
        {
            return (currentEndPosition - currentStartPosition).normalized;
        }
    }

    public Vector2 CurrentTravelPosition { get => transform.position; }
    public Vector2 PreviousTravelPosition { get => previousTravelPosition; }

    public Vector2 CurrentStartPosition { get => currentStartPosition; }
}