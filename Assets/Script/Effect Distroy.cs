using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���� : (��)����Ʈ�� Ư�� �ð��� ������ ���� �ȴ�.
// �ʿ�Ӽ� : ���� �ð� , Ư���ð�
public class EffectDistroy : MonoBehaviour
{
    // �ʿ�Ӽ� : ���� �ð� , Ư���ð�

    public float destroyTime = 1.5f;

    float currentTime;

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
    }
}
