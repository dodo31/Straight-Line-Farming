using System;
using UnityEngine;

public class CompostArea : MonoBehaviour
{
    [SerializeField]
    private CompostBin bin;

    [SerializeField]
    private WastesContainer wastesContainer;

    public event Action OnWasteReceivingComplete;

    protected void Start()
    {
        bin.OnWasteReceivingComplete += Handle_OnWasteReceivingComplete;
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