using Failsafe.Scripts.Modifiebles;
using Failsafe.Scripts.EffectSystem;
using Failsafe.PlayerMovements;

namespace Failsafe.Items
{
    public class Adrenaline : StimulatorBaseItem
    {

        public Adrenaline(AdrenalineData data, PlayerMovementParameters playerMovementParameters, IEffectManager effectManager) : base(effectManager)
        {
            Effect = new AdrenalineEffect(playerMovementParameters, data);
        }



        //TODO: Вынести в отдельный файл если эффект будет переиспользоваться другими предметами
        public class AdrenalineEffect : StimulatorBaseEffect
        {
            private readonly PlayerMovementParameters _playerMovementParameters;


            public AdrenalineEffect(PlayerMovementParameters playerMovementParameters, AdrenalineData data) : base(data)
            {
                _playerMovementParameters = playerMovementParameters;
            }

            public override void ApplyEffect()
            {
                //происходит именно УМНОЖЕНИЕ
                _playerMovementParameters.WalkSpeed.AddModificator(Modificator);
                _playerMovementParameters.RunSpeed.AddModificator(Modificator);
                _playerMovementParameters.CrouchSpeed.AddModificator(Modificator);
            }

            public override void ClearEffect()
            {
                _playerMovementParameters.WalkSpeed.RemoveModificator(Modificator);
                _playerMovementParameters.RunSpeed.RemoveModificator(Modificator);
                _playerMovementParameters.CrouchSpeed.RemoveModificator(Modificator);
            }
        }
    }
}
