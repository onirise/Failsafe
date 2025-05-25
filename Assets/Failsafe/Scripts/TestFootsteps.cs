using UnityEngine;
using FMODUnity;

[RequireComponent(typeof(CharacterController))]
public class SimpleFootsteps : MonoBehaviour
{
    [Tooltip("Перетащите сюда FMOD Event из FMOD Browser")]
    public EventReference footstepEvent;    // <-- новое поле вместо строки

    [Tooltip("Интервал между шагами (сек)")]
    public float stepInterval = 0.4f;

    private CharacterController cc;
    private float stepTimer;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        stepTimer = 0f;
    }

    void Update()
    {
        Vector3 horizontalVel = new Vector3(cc.velocity.x, 0, cc.velocity.z);
        if (horizontalVel.magnitude > 0.1f && cc.isGrounded)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                // теперь используем EventReference
                RuntimeManager.PlayOneShot(footstepEvent, transform.position);
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }
}