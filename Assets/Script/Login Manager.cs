using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


// ��ǥ 1 : ������� ���������� �Է��Ͽ� �����ϰų� (ȸ������) ����� �����͸� �о �������� ���忩�ο� ���� �α��� �ϰ�ʹ�.
// �ʿ�Ӽ� : ID InputField, PWInputField, �����ؽ�Ʈ

// ��ǥ2 : ���̵�� �н����带 �����ؼ� ȸ������ �ϰ�ʹ�.

// ��ǥ 3 : �Է��� ���� ���, �Է��� ä���޶�� �޼����� ����.
public class LoginManager : MonoBehaviour
{
    //�ʿ�Ӽ� : ID InputField, PWInputField, �����ؽ�Ʈ
    public TMP_InputField id;
    public TMP_InputField pw;
    public TMP_Text authTxt;

    void Start()
    {
        authTxt.text = string.Empty;
    }
    // ��ǥ2 : ���̵�� �н����带 �����ؼ� ȸ������ �ϰ�ʹ�.
    public void SginUp()
    {
        //���� �ý����� ������ ���ٸ� ȸ�������� �ϰ�ʹ�.
        if (PlayerPrefs.HasKey(id.text))
        {
            PlayerPrefs.SetString(id.text, pw.text);
            authTxt.text = "ȸ�������� �Ϸ� �Ǿ����ϴ�";
        }

        else
        {
            authTxt.text = "�̹� �����ϴ� ID�Դϴ�.";
        }

        public void LogIn()
        {
            string password = PlayerPrefs.GetString(id.text);

            //��й�ȣ�� ��ġ�ϸ�
            if (password == pw.text)
            {
                //���� ���� �ε� �Ѵ�.
                SceneManager.LoadScene(1);

            }
            else
            {
                authTxt.text = "�Է��Ͻ� ���̵�� �н����尡 ��ġ���� �ʽ��ϴ�.";
            }
            else
            {
                authTxt.text = "���̵� �������� �ʽ��ϴ�. ȸ�������� ���ּ���.";
            }

            // ��ǥ 3 : �Է��� ���� ���, �Է��� ä���޶�� �޼����� ����.
            bool CheakInPut(string _id, string _pw)
            {
                if(_id == "" || _pw == "")
                {
                    authTxt.text = "���̵� �Ǵ� �н����带 �Է��� �ּ���.";

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


     