using UnityEngine;

public class FollowHead : MonoBehaviour
{
    public Transform transformToFollow;

    private void LateUpdate()
    {
        transform.position = transformToFollow.position;
    }
}