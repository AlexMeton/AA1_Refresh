using UnityEngine;
using UnityEngine.InputSystem; // Nuevo Input System

public class TorretaControl : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivotBase;       // Empty que rota en Y (yaw)
    public Transform pivotSuperior;   // Empty que rota en X (pitch)
    public Transform spawnPoint;      // punto de salida proyectiles

    [Header("Sensibilidad (grados por píxel)")]
    public float sensibilidadX = 0.15f; // horizontal
    public float sensibilidadY = 0.15f; // vertical

    [Header("Restricciones (grados, eje X del pivotSuperior)")]
    public float minAnguloX = -20f; // cañón baja
    public float maxAnguloX = 45f;  // cañón sube

    [Header("Opciones")]
    public bool requiereBotonDerechoParaRotar = false;

    // acumuladores
    private float rotY;
    private float rotX;

    void Start()
    {
        if (pivotBase == null) pivotBase = transform;
        if (pivotSuperior == null) Debug.LogWarning("pivotSuperior no asignado en TorretaControl.");

        rotY = NormalizeAngle(pivotBase.localEulerAngles.y);
        rotX = NormalizeAngle(pivotSuperior.localEulerAngles.x);
    }

    void Update()
    {
        if (Mouse.current == null) return;
        if (requiereBotonDerechoParaRotar && !Mouse.current.rightButton.isPressed) return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        // yaw (horizontal, Y)
        rotY += delta.x * sensibilidadX;

        // pitch (vertical, X)
        rotX -= delta.y * sensibilidadY;
        rotX = Mathf.Clamp(rotX, minAnguloX, maxAnguloX);

        // aplicar yaw a pivotBase
        Vector3 baseAngles = pivotBase.localEulerAngles;
        pivotBase.localEulerAngles = new Vector3(baseAngles.x, rotY, baseAngles.z);

        // aplicar pitch a pivotSuperior
        if (pivotSuperior != null)
        {
            Vector3 supAngles = pivotSuperior.localEulerAngles;
            pivotSuperior.localEulerAngles = new Vector3(rotX, supAngles.y, supAngles.z);
        }
    }

    private float NormalizeAngle(float angle)
    {
        return Mathf.Repeat(angle + 180f, 360f) - 180f;
    }
}
