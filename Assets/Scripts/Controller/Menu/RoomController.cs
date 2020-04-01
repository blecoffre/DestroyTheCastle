using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controller.Menu
{
    class RoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private int m_waitingRoomSceneIndex = 0;

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
            SceneManager.LoadScene(m_waitingRoomSceneIndex);
        }
    }
}
