using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Security.Cryptography;
using System.Text;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText;
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordCheckInputField;
    public Button ConfirmButton; // 로그인 or 회원가입 버튼튼

}

public class UI_Login : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    [Header("회원가입")]
    public UI_InputFields RegisterInputFields;

    [Header("로그인")]
    public UI_InputFields LoginInputFields;
    private const string SALT = "15000";

    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
        LoginCheck();
    }

    private void ShowResultText(TextMeshProUGUI resultText, string message)
    {
        resultText.text = message;
        resultText.transform.DOScale(1.1f, 0.2f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => {
                resultText.transform.DOScale(1f, 0.2f);
            });
    }

    public void OnClickGoToRegisterButton()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }
    
    // 회원가입입
    public void Register()
    {
        // 1. 아이디 입력을 확인한다.
        string id = RegisterInputFields.IDInputField.text;
        if(string.IsNullOrEmpty(id))
        {
            ShowResultText(RegisterInputFields.ResultText, "아이디를 입력해주세요.");
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string password = RegisterInputFields.PasswordInputField.text;
        if(string.IsNullOrEmpty(password))
        {
            ShowResultText(RegisterInputFields.ResultText, "비밀번호를 입력해주세요.");
            return;
        }

        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다.
        string passwordConfirm = RegisterInputFields.PasswordCheckInputField.text;
        if(string.IsNullOrEmpty(passwordConfirm))
        {
            ShowResultText(RegisterInputFields.ResultText, "비밀번호 확인을 입력해주세요.");
            return;
        }
        else
        {
            if(password.Equals(passwordConfirm))
            {
                ShowResultText(RegisterInputFields.ResultText, "회원가입 되었습니다.");
                // 4. PlayerPrefs를 이용해서 아이디와 비밀번호를 저장한다
                PlayerPrefs.SetString("아이디", id);
                PlayerPrefs.SetString("비밀번호",   Encryption(password + SALT));

                // 5. 로그인 창으로 돌아간다. (이때 아이디는 자동 입력되어 있다.)
                LoginInputFields.IDInputField.text = id;
                OnClickGoToLoginButton();
            }
            else
            {
                ShowResultText(RegisterInputFields.ResultText, "비밀번호와 비밀번호 확인이 다릅니다. 다시 입력해주세요.");
            }
        }
    }

    public void Login()
    {
        string id = LoginInputFields.IDInputField.text;
        string password = LoginInputFields.PasswordInputField.text;

        if(string.IsNullOrEmpty(id))
        {
            ShowResultText(LoginInputFields.ResultText, "아이디를 입력해주세요.");
            return;
        }
        if(string.IsNullOrEmpty(password))
        {
            ShowResultText(LoginInputFields.ResultText, "비밀번호를 입력해주세요.");
            return;
        }
        string savedId = PlayerPrefs.GetString("아이디", "");
        string savedPassword = PlayerPrefs.GetString("비밀번호", "");

        // 아이디 패스워드 비교
        if(id.Equals(savedId) && Encryption(password + SALT).Equals(savedPassword))
        {
            ShowResultText(LoginInputFields.ResultText, "로그인 되었습니다.");
            SceneManager.LoadScene(1);
        }
        else    
        {
            ShowResultText(LoginInputFields.ResultText, "아이디 또는 비밀번호가 틀렸습니다.");
        }
    }

    public void LoginCheck()
    {
        string id = LoginInputFields.IDInputField.text;
        string password = LoginInputFields.PasswordInputField.text;

        LoginInputFields.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password); 
    }

     public string Encryption(string text)
    {
        // 해시 암호화 알고리즘 인스턴스를 생성한다.
        SHA256 sha256 = SHA256.Create();
        // 운영체제 혹은 프로그래밍 언어별로 string 표현하는 방식이 다 다르므로
        // UTF8 버전 바이트로 배열로 바꿔야한다.
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);
        string resultText = string.Empty;
        foreach (byte b in hash)
        {
            // byte를 다시 string으로 바꿔서 이어붙이기
            resultText += b.ToString("X2");
        }
        return resultText;
    }
}


