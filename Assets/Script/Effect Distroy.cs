using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 목적 : (내)이펙트가 특정 시간이 지나면 제거 된다.
// 필요속성 : 현재 시간 , 특정시간

//목적2  : 
public class EffectDistroy : MonoBehaviour
{
    // 필요속성 : 현재 시간 , 특정시간

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
