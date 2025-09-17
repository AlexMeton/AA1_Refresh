using UnityEngine;
using UnityEngine.InputSystem; // << importante: Input System

public class TorretaControl : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivotBase;       // Empty que rota en Y (si está vacío, se usa el objeto donde está el script)
    public Transform pivotSuperior;   // Empty que rota en Z (elevación)
    public Transform spawnPoint;      // punto de salida proyectiles (opcional)

    [Header("Sensibilidad (grados por píxel)")]
    public float sensibilidadX = 0.15f;
    public float sensibilidadY = 0.15f;

    [Header("Restricciones (grados, eje Z del pivotSuperior)")]
    public float minAnguloZ = -20f;
    public float maxAnguloZ = 45f;

    [Header("Opciones")]
    public bool requiereBotonDerechoParaRotar = false; // si true, rota solo mientras mantienes botón derecho

    // valores acumulados (en rango -180..180)
    private float rotY;
    private float rotZ;

    void Start()
    {
        if (pivotBase == null) pivotBase = transform;
        if (pivotSuperior == null) Debug.LogWarning("pivotSuperior no asignado en TorretaControl.");

        // Inicializar acumuladores con las rotaciones actuales (normalizadas a -180..180)
        rotY = pivotBase.localEulerAngles.y;
        rotZ = pivotSuperior.localEulerAngles.z;
    }

    void Update()
    {
        // Protección: si no hay ratón no hacemos nada
        if (Mouse.current == null) return;

        // si hemos requerido botón derecho para rotar
        if (requiereBotonDerechoParaRotar && !Mouse.current.rightButton.isPressed) return;

        // delta de ratón (píxeles desde la última frame)
        Vector2 delta = Mouse.current.delta.ReadValue();

        // Aplicar sensibilidad (grados por pixel)
        rotY += delta.x * sensibilidadX;
        rotZ -= delta.y * sensibilidadY; // restamos para que mover el ratón hacia arriba suba la torreta

        // Limitamos la elevación
        rotZ = Mathf.Clamp(rotZ, minAnguloZ, maxAnguloZ);

        // Aplicar las rotaciones manteniendo los otros ejes como estaban
        Vector3 baseAngles = pivotBase.localEulerAngles;
        pivotBase.localEulerAngles = new Vector3(baseAngles.x, rotY, baseAngles.z);

        if (pivotSuperior != null)
        {
            Vector3 supAngles = pivotSuperior.localEulerAngles;
            pivotSuperior.localEulerAngles = new Vector3(supAngles.x, supAngles.y, rotZ);
        }
    }

    // Normaliza un ángulo de 0..360 a -180..180
    private float NormalizeAngle(float angle)
    {
        return Mathf.Repeat(angle + 180f, 360f) - 180f;
    }
}
