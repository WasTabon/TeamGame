using UnityEngine;

public class WalletController : MonoBehaviour
{
    public static WalletController Instance;

    public int skillPoints;

    private void Awake()
    {
        Instance = this;
    }
}
