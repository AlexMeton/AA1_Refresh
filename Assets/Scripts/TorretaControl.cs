using UnityEngine;
using UnityEngine.InputSystem; // << importante: Input System

public class TorretaControl : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pivotBase;       // Empty que rota en Y (si est� vac�o, se usa el objeto donde est� el script)
    public Transform pivotSuperior;   // Empty que rota en Z (elevaci�n)
    public Transform spawnPoint;      // punto de salida proyectiles (opcional)

    [Header("Sensibilidad (grados por p�xel)")]
    public float sensibilidadX = 0.15f;
    public float sensibilidadY = 0.15f;

    [Header("Restricciones (grados, eje Z del pivotSuperior)")]
    public float minAnguloZ = -20f;
    public float maxAnguloZ = 45f;

    [Header("Opciones")]
    public bool requiereBotonDerechoParaRotar = false; // si true, rota solo mientras mantienes bot�n derecho

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
        // Protecci�n: si no hay rat�n no hacemos nada
        if (Mouse.current == null) return;

        // si hemos requerido bot�n derecho para rotar
        if (requiereBotonDerechoParaRotar && !Mouse.current.rightButton.isPressed) return;

        // delta de rat�n (p�xeles desde la �ltima frame)
        Vector2 delta = Mouse.current.delta.ReadValue();

        // Aplicar sensibilidad (grados por pixel)
        rotY += delta.x * sensibilidadX;
        rotZ -= delta.y * sensibilidadY; // restamos para que mover el rat�n hacia arriba suba la torreta

        // Limitamos la elevaci�n
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

    // Normaliza un �ngulo de 0..360 a -180..180
    private float NormalizeAngle(float angle)
    {
        return Mathf.Repeat(angle + 180f, 360f) - 180f;
    }
}
