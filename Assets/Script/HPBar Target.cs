using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ���� : HP bar�� �չ����� Ÿ���� �չ������� ���Ѵ�.
//�ʿ�Ӽ� : Ÿ��
public class HPBartaget : MonoBehaviour
{
    //�ʿ�Ӽ� : Ÿ��
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = target.forward;
    }
}
