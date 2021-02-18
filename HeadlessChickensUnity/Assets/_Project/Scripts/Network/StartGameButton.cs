using PixelPeeps.HeadlessChickens.GameState;
using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;
using UnityEngine.UI;

namespace PixelPeeps.HeadlessChickens.Network
{
    public class StartGameButton : MonoBehaviour
    {
        private Button thisButton;
        
        private void Start()
        {
            thisButton = gameObject.GetComponent<Button>();
            
            thisButton.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            NetworkManager.Instance.StartGameOnMaster();
        }
    }
}
