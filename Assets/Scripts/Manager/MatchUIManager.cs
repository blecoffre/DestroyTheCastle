using UnityEngine;
using TMPro;
using Photon.Pun;
using System.Linq;

namespace Manager
{
    class MatchUIManager : MonoBehaviour
    {
        public static MatchUIManager Instance;

        [SerializeField]
        private TextMeshProUGUI m_leftPlayerName;
        [SerializeField]
        private TextMeshProUGUI m_rightPlayerName;

        [SerializeField]
        private Transform m_entityUIContainer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetPlayerName();
        }

        public Transform GetEntityUIContainer()
        {
            return m_entityUIContainer;
        }

        private void SetPlayerName()
        {
            m_leftPlayerName?.SetText(PhotonNetwork.NickName);
            m_rightPlayerName?.SetText(PhotonNetwork.PlayerList.First(x => x.NickName != PhotonNetwork.NickName).NickName);
        }
    }
}
