using System.Collections;
using UnityEngine;

public class Stasisable : MonoBehaviour
{
    private Rigidbody _rb;
    private Coroutine _startedCoroutine;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    public void StartStasis(float duration)
    {
        _startedCoroutine ??= StartCoroutine(FreezeRigidbody(duration));
    }
    public void StartStasisWithInertion(float duration)
    {
        _startedCoroutine ??= StartCoroutine(FreezeRigidbodyWithInertion(duration));
    }

    private IEnumerator FreezeRigidbody(float duration)
    {
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints.FreezeAll;

        yield return new WaitForSeconds(duration);

        _rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints.None;
        _startedCoroutine = null;

    }

    IEnumerator FreezeRigidbodyWithInertion(float duration)
    {
        _rb.useGravity = false;
        //_rb.linearDamping = 1;
        yield return new WaitForSeconds(duration);
        _rb.useGravity = true;
        _rb.isKinematic = false;
        _rb.constraints = RigidbodyConstraints.None;
        //_rb.linearDamping = 0;
        _startedCoroutine = null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_startedCoroutine != null)
        {
            _rb.isKinematic = true;
            _rb.constraints = RigidbodyConstraints.FreezeAll;

        }
    }
}
