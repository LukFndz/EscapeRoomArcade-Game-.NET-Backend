using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.Scripts.Api
{
    public class LoginUIController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private InputField _playerNameInput;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _loginButton;
        [SerializeField] private Text _messageText;
        #endregion

        #region Monobehaviour Functions
        private void Start()
        {
            _createButton.onClick.AddListener(OnCreateClicked);
            _loginButton.onClick.AddListener(OnLoginClicked);

            //StartCoroutine(AutoLoginRoutine());
        }
        #endregion

        #region Private Functions
        private System.Collections.IEnumerator AutoLoginRoutine()
        {
            yield return null; // wait a frame
            LoginManager.Instance.TryAutoLogin((ok, resp) =>
            {
                if (ok)
                {
                    SetMessage("Auto-login successful");
                    OnLoginSuccess();
                }
                else
                {
                    SetMessage("Please login or create an account.");
                }
            });
        }

        private void OnCreateClicked()
        {
            var name = _playerNameInput.text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                SetMessage("Enter a name.");
                return;
            }

            LoginManager.Instance.CreateAndLogin(name, (ok, resp) =>
            {
                if (ok)
                {
                    SetMessage("Account created. Logged in.");
                    OnLoginSuccess();
                }
                else
                {
                    SetMessage($"Create failed: {resp}");
                }
            });
        }

        private void OnLoginClicked()
        {
            var name = _playerNameInput.text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                SetMessage("Enter a name.");
                return;
            }

            LoginManager.Instance.Login(name, (ok, resp) =>
            {
                if (ok)
                {
                    SetMessage("Logged in.");
                    OnLoginSuccess();
                }
                else
                {
                    SetMessage($"Login failed: {resp}");
                }
            });
        }

        private void OnLoginSuccess()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }

        private void SetMessage(string msg)
        {
            if (_messageText != null) _messageText.text = msg;
            Debug.Log(msg);
        }
        #endregion
    }
}