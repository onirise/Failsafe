using Cysharp.Threading.Tasks;
using Failsafe.Items;
using System;
using UnityEngine;
using VContainer.Unity;

/// <summary>
/// Использование предметов в руках
/// </summary>
public class PlayerHandsSystem : ITickable
{
    public enum UsingState { None, Start, Using, OnDelay }
    public event Action OnItemStartUsing;
    public UsingState ItemUsingState => _usingState;

    private readonly PlayerHandsContainer _playerHandsContainer;
    private readonly InputHandler _inputHandler;

    private UsingState _usingState = UsingState.None;

    // Задержка после применения предмета, чтобы не спамить использование предметов
    // Примерно должен соответсвовать времени анимаций, но не обязательно
    // Если у разных предметов должны быть разные кулдауны, то вынести это в ItemData
    private float _itemUseDelay = 0.5f;
    // Время использования одного предмета. По сути время анимации использования
    // Нужно чтобы Эффект предмета/визуал/звук сработал в определенный момент анимации, а не сразу при нажатии кнопки
    // Сейчас задается один на всех, нужно будет вынести в предмет и настраивать для каждого свой
    private float _itemUseStartDelay = 0.5f;
    // Пропускать начальную анимацию при повторном применении, скорее всего нужно вынести в параметры предмета или в UseResult
    private bool _skipStartDelay;

    public PlayerHandsSystem(PlayerHandsContainer playerHandsSystem, InputHandler inputHandler)
    {
        _playerHandsContainer = playerHandsSystem;
        _inputHandler = inputHandler;
    }

    public void Tick()
    {
        if (_inputHandler.UseTrigger.IsTriggered && CanUseItemInHand())
        {
            UseItemInHand().Forget();
        }
        if (!_inputHandler.UseTrigger.IsPressed)
        {
            _skipStartDelay = false;
        }
    }

    private bool CanUseItemInHand()
    {
        Debug.Log(_playerHandsContainer.State == PlayerHandsContainer.HandState.EmptyHands ? "Нет предмета в руке"
         : _usingState != UsingState.None ? "Нельзя использовать предмет - " + _usingState
         : "Можно использовать предмет");
        return _playerHandsContainer.State == PlayerHandsContainer.HandState.ItemInHand && _usingState == UsingState.None;
    }

    private async UniTask<ItemUseResult> UseItemInHand()
    {
        if (!_skipStartDelay)
        {
            // В ивент нужно передавать тип предмета, чтобы определить какую анимацию использовать
            // Типы предметов в качестве примера: грана, шприц, пистолет
            OnItemStartUsing?.Invoke();
            _usingState = UsingState.Start;
            await UniTask.Delay(TimeSpan.FromSeconds(_itemUseStartDelay));
        }
        _usingState = UsingState.Using;
        var useResult = _playerHandsContainer.ItemInHand.ItemUsable?.Use() ?? ItemUseResult.Consumed;
        _playerHandsContainer.ItemInHand.ItemObject.Use();

        if (useResult.ItemStateAfterUse == ItemState.Consume)
        {
            _playerHandsContainer.SetItemNull();
        }
        else if (useResult.ItemStateAfterUse == ItemState.Drop)
        {
            _playerHandsContainer.DropItemFromHand();
        }

        if (useResult.UsageType == UsageType.ClickToUse)
        {
            _skipStartDelay = false;
            _inputHandler.UseTrigger.ReleaseTrigger();
            _usingState = UsingState.OnDelay;
            await UniTask.Delay(TimeSpan.FromSeconds(_itemUseDelay));
            _usingState = UsingState.None;
        }
        else if (useResult.UsageType == UsageType.HoldToUse)
        {
            _skipStartDelay = true;
            _usingState = UsingState.None;
        }
        return useResult;
    }

}