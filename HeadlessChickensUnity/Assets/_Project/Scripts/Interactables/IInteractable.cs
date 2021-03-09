using Photon.Pun;
using PixelPeeps.HeadlessChickens._Project.Scripts.Character;

public interface IInteractable
{
    void Interact(CharacterBase characterBase, bool willLoop);
    void InteractionFocus(bool focussed, CharacterBase character);
}