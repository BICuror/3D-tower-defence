using UnityEngine;

public sealed class ParticleInsantiator : MonoBehaviour
{
    [SerializeField] private GameObject _particle;

    public void InstantiateParticle(GameObject gameObject)
    {
        Instantiate(_particle, gameObject.transform.position, Quaternion.identity);
    }
}
