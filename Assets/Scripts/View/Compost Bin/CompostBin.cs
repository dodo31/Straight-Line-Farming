using System;
using UnityEngine;

public class CompostBin : MonoBehaviour
{
    [SerializeField]
    private Animator binAnimator;

    public event Action OnWasteReceivingComplete;

    protected void Update()
    {
        AnimatorStateInfo animatorStateInfo = binAnimator.GetCurrentAnimatorStateInfo(0);

        if (animatorStateInfo.IsName("Closed After Receiving Wastes"))
        {
            OnWasteReceivingComplete?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
        
        binAnimator.ResetTrigger("CLOSE");
        binAnimator.ResetTrigger("RECEIVE_WASTES");
        binAnimator.SetTrigger("OPEN");
    }

    public void Close()
    {
        binAnimator.ResetTrigger("OPEN");
        binAnimator.ResetTrigger("RECEIVE_WASTES");
        binAnimator.SetTrigger("CLOSE");
    }

    public void ReceiveWastes()
    {
        binAnimator.ResetTrigger("OPEN");
        binAnimator.ResetTrigger("CLOSE");
        binAnimator.SetTrigger("RECEIVE_WASTES");
    }
}