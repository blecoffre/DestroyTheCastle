using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller.Menu
{
    class RoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private string m_waitingRoomSceneName = "WaitingForOpponentScene";

        public override void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public override void OnJoinedRoom()
        {
            SceneManager.LoadScene(m_waitingRoomSceneName);
        }
    }
}
