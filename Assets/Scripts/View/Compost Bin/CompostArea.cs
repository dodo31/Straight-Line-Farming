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

    public void Refresh(PlantCount[] plantsGarbage)
    {
        if (plantsGarbage.Length > 0)
        {
            RejectWastes();
            OpenBin();

            for (int i = 0; i < plantsGarbage.Length; i++)
            {
                PlantCount wasteCount = plantsGarbage[i];
                AddWaste(i, wasteCount.Type, wasteCount.Count);
            }
        }
        else
        {
            RejectWastes();
        }
    }

    private void AddWaste(int slotIndex, PlantTypes plantType, int wasteCount)
    {
        wastesContainer.AddWaste(slotIndex, plantType, wasteCount);
    }

    private void RejectWastes()
    {
        wastesContainer.RejectWastes();
        bin.Close();
    }

    public void AcceptWastes()
    {
        wastesContainer.AcceptWastes();
        bin.ReceiveWastes();
    }

    private void OpenBin()
    {
        bin.Open();
    }

    private void Handle_OnWasteReceivingComplete()
    {
        OnWasteReceivingComplete?.Invoke();
    }
}