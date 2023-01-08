using UnityEngine;

public class CompostBin : MonoBehaviour
{
    [SerializeField]
    private Animator binAnimator;

    public void Open()
    {
        binAnimator.ResetTrigger("CLOSE");
        binAnimator.SetTrigger("OPEN");
    }

    public void Close()
    {
        binAnimator.ResetTrigger("OPEN");
        binAnimator.SetTrigger("CLOSE");
    }
    
    public void ReceiveWastes()
    {
        binAnimator.ResetTrigger("OPEN");
        binAnimator.ResetTrigger("CLOSE");
        binAnimator.SetTrigger("RECEIVE_WASTES");
    }
}