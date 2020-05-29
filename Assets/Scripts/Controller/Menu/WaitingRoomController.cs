using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using System.Collections;

namespace Controller.Menu
{
    class WaitingRoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private string m_multiplayerSceneName = "GameScene";
        [SerializeField]
        private string m_menuSceneName = "Menu";
        [SerializeField]
        private int m_playerToStart = 0;
        [SerializeField]
        private TextMeshProUGUI m_waitedTime;

        private int m_waitedSeconds = 0;

        private void Start()
        {
            StartCoroutine(UpdateTimer());
        }

        public override void OnDisable()
        {
            StopCoroutine(UpdateTimer());
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            StartGame();
        }

        private void StartGame()
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == m_playerToStart)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel(m_multiplayerSceneName);
            }
        }

        public void DelayCancel()
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(m_menuSceneName);
        }

        private IEnumerator UpdateTimer()
        {
            while (true)
            {
                m_waitedSeconds += 1;
                m_waitedTime?.SetText(m_waitedSeconds.ToString());

                yield return new WaitForSecondsRealtime(1.0f);
            }
        }
    }
}
