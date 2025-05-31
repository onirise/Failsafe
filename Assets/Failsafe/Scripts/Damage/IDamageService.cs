namespace Failsafe.Scripts.Damage
{
    public interface IDamageService
    {
        void Provide(IDamage damage);
        
        void Register(IDamageProvider provider);
        void Unregister(IDamageProvider provider);
    }
}