using System;
using System.Collections;
using UnityEngine;

public class CompostArea : MonoBehaviour
{
    [SerializeField]
    private CompostBin bin;

    [SerializeField]
    private WastesContainer wastesContainer;

    private CompostStates compostState;

    public event Action OnWasteReceivingComplete;

    protected void Start()
    {
        bin.OnWasteReceivingComplete += Handle_OnWasteReceivingComplete;

        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        // Debug.Log("OK 1");

        OpenBin();
        AddWaste(0, PlantTypes.Wheat, 6);
        AddWaste(1, PlantTypes.Corn, 4);
        AddWaste(2, PlantTypes.Pumpkin, 2);
        AddWaste(3, PlantTypes.Chilli, 3);

        yield return new WaitForSeconds(3);
        // Debug.Log("OK 2");

        AcceptWastes();
        
        yield return new WaitForSeconds(3);
        // Debug.Log("OK 3");
        
        OpenBin();
        AddWaste(0, PlantTypes.Wheat, 6);
        AddWaste(1, PlantTypes.Corn, 4);
        AddWaste(2, PlantTypes.Pumpkin, 2);
        AddWaste(3, PlantTypes.Chilli, 3);
        
        yield return new WaitForSeconds(3);
        
        AcceptWastes();
    }

    protected void Update()
    {
        switch (compostState)
        {
            case CompostStates.CLOSED:

                break;
            case CompostStates.OPENING:

                break;
            case CompostStates.OPEN:

                break;
            case CompostStates.CLOSING:

                break;
        }
    }

    public void AddWaste(int slotIndex, PlantTypes plantType, int wasteCount)
    {
        wastesContainer.AddWaste(slotIndex, plantType, wasteCount);
    }

    public void RejectWastes()
    {
        wastesContainer.RejectWastes();
        bin.Close();
    }

    public void AcceptWastes()
    {
        wastesContainer.AcceptWastes();
        bin.ReceiveWastes();
    }

    public void OpenBin()
    {
        bin.Open();
    }

    private void Handle_OnWasteReceivingComplete()
    {
        OnWasteReceivingComplete?.Invoke();
    }
}