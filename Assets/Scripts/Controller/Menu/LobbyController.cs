using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

namespace Controller.Menu
{
    class LobbyController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private TMP_InputField m_playerName;
        [SerializeField]
        private GameObject m_quickStartButton = default;
        [SerializeField]
        private GameObject m_quickCancelButton = default;
        [SerializeField]
        private int m_roomSize = 0;

        private void Start()
        {
            m_playerName?.onEndEdit.AddListener(SetPlayerName);
        }

        public void SetPlayerName(string text)
        {
            PhotonNetwork.NickName = m_playerName.text;
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.AutomaticallySyncScene = true; //Make it so whatever scene the master client has loaded is the scene all other clients will load
            m_quickStartButton.SetActive(true);
        }

        public void DelayStart()
        {
            m_quickStartButton.SetActive(false);
            m_quickCancelButton.SetActive(true);
            PhotonNetwork.JoinRandomRoom(); //First tries to join an existing room
            Debug.Log("Quick Start");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to join a room");
            CreateRoom();
        }

        private void CreateRoom()
        {
            Debug.Log("Creating room now");
            int randomRoomNumber = Random.Range(0, 10000); //creating a random name for the room
            RoomOptions roomOpt = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)m_roomSize };
            PhotonNetwork.CreateRoom("Room" + randomRoomNumber, roomOpt); //attempting to create a new room
            Debug.Log(randomRoomNumber);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed to create room... try again!");
            CreateRoom(); //Retrying to create a new room with a different name
        }

        public void DelayCancel()
        {
            m_quickCancelButton.SetActive(false);
            m_quickStartButton.SetActive(true);
            PhotonNetwork.LeaveRoom();
        }
    }
}
