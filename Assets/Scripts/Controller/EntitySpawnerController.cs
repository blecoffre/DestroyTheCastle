using Manager;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace Controller
{
    public class EntitySpawnerController : MonoBehaviour, IOnEventCallback
    {
        [SerializeField]
        private Transform m_leftSpawn;
        public Vector3 LeftSpawnPosition => m_leftSpawn.position;
        [SerializeField]
        private Transform m_rightSpawn;
        public Vector3 RightSpawnPosition => m_rightSpawn.position;

        [SerializeField]
        private Transform m_unitContainer;

        private readonly byte SpawnArcher = 1;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void SpawnBaseSoldier()
        {
            SpawnEntity(SpawnableEntity.Archer, 0);
            SendSpawnEvent(SpawnableEntity.AdversaryArcher, 0);
        }

        public void SpawnEntity(SpawnableEntity entityToSpawn, int entityLevel)
        {
            EntitysManager.SpawnEntity(entityToSpawn, entityLevel, m_leftSpawn.position);
        }

        private void SendSpawnEvent(SpawnableEntity entityToSpawn, int entityLevel)
        {
            byte evCode = SpawnArcher;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            SendOptions sendOptions = new SendOptions { Reliability = true };
            PhotonNetwork.RaiseEvent(evCode, entityToSpawn, raiseEventOptions, sendOptions);
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == SpawnArcher)
            {
                int entityType = (int)photonEvent.CustomData;

                EntitysManager.SpawnEntity((SpawnableEntity)entityType, 0, m_rightSpawn.position);
            }
        }
    }
}

