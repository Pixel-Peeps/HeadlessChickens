using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public interface IInteractable : IPunObservable
{
    void Interact(CharacterBase characterBase);
    void InteractionFocus(bool focussed);
}