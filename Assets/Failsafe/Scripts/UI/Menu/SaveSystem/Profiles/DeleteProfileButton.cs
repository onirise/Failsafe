using UnityEngine;
using Zenject;

public class DeleteProfileButton : BaseConfirmCallButton
{
    [Inject] ProfilesHandler profilesHandler;
    [Inject] TabletHandler tabletHandler;
    public void DeleteProfile()
    {
        Profile thisProfile = gameObject.GetComponentInParent<Profile>();
        profilesHandler.RemoveFromProfilesList(thisProfile);
        if(profilesHandler.GetSelectedProfile() == null || thisProfile.DATA.selected == true)
            tabletHandler.playButton.SetPlayButtonInteractable(false);
        Destroy(thisProfile.gameObject);

    }

    public override void funcToListen()
    {
        DeleteProfile();
        
    }
}
