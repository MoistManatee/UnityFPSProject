using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInputMap;

    [Header("Sway Settings")]
    [SerializeField] private float smooth = 8;

    [SerializeField] private float swayMultiplier = 2;

    private float mouseX;
    private float mouseY;
    private Quaternion rotationX;
    private Quaternion rotationY;
    private Quaternion targetRotation;

    private void Start()
    {
        playerInputMap = new PlayerInput();
        playerInputMap.Enable();
    }

    public void applySway()
    {
        mouseX = playerInputMap.FPS.MouseInput.ReadValue<Vector2>().x * swayMultiplier;
        mouseY = playerInputMap.FPS.MouseInput.ReadValue<Vector2>().y * swayMultiplier;

        rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        rotationY = Quaternion.AngleAxis(-mouseX, Vector3.up);
        targetRotation = rotationX * rotationY;

        //rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}