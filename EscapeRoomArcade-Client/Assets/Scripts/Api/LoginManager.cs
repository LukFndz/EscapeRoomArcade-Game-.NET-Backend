using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Api
{
    public class LoginManager : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("Key used in PlayerPrefs to store last logged player name.")]
        private string _prefsKey = "player_name";
        #endregion

        #region Properties
        public static LoginManager Instance { get; private set; }
        public string CurrentPlayerName { get; private set; } = string.Empty;
        #endregion

        #region Monobehaviour Functions
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (PlayerPrefs.HasKey(_prefsKey))
                CurrentPlayerName = PlayerPrefs.GetString(_prefsKey);
        }
        #endregion

        #region Public Functions
        public void TryAutoLogin(System.Action<bool, string> callback)
        {
            if (string.IsNullOrEmpty(CurrentPlayerName))
            {
                callback(false, "No saved player");
                return;
            }

            StartCoroutine(ApiClient.Instance.Login(CurrentPlayerName, (ok, response) =>
            {
                if (ok)
                    callback(true, response);
                else
                    callback(false, response);
            }));
        }

        public void CreateAndLogin(string playerName, System.Action<bool, string> callback)
        {
            StartCoroutine(ApiClient.Instance.CreateUser(playerName, (ok, response) =>
            {
                if (!ok)
                {
                    callback(false, response);
                    return;
                }

                SavePlayer(playerName);
                callback(true, response);
            }));
        }

        public void Login(string playerName, System.Action<bool, string> callback)
        {
            StartCoroutine(ApiClient.Instance.Login(playerName, (ok, response) =>
            {
                if (!ok)
                {
                    callback(false, response);
                    return;
                }

                SavePlayer(playerName);
                callback(true, response);
            }));
        }
        #endregion

        #region Private Functions
        private void SavePlayer(string playerName)
        {
            CurrentPlayerName = playerName;
            PlayerPrefs.SetString(_prefsKey, playerName);
            PlayerPrefs.Save();
        }
        #endregion
    }
}