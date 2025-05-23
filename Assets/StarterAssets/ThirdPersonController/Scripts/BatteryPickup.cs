using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BatteryPickup : MonoBehaviour
{
    public int restoreAmount = 3;

    void Awake()
    {
        var col = GetComponent<Collider>(); col.isTrigger = true;
        var rb = GetComponent<Rigidbody>(); rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // sólo si entra el CharacterController del jugador
        if (!other.GetComponent<CharacterController>()) return;

        // buscamos EnergySystem en toda la jerarquía ascendente
        EnergySystem energy = other.GetComponentInParent<EnergySystem>();
        if (!energy) return;

        energy.Restore(restoreAmount);
        Destroy(gameObject);
    }
}
