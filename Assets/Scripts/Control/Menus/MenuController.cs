using UnityEngine;

public class MenuController : MonoBehaviour
{
    public Screen MenuScreen;

    protected void Awake()
    {
        MenuScreen.Toggle(true);
    }
}