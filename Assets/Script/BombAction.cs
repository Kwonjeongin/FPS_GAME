using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// ���� : ��ź�� ��ü�� �΋H���� ����Ʈ�� ����� �ı��ȴ�.
//�ʿ�Ӽ� : ���� ����Ʈ

// ����2 : ����ȿ�� �ݰ泻���� ���̾ 'Enemy '�� ��� ���� ������Ʈ�� Collider�� �����Ͽ�,�ش� �� ���� ������Ʈ���� ����ź �������� �ش�.
//�ʿ�Ӽ� : ���� ȿ�� �ݰ� 

public class Bomb : MonoBehaviour
{
    //�ʿ�Ӽ� : ���� ����Ʈ

    public GameObject bombeffect;

    //�ʿ�Ӽ� : ���� ȿ�� �ݰ� 
    public float explosionRadius = 5f;
    //public damage = 5;

    // ���� : ��ź�� ��ü�� �΋H���� �ı��ȴ�.

    private void OnCollisionEnter(Collision collision)
    {
        // ����2 : ����ȿ�� �ݰ泻���� ���̾ 'Enemy '�� ��� ���� ������Ʈ�� Collider�� �����Ͽ�,�ش� �� ���� ������Ʈ���� ����ź �������� �ش�.
        Collider[] cols= Physics.OverlapSphere(transform.position, explosionRadius, 1 << 8 | 6);
        //Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Enemy")| LayerMask.GetMask("Player"));

        //�ش� �� ���� ������Ʈ���� ����ź �������� �ش�.
        for(int i=0; i<cols.Length; i++)
        {
            //cols[i].GetComponent<EnemyFsm>().DamageAction(damage);
        }
        
        //����Ʈ�� �����.
        GameObject bombGo = Instantiate(bombeffect);
        bombGo.transform.position = transform.position;
        
        //����Ʈ ��ġ�� ��(��ź)�� ��ġ�����ش�.
        Destroy(gameObject);
    }
}
