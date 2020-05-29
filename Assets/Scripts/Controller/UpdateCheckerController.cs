using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controller
{
    class UpdateCheckerController : MonoBehaviour
    {
        public string MenuSceneName = "Menu";
        public string LabelToDownload = "StoneAgeUnits";

        private List<string> m_contentToUpdate = new List<string>();
        private float m_downloadSize = 0;
        public float CurrentDownloadedSize = 0;

        [SerializeField]
        private Image m_progressBar = default;


        private void Start()
        {
            CheckForUpdatedContent();  
            //LoadMenuScene();

        }

        private void CheckForUpdatedContent()
        {
            Addressables.InitializeAsync().Completed += objects =>
            {
                Addressables.ClearDependencyCacheAsync(LabelToDownload);

                Addressables.CheckForCatalogUpdates().Completed += OnCheckCompleted;
            };
        }

        private void OnCheckCompleted(AsyncOperationHandle<List<string>> handle)
        {
            m_contentToUpdate = handle.Result;

            if (m_contentToUpdate.Count > 0)
            {
                var totalDownloadSizeHandle = GetTotalDownloadSize(m_contentToUpdate);
                totalDownloadSizeHandle.Completed += get =>
                {
                    m_downloadSize = totalDownloadSizeHandle.Result;
                    InitDownload(m_contentToUpdate);
                };
            }
            else
            {
                Addressables.LoadAssetsAsync<GameObject>(LabelToDownload, null);
            }
        }

        private AsyncOperationHandle<long> GetTotalDownloadSize(List<string> catalogs)
        {
            return Addressables.GetDownloadSizeAsync(catalogs);
        }

        private IEnumerator InitDownload(List<string> catalogs)
        {
            foreach(string s in catalogs)
            {
                var handle = Addressables.GetDownloadSizeAsync(catalogs);
                while (!handle.IsDone)
                    yield return new WaitForFixedUpdate();

                float catalogSize = handle.Result;
                float prevPercent = 0;
                var newhandle = Addressables.DownloadDependenciesAsync(catalogs);
                while (!newhandle.IsDone)
                {
                    prevPercent = newhandle.PercentComplete;
                    CurrentDownloadedSize += catalogSize * (newhandle.PercentComplete - prevPercent);

                    m_progressBar.fillAmount = (1 /(m_downloadSize / CurrentDownloadedSize));

                    yield return new WaitForFixedUpdate();
                }
            }

            LoadMenuScene();
        }

        private void LoadMenuScene()
        {
            SceneManager.LoadScene(MenuSceneName);
        }

        //private void Start()
        //{
        //    Addressables.ClearDependencyCacheAsync(LabelToDownload);
        //    Addressables.InitializeAsync().Completed += objects =>
        //    {
        //        Addressables.CheckForCatalogUpdates().Completed += checkforupdates =>
        //        {
        //            if (checkforupdates.Result.Count > 0)
        //                Addressables.UpdateCatalogs().Completed += updates => Load(LabelToDownload);
        //            else
        //                Load(LabelToDownload);
        //        };
        //    };
        //}

        //private void Load(string label)
        //{
        //    Addressables.LoadAssetsAsync<GameObject>(label, null).Completed += objects =>
        //    {
        //        foreach (var go in objects.Result)
        //            Debug.Log($"Addressable Loaded: {go.name}");

        //        LoadMenuScene();
        //    };
        //}


    }
}
