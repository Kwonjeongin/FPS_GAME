using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : ��ź�� ��ü�� �΋H���� ����Ʈ�� ����� �ı��ȴ�.
//�ʿ�Ӽ� : ���� ����Ʈ

public class Bomb : MonoBehaviour
{
    //�ʿ�Ӽ� : ���� ����Ʈ

    public GameObject bombeffect;

    // ���� : ��ź�� ��ü�� �΋H���� �ı��ȴ�.

    private void OnCollisionEnter(Collision collision)
    {
        //����Ʈ�� �����.
        GameObject bombGo = Instantiate(bombeffect);
        bombGo.transform.position = transform.position;
        
        //����Ʈ ��ġ�� ��(��ź)�� ��ġ�����ش�.
        Destroy(gameObject);
    }
}
