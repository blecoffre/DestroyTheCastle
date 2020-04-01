using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class CustomSceneManager
    {
        private static object m_dataToPass = null;
        public static void LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single, object dataToPassBetweenScenes = null)
        {
            if(sceneName != SceneManager.GetActiveScene().name)
                SceneManager.LoadSceneAsync(sceneName, mode);

            if (dataToPassBetweenScenes != null)
                m_dataToPass = dataToPassBetweenScenes;
        }

        public static void GetData(ref object Data)
        {
            if (m_dataToPass != null)
            {
                Data = m_dataToPass;
                m_dataToPass = null;
            }
        }

        public static void LoadHomeScene()
        {
            LoadSceneAsync(SceneName.Home);
        }
    }
}

