using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public float lifeTime = 6f;
    public float damage = 10f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
            

        // aquí podrías instanciar un efecto de impacto, sonido, etc.
        Destroy(gameObject);
    }
}
