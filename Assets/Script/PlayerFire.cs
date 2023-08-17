using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 목적 : 마우스 오른쪽 버튼을 눌러 폭탄을 특정방향으로 발사하고 싶다.
// 필요속성 : 폭탄 게임오브젝트, 발사위치, 방향
// 순서1 마우스 오른쪽 버튼을 누른다.
// 순서2 폭탄 게임오브젝트를 생성하고 fireposition에 위치시킨다.
// 순서3 폭탄 오브젝트의 rigibody를 가져와서 카메라 정면방향으로 힘을 가한다.
public class PlayerFire : MonoBehaviour
{
    //필요속성 : 폭탄 게임오브젝트, 발사위치, 방향

    public GameObject bobm;
    public GameObject fireposition;
    public float power = 10f;

    void Update()
    {
        //순서1 마우스 오른쪽 버튼을 누른다.

        if(Input.GetMouseButtonDown(1))//외쪽 (0), 오른쪽 (1) ,휠 (2)
        {
            //순서2 폭탄 게임오브젝트를 생성하고 fireposition에 위치시킨다.
            GameObject bombGo =Instantiate(bobm);
            bobm.transform.position = fireposition.transform.position;

            // 순서3 폭탄 오브젝트의 rigibody를 가져와서 힘을 가한다.
            Rigidbody rigidbody = bombGo.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * power, ForceMode.Impulse);    
            //폭탄에 힘을 가하면서 이 방향으로 발사됩니다.
        }

    }
}
