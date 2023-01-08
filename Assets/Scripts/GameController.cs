using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GridController gridController;

    protected void Update()
    {
        switch (gridController.GridState)
        {
            case GridStates.IDLE:
                gridController.ManageRowSelection();
                break;
        }
    }
}