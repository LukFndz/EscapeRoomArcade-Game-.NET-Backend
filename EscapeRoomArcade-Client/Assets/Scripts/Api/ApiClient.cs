using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Api
{
    public class ApiClient : MonoBehaviour
    {
        [SerializeField] private string _baseUrl = "https://localhost:7273";

        public static ApiClient Instance { get; private set; }

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

        // ------------------------- USER -------------------------

        public IEnumerator CreateUser(string playerName, System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/user/create";
            var json = $"{{\"playerName\":\"{EscapeJson(playerName)}\"}}";
            yield return PostJson(url, json, callback);
        }

        public IEnumerator Login(string playerName, System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/user/login/{UnityWebRequest.EscapeURL(playerName)}";
            using var req = UnityWebRequest.Get(url);
            req.SetRequestHeader("Content-Type", "application/json");

            var op = req.SendWebRequest();
            while (!op.isDone) yield return null;

#if UNITY_2020_1_OR_NEWER
            bool isError = req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError;
#else
            bool isError = req.isNetworkError || req.isHttpError;
#endif

            string txt = req.downloadHandler.text;
            callback(!isError, isError ? (txt != "" ? txt : req.error) : txt);
        }

        // ------------------------- GAME END / LEADERBOARD -------------------------

        public IEnumerator EndRun(string jsonPayload, System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/game/end";
            yield return PostJson(url, jsonPayload, callback);
        }

        public IEnumerator GetLeaderboard(System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/game/leaderboard";
            using var req = UnityWebRequest.Get(url);
            req.SetRequestHeader("Content-Type", "application/json");

            var op = req.SendWebRequest();
            while (!op.isDone) yield return null;

#if UNITY_2020_1_OR_NEWER
            bool isError = req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError;
#else
            bool isError = req.isNetworkError || req.isHttpError;
#endif

            string txt = req.downloadHandler.text;
            callback(!isError, isError ? (txt != "" ? txt : req.error) : txt);
        }

        public IEnumerator GetRank(string playerName, System.Action<bool, string> callback)
        {
            var url = $"{_baseUrl}/api/user/rank/{UnityWebRequest.EscapeURL(playerName)}";
            using var req = UnityWebRequest.Get(url);
            req.SetRequestHeader("Content-Type", "application/json");

            var op = req.SendWebRequest();
            while (!op.isDone) yield return null;

#if UNITY_2020_1_OR_NEWER
            bool isError = req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError;
#else
            bool isError = req.isNetworkError || req.isHttpError;
#endif

            string txt = req.downloadHandler.text;
            callback(!isError, isError ? (txt != "" ? txt : req.error) : txt);
        }

        // ------------------------- INTERNAL -------------------------

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
            bool isError = req.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError;
#else
            bool isError = req.isNetworkError || req.isHttpError;
#endif

            string txt = req.downloadHandler.text;
            callback(!isError, isError ? (txt != "" ? txt : req.error) : txt);
        }

        private static string EscapeJson(string s) => s.Replace("\"", "\\\"");
    }
}
