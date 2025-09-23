using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Target Settings")]
    public bool isDestructible = true; // Si se destruye o no
    public int points = 100; // Puntos a sumar o restar
    public GameObject brokenPrefab; // Solo si es destructible
    public GameObject wholeTarget;

    private bool isBroken = false;
    private GameObject brokenInstance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (isDestructible)
            {
                Break();
            }
            else
            {
                ScoreManager.Instance.AddScore(points); // negativo en obstáculo
            }

            Destroy(other.gameObject); // destruir la bala
        }
    }

    private void Break()
    {
        if (isBroken) return;

        // Desactivar el target completo
        wholeTarget.SetActive(false);

        // Instanciar el target roto
        brokenInstance = Instantiate(brokenPrefab, transform.position, transform.rotation);

        // Sumar puntuación
        ScoreManager.Instance.AddScore(points);

        // Marcar como roto
        isBroken = true;

        // Reactivar después de 3s
        Invoke(nameof(ResetTarget), 3f);
    }

    private void ResetTarget()
    {
        if (brokenInstance != null) Destroy(brokenInstance);
        wholeTarget.SetActive(true);
        isBroken = false;
    }
}
