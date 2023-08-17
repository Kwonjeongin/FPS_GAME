using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : ���콺 ������ ��ư�� ���� ��ź�� Ư���������� �߻��ϰ� �ʹ�.
// �ʿ�Ӽ� : ��ź ���ӿ�����Ʈ, �߻���ġ, ����
// ����1 ���콺 ������ ��ư�� ������.
// ����2 ��ź ���ӿ�����Ʈ�� �����ϰ� fireposition�� ��ġ��Ų��.
// ����3 ��ź ������Ʈ�� rigibody�� �����ͼ� ī�޶� ����������� ���� ���Ѵ�.
public class PlayerFire : MonoBehaviour
{
    //�ʿ�Ӽ� : ��ź ���ӿ�����Ʈ, �߻���ġ, ����

    public GameObject bobm;
    public GameObject fireposition;
    public float power = 10f;

    void Update()
    {
        //����1 ���콺 ������ ��ư�� ������.

        if(Input.GetMouseButtonDown(1))//���� (0), ������ (1) ,�� (2)
        {
            //����2 ��ź ���ӿ�����Ʈ�� �����ϰ� fireposition�� ��ġ��Ų��.
            GameObject bombGo =Instantiate(bobm);
            bobm.transform.position = fireposition.transform.position;

            // ����3 ��ź ������Ʈ�� rigibody�� �����ͼ� ���� ���Ѵ�.
            Rigidbody rigidbody = bombGo.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);    
            //��ź�� ���� ���ϸ鼭 �� �������� �߻�˴ϴ�.
        }

    }
}
