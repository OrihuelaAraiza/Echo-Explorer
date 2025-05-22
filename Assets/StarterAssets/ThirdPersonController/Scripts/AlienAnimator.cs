using UnityEngine;

/// <summary>
/// Actualiza parámetros 'Speed' y 'Alert' en el Animator.
/// El Animator Controller debe tener esos dos parámetros.
/// </summary>
[RequireComponent(typeof(AlienController))]
public class AlienAnimator : MonoBehaviour
{
    public Animator anim;                 // arrastra aquí el Animator del Armature

    AlienController ctrl;

    readonly int hashSpeed = Animator.StringToHash("Speed");
    readonly int hashAlert = Animator.StringToHash("Alert");

    void Awake()
    {
        ctrl = GetComponent<AlienController>();
        if (!anim) anim = GetComponentInChildren<Animator>();   // ← NUEVO
    }


    void Update()
    {
        if (!anim) return;

        anim.SetFloat(hashSpeed, ctrl.CurrentSpeed);
        anim.SetBool(hashAlert, ctrl.IsChasing);
    }
}
