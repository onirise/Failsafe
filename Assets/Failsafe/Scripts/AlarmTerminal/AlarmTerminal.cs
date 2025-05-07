using System;
using System.Collections;
using UnityEngine;

public class AlarmTerminal : MonoBehaviour
{
    public event Action AlarmStart;
    public event Action AlarmStop;
    public event Action ReinforcementsCall;

    [SerializeField] private float _reinforcementsCallDelay = 5;
    [SerializeField] private AudioSource _siren;

    private bool _isAlarmStart;
    private Coroutine _reinforcementsCallRoutine;

    public void StartAlarm()
    {
        if (_isAlarmStart)
        {
            return;
        }
        _isAlarmStart = true;
        AlarmStart?.Invoke();
        StopRoutine();
        _reinforcementsCallRoutine = StartCoroutine(CallReinforcements());

        if (_siren != null)
        {
            _siren.Play();
        }
    }

    public void StopAlarm()
    {
        if (_isAlarmStart == false)
        {
            return;
        }
        _isAlarmStart = false;
        AlarmStop?.Invoke();
        StopRoutine();

        if (_siren != null)
        {
            _siren.Stop();
        }
    }

    private IEnumerator CallReinforcements()
    {
        yield return new WaitForSeconds(_reinforcementsCallDelay);
        ReinforcementsCall?.Invoke();
    }

    private void StopRoutine()
    {
        if (_reinforcementsCallRoutine != null)
        {
            StopCoroutine(_reinforcementsCallRoutine);
        }
    }
}
