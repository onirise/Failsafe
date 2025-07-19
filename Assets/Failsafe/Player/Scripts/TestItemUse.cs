using Failsafe.Items;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Failsafe.Player
{
    /// <summary>
    /// Используется для тестирования логики предметов без необходимости их спаунить и подбирать
    /// </summary>
    public class TestItemUse : MonoBehaviour
    {
        [Inject] private IEnumerable<IUsable> _items;
        [Inject] private InputHandler _inputHandler;

        [ValueDropdown("_itemNames")]
        public string ItemName;
        public GameObject ItemPrefab;
        public string UseOnKey = "E";

        private string[] _itemNames;
        private KeyCode _useKeyCode;

        List<string> _activeEffects = new List<string>();

        List<ILimitedEffect> _activeEffects1 = new List<ILimitedEffect>();
        bool _allowToAltUse = true;

        [ContextMenu(nameof(TestUse))]
        public void TestUse()
        {
            string itemName = SelectItem();


            if (string.IsNullOrEmpty(itemName))
            {
                Debug.Log($"({nameof(TestItemUse)}) Не задан предмет для теста");
                return;
            }
            var item = _items.FirstOrDefault(x => x.GetType().Name == itemName);
            if (item == null)
            {
                Debug.Log($"({nameof(TestItemUse)}) Не найдена реализация для {itemName}");
                return;
            }
            Debug.Log($"({nameof(TestItemUse)}) был использован {itemName}");

            if (item is IShootable shootable)
                shootable.Shoot(RaycastUniversal());


            if (item is ILimitedEffect limitedEffect)
            {
                if (!_activeEffects1.Contains(limitedEffect))
                {

                    _activeEffects1.Add(limitedEffect);
                    StartCoroutine(limitedEffect.EndEffect(() => { _activeEffects1.Remove(limitedEffect); }));
                }
                else
                {
                    Debug.Log("Not yet!");
                    return;
                }


            }

            item.Use();

        }

        void Start()
        {
            _itemNames = _items.Select(x => x.GetType().Name).Concat(new string[1] { "" }).ToArray();
            //пока вписываю конкретный итем чтобы сразу можно было тестить как только запустился
            ItemName = "Adrenaline";
        }

        void Update()
        {
            Debug.Log("Активных эффектов " + _activeEffects1.Count);
            if (_activeEffects1.Count > 0)
            {
                Debug.Log(_activeEffects1[0].GetType());
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
                ItemName = "Stimpack";
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ItemName = "Adrenaline";
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ItemName = "StasisGun";
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                ItemName = "Tushkan";
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                ItemName = "Gorilla";

            if (_items.FirstOrDefault(x => x.GetType().Name == SelectItem()) is IUpdatable updatable) updatable.Update();

            if (_inputHandler.ZoomTriggered && _allowToAltUse && _items.FirstOrDefault(x => x.GetType().Name == SelectItem()) is IAltUsable altUsable)
            {
                _allowToAltUse = false;
                altUsable.AltUse();
            }
            else if (!_inputHandler.ZoomTriggered)
                _allowToAltUse = true;


            if (Input.GetKeyDown(_useKeyCode))
            {
                TestUse();
            }

        }

        void OnValidate()
        {
            _useKeyCode = System.Enum.Parse<KeyCode>(UseOnKey);
        }

        string SelectItem()
        {
            if (ItemPrefab)
            {
                return ItemPrefab.name;
            }

            return ItemName;


        }

        RaycastHit RaycastUniversal()
        {
            Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
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
