using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour, IAmmoObserver
{
    [SerializeField] TextMeshProUGUI ammoText;
    private void Start()
    {
        ammoText = GetComponent<TextMeshProUGUI>();
    }
    public void UpdateAmmo(int ammoCount, int maxAmmo)
    {
        ammoText.text = ($"{ammoCount}/{maxAmmo}");

    }
}


public interface IAmmoObserver
{
    void UpdateAmmo(int ammoCount, int maxAmmo);
}