using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    public int targetFrameRate = 1000;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}