using UnityEngine;
using VContainer;

namespace Failsafe.Items
{
    public class StasisGun : IUsable, IUpdatable, IAltUsable
    {
        StasisGunData _data;
        EnergyContainer _energyContainer;
        private bool _isDefaultMode = true;
        float _fireRateTimer = 0;

        [Inject] Camera _playerCam;

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


        public ItemUseResult Use()
        {
            Shoot(Raycast());
            return new ItemUseResult { ItemStateAfterUse = ItemState.Hold, UsageType = UsageType.ClickToUse };
        }

        public ItemUseResult AltUse()
        {
            ChangeMode();
            return new ItemUseResult { ItemStateAfterUse = ItemState.Hold, UsageType = UsageType.ClickToUse };
        }

        public void ChangeMode()
        {
            _isDefaultMode = !_isDefaultMode;
            Debug.Log("Default mode is " + _isDefaultMode);
        }


        public void Shoot(RaycastHit hit)
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

        RaycastHit Raycast()
        {
            Ray ray = _playerCam.ScreenPointToRay(Input.mousePosition);
            //маска чтобы рейкаст точно игнорировал игрока
            LayerMask mask = ~(1 << 5);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                Debug.Log("Object ahead: " + hit.collider.name);
            }
            else
            {

                Debug.Log("No Object!");
            }
            return hit;
        }


    }

}
