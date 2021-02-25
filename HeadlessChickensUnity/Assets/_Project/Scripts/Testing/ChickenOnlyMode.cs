using PixelPeeps.HeadlessChickens.Network;
using UnityEngine;

namespace PixelPeeps.HeadlessChickens.Testing
{
    public class ChickenOnlyMode : MonoBehaviour
    {
        public void OnToggle(bool newValue)
        {
            PlayerAssignmentRPC.Instance.chickenOnlyMode = newValue;
            print("<color=green> chickenOnlyMode: " + PlayerAssignmentRPC.Instance.chickenOnlyMode);
        }
    }
}
