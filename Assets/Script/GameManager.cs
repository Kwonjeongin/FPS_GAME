using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ���� : ������ ����(Ready, start, Game Over)�� �����ϰ�, ������ ���۰� ���� TEXTUI�� ǥ���ϰ� �ʹ�.
// �ʿ�Ӽ� : ���ӻ���, ������ ����,TextUI

// ����2 : 2���� ������ ���� �ȴ� .Ready ���¿��� Start���·� ����Ǹ� ������ ���۵ȴ�.

// ���� 3 : �÷��̾��� Hp�� 0���� ������ ���� �ؽ�Ʈ�� GAmeOver�� �ٲ��ְ� ���¶��� GameOver�ιٲ��ش�.
// �ʿ�Ӽ�3 : hp�� �� �ִ� playerMove

//���� 4 :  �÷��̾��� hp�� 0���϶��, �÷��̾��� �ִϸ��̼��� �����.
//�ʿ�Ӽ� : �÷��̾��� �ִϸ����� ������Ʈ
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // �ʿ�Ӽ� :���ӻ���, ������ ����,TextUI

    public enum Gamestate
    {
        Ready, 
        start, 
        GameOver,
        Start
    }

    public Gamestate state = Gamestate.Ready;
    public TMP_Text stateText;

    // �ʿ�Ӽ�3 : hp�� �� �ִ� playerMove
    PlayerMove player;

    //�ʿ�Ӽ� : �÷��̾��� �ִϸ����� ������Ʈ
    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stateText.text = "Ready";
        stateText.color = new Color32 (225, 185, 0, 255);

        StartCoroutine(GameStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        animator = player.GetComponentInChildren<Animator>();
    }
    // ����2 : 2���� ������ ���� �ȴ� .Ready ���¿��� Start���·� ����Ǹ� ������ ���۵ȴ�.
    IEnumerator GameStart()
    {
        //2�ʸ� ��ٸ���.
        yield return new WaitForSeconds(2f);

        stateText.text = "Game Start";

        //0.5�ʸ� ��ٸ���.
        yield return new WaitForSeconds(0.5f);

        //���� text ��Ȱ��
        stateText.gameObject.SetActive(false);

        //���º���
        state = Gamestate.start;
        
    }

    // ����3: �÷��̾��� hp�� 0���� ������ �����ؽ�Ʈ�� ���¸� GameOver�� �ٲ��ְ�

    void CheakGameOver() 
    {
        if(player.hp <= 0) 
        {
            // ����4: �÷��̾��� hp�� 0 ���϶��, �÷��̾��� �ִϸ��̼��� �����.
            animator.SetFloat("MoveMotion", 0f);

            //���� �ؽ�ƮON
            stateText.gameObject.SetActive(true);

            //���� �ؽ�Ʈ�� GameOver�� ����
            stateText.text = "GameOver";

            //���� �ؽ�Ʈ�� �÷��� �������� ����
            stateText.color = new Color32(225, 0, 0, 255);

            state = Gamestate.GameOver;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheakGameOver();
    }
}
