using UnityEngine;

public class TruckController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer truckRenderer;

    [SerializeField]
    private Animator truckAnimator;

    private TruckStates truckState;

    protected void Awake()
    {
        truckState = TruckStates.HIDDEN;
    }

    protected void Update()
    {
        switch (truckState)
        {
            case TruckStates.HIDDEN:

                break;
            case TruckStates.SPAWNING:
                
                break;
            case TruckStates.TRAVELING:

                break;
            case TruckStates.LEAVING:

                break;
        }
    }

    public void PlantRow(Vector2 startPosition, Vector2 endPosition)
    {
        TravelRow(startPosition, endPosition);
    }

    public void CollectRow(Vector2 startPosition, Vector2 endPosition)
    {
        TravelRow(startPosition, endPosition);
    }

    private void TravelRow(Vector2 startPosition, Vector2 endPosition)
    {
        truckAnimator.SetTrigger("SPAWN");
        truckState = TruckStates.SPAWNING;
    }
}