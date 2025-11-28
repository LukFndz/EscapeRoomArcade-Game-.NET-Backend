using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Api

{
    public class ApiClient : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("Base URL of the backend API, e.g. https://localhost:7273")]
        private string _baseUrl = "https://localhost:7273";
        #endregion

        #region Properties
        public static ApiClient Instance { get; private set; }
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
        }
        #endregion

        #region Public Functions
        public IEnumerator CreateUser(string playerName, System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/user/create";
            var formJson = $"{{\"playerName\":\"{EscapeJson(playerName)}\"}}";
            yield return PostJson(url, formJson, callback);
        }

        public IEnumerator Login(string playerName, System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/user/login/{UnityWebRequest.EscapeURL(playerName)}";
            using var req = UnityWebRequest.Get(url);
            req.SetRequestHeader("Content-Type", "application/json");

            var op = req.SendWebRequest();
            while (!op.isDone) yield return null;

#if UNITY_2020_1_OR_NEWER
            bool isError = req.result == UnityWebRequest.Result.ConnectionError ||
                           req.result == UnityWebRequest.Result.ProtocolError;
#else
    bool isError = req.isNetworkError || req.isHttpError;
#endif

            string serverMessage = req.downloadHandler.text;

            if (isError)
            {
                if (!string.IsNullOrEmpty(serverMessage))
                    callback(false, serverMessage);
                else
                    callback(false, req.error);
            }
            else
            {
                callback(true, serverMessage);
            }
        }

        public IEnumerator GetLeaderboard(System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/game/leaderboard";
            using var req = UnityWebRequest.Get(url);
            req.SetRequestHeader("Content-Type", "application/json");

            var op = req.SendWebRequest();
            while (!op.isDone) yield return null;

#if UNITY_2020_1_OR_NEWER
            bool isError = req.result == UnityWebRequest.Result.ConnectionError ||
                           req.result == UnityWebRequest.Result.ProtocolError;
#else
    bool isError = req.isNetworkError || req.isHttpError;
#endif

            string serverMessage = req.downloadHandler.text;

            if (isError)
            {
                if (!string.IsNullOrEmpty(serverMessage))
                    callback(false, serverMessage);
                else
                    callback(false, req.error);
            }
            else
            {
                callback(true, serverMessage);
            }
        }


        public IEnumerator EndRun(string jsonPayload, System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/game/end";
            yield return PostJson(url, jsonPayload, callback);
        }
        #endregion

        #region Private Functions
        private IEnumerator PostJson(string url, string json, System.Action<bool, string> callback)
        {
            var body = Encoding.UTF8.GetBytes(json);
            using var req = new UnityWebRequest(url, "POST");
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            var op = req.SendWebRequest();
            while (!op.isDone) yield return null;

#if UNITY_2020_1_OR_NEWER
            bool isError = req.result == UnityWebRequest.Result.ConnectionError ||
                           req.result == UnityWebRequest.Result.ProtocolError;
#else
    bool isError = req.isNetworkError || req.isHttpError;
#endif

            string serverMessage = req.downloadHandler.text;

            if (isError)
            {
                // If server returned a meaningful message, use it.
                if (!string.IsNullOrEmpty(serverMessage))
                    callback(false, serverMessage);
                else
                    callback(false, req.error);
            }
            else
            {
                callback(true, serverMessage);
            }

        }

        private static string EscapeJson(string s)
        {
            return s.Replace("\"", "\\\"");
        }
        #endregion
    }
}