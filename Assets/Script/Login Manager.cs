using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


// 목표 1 : 사용자의 개인정보를 입력하여 저장하거나 (회원가입) 저장된 데이터를 읽어서 개인정보 저장여부에 따라 로그인 하고싶다.
// 필요속성 : ID InputField, PWInputField, 인증텍스트

// 목표2 : 아이디와 패스워드를 저장해서 회원가입 하고싶다.

// 목표 3 : 입력이 없을 경우, 입력을 채워달라는 메세지를 띄운다.
public class LoginManager : MonoBehaviour
{
    //필요속성 : ID InputField, PWInputField, 인증텍스트
    public TMP_InputField id;
    public TMP_InputField pw;
    public TMP_Text authTxt;

    void Start()
    {
        authTxt.text = string.Empty;
    }
    // 목표2 : 아이디와 패스워드를 저장해서 회원가입 하고싶다.
    public void SginUp()
    {
        //만일 시스템의 정보가 없다면 회원가입을 하고싶다.
        if (PlayerPrefs.HasKey(id.text))
        {
            PlayerPrefs.SetString(id.text, pw.text);
            authTxt.text = "회원가입이 완료 되었습니다";
        }

        else
        {
            authTxt.text = "이미 존재하는 ID입니다.";
        }

        public void LogIn()
        {
            string password = PlayerPrefs.GetString(id.text);

            //비밀번호가 일치하면
            if (password == pw.text)
            {
                //다음 씬을 로드 한다.
                SceneManager.LoadScene(1);

            }
            else
            {
                authTxt.text = "입력하신 아이디와 패스워드가 일치하지 않습니다.";
            }
            else
            {
                authTxt.text = "아이디가 존재하지 않습니다. 회원가입을 해주세요.";
            }

            // 목표 3 : 입력이 없을 경우, 입력을 채워달라는 메세지를 띄운다.
            bool CheakInPut(string _id, string _pw)
            {
                if(_id == "" || _pw == "")
                {
                    authTxt.text = "아이디 또는 패스워드를 입력해 주세요.";

                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}


     