using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject wholeTarget;   // El modelo completo
    [SerializeField] private GameObject brokenPrefab;  // Prefab del target roto

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 3f;

    private GameObject brokenInstance;
    private bool isBroken = false;
    private float timer = 0f;

    private void Update()
    {
        if (isBroken)
        {
            timer += Time.deltaTime;

            if (timer >= respawnTime)
            {
                // Destruir el target roto
                if (brokenInstance != null)
                {
                    Destroy(brokenInstance);
                }

                // Reactivar el modelo completo
                wholeTarget.SetActive(true);

                // Resetear estado
                isBroken = false;
                timer = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile")) // Solo reaccionar a las balas
        {
            Break();
            Destroy(other.gameObject); // Destruir la bala al impactar
        }
    }

    private void Break()
    {
        if (isBroken) return;

        // Desactivar el target completo
        wholeTarget.SetActive(false);

        // Instanciar el target roto en la misma posición y rotación
        brokenInstance = Instantiate(brokenPrefab, transform.position, transform.rotation);

        // Marcar como roto
        isBroken = true;
    }
}
