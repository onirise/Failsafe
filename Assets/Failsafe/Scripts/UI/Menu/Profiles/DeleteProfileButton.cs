using UnityEngine;
using Zenject;

public class DeleteProfileButton : BaseConfirmCallButton
{
    [Inject] ProfilesHandler profilesHandler;
    public void DeleteProfile()
    {
        Profile thisProfile = gameObject.GetComponentInParent<Profile>();
        profilesHandler.RemoveFromProfilesList(thisProfile);
        Destroy(thisProfile.gameObject);

    }

    public override void funcToListen()
    {
        DeleteProfile();
        
    }
}
