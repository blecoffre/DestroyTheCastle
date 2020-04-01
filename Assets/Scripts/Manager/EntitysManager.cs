using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;
using Controller;

namespace Manager
{
    public class EntitysManager : MonoBehaviour
    {
        public static EntitysManager Instance;

        [SerializeField]
        private AddressableAssetReferencesDB m_addressableAssetReferencesDB;

        [SerializeField]
        public EntitySpawnerController EntitySpawnerController;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityAdressablePath"></param>
        /// <param name="entityLevel"></param>
        /// <param name="isOpponentUnit"></param>
        public static void SpawnEntity(SpawnableEntity entityType, int entityLevel, Vector3 spawnPosition)
        {
            AssetReference toSpawn = null;

            toSpawn = Instance.m_addressableAssetReferencesDB.FindElementWithId((int)entityType);

            if (toSpawn != null)
            {
                Addressables.InitializeAsync().Completed += (instantiate) =>
                {
                    Debug.Log("we're done with init now");
                    Addressables.InstantiateAsync(toSpawn, spawnPosition, Quaternion.identity);
                };
            }
            else
            {
                Debug.Log("Cant find required Entity to spawn : " + entityType.ToString());
            }
        }

        public static void DestroyEntity(GameObject entity)
        {
            //Call anim method etc here before destroying
            Destroy(entity);
        }
    }
}
