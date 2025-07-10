using UnityEngine;

namespace Failsafe.Items
{
    public class StasisGun : IUsable, IUpdatable, ITargetable, IAltUsable
    {
        StasisGunData _data;
        EnergyContainer _energyContainer;
        private bool _isDefaultMode = true;
        float _fireRateTimer = 0;

        public StasisGun(StasisGunData data)
        {
            _data = data;
            _energyContainer = new EnergyContainer(_data);
        }

        public void Update()
        {
            if (_fireRateTimer > 0)
            {
                _fireRateTimer -= Time.deltaTime;

            }
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                _isDefaultMode = !_isDefaultMode;
                Debug.Log("Default mode is " + _isDefaultMode);
            }

        }


        public void Use()
        {

        }

        public void AltUse()
        {
            ChangeMode();
        }

        public void ChangeMode()
        {
            _isDefaultMode = !_isDefaultMode;
            Debug.Log("Default mode is " + _isDefaultMode);
        }


        public void TargetAction(RaycastHit hit)
        {
            if (_fireRateTimer <= 0 && !_energyContainer.IsEmpty())
            {
                _fireRateTimer = _data.FireRate;
                _energyContainer.UseChargeAmount();
                if (hit.collider != null)
                {
                    if (_isDefaultMode)
                        DefaultMode(hit);
                    else
                        AltMode(hit);
                }

            }



        }

        void DefaultMode(RaycastHit hit)
        {
            if (hit.collider.GetComponent<Stasisable>() != null)
            {
                hit.collider.GetComponent<Stasisable>().StartStasis(_data.StasisDuration);
            }
            else if (hit.collider.GetComponentInParent<Enemy>() != null)
            {
                hit.collider.GetComponentInParent<Enemy>().DisableState();
            }
        }

        void AltMode(RaycastHit hit)
        {
            if (hit.collider.GetComponent<Stasisable>() != null)
            {
                hit.collider.GetComponent<Stasisable>().StartStasisWithInertion(_data.StasisDuration);
            }
            else if (hit.collider.GetComponentInParent<Enemy>() != null)
            {
                hit.collider.GetComponentInParent<Enemy>().DisableState();
            }
        }


    }

}
