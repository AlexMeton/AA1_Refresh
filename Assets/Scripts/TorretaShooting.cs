using UnityEngine;
using UnityEngine.InputSystem;

public class TorretaShooting : MonoBehaviour
{
    [Header("Referencias")]
    public Transform spawnPoint;          // Asigna el mismo que usa la torreta
    public GameObject projectilePrefab;   // Prefab con Rigidbody + CapsuleCollider

    [Header("Físicas")]
    [Min(0f)] public float launchSpeed = 50f;  // m/s aprox
    public float spawnForwardOffset = 0.06f;   // evita chocar con la torreta

    private PlayerControls _controls;

    void Awake()
    {
        // Fallback por si te olvidas de arrastrarlo en el inspector
        if (spawnPoint == null)
            spawnPoint = GetComponent<TorretaControl>()?.spawnPoint;

        _controls = new PlayerControls();
    }

    void OnEnable()
    {
        _controls.Player.Enable();
        // Dispara al realizar la acción "Fire" (puedes añadir <Keyboard>/space en el asset)
        _controls.Player.Fire.performed += OnFireAction;
    }

    void OnDisable()
    {
        _controls.Player.Fire.performed -= OnFireAction;
        _controls.Player.Disable();
    }

    private void OnFireAction(InputAction.CallbackContext ctx)
    {
        Fire();
    }

    public void Fire()
    {
        if (spawnPoint == null || projectilePrefab == null)
        {
            Debug.LogWarning("Falta spawnPoint o projectilePrefab en TorretaShooting.");
            return;
        }

        // Instancia un poco por delante para evitar autocolisión con la torreta
        Vector3 pos = spawnPoint.position + spawnPoint.forward * spawnForwardOffset;
        Quaternion rot = spawnPoint.rotation;

        GameObject proj = Instantiate(projectilePrefab, pos, rot);

        if (!proj.TryGetComponent<Rigidbody>(out var rb))
        {
            Debug.LogError("El prefab de proyectil no tiene Rigidbody en la raíz.");
            return;
        }

        // Resetea por si el prefab trae valores residuales
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Opción A: velocidad directa (clara y determinista)
        rb.linearVelocity = spawnPoint.forward * launchSpeed;

              Debug.Log("🔥 Proyectil disparado!");
    }
}
