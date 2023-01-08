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

        // Debug.Log("Open");

        // binAnimator.SetBool("OPEN_BOOL", true);
        // binAnimator.SetBool("CLOSE_BOOL", false);
        // binAnimator.SetBool("RECEIVE_WASTES_BOOL", false);
    }

    public void Close()
    {
        binAnimator.ResetTrigger("OPEN");
        binAnimator.ResetTrigger("RECEIVE_WASTES");
        binAnimator.SetTrigger("CLOSE");

        // Debug.Log("Close");

        // binAnimator.SetBool("OPEN_BOOL", false);
        // binAnimator.SetBool("CLOSE_BOOL", true);
        // binAnimator.SetBool("RECEIVE_WASTES_BOOL", false);
    }

    public void ReceiveWastes()
    {
        binAnimator.ResetTrigger("OPEN");
        binAnimator.ResetTrigger("CLOSE");
        binAnimator.SetTrigger("RECEIVE_WASTES");

        // Debug.Log("ReceiveWastes");

        // binAnimator.SetBool("OPEN_BOOL", false);
        // binAnimator.SetBool("CLOSE_BOOL", false);
        // binAnimator.SetBool("RECEIVE_WASTES_BOOL", true);
    }
}