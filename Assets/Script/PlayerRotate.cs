using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocate : MonoBehaviour
{ // 필요속성 : 마우스 입력 X , Y, 속도
    public float speed = 200f;
    void Update()
    {
        // 목적 7 :GameManager가 Ready 상태일때는 플레이어와 적이 움직일 수 없도록 한다. 
        if (GameManager.Instance.state != GameManager.Gamestate.start)
            return;

        // 순서1 사용자의 마우스의 입력(X,Y)을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        
        // 순서2 마우스의 입력에 따라 방향을 설정한다.
        Vector3 dir = new Vector3(0, mouseX, 0);

        // 순서3 물체를 회전 시킨다. 
        // r =r0 +vt
        transform.eulerAngles = transform.eulerAngles + dir * speed * Time.deltaTime;
    }
}
