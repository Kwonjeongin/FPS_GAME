using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// ���� : ������ ����(Ready, start, Game Over)�� �����ϰ�, ������ ���۰� ���� TEXTUI�� ǥ���ϰ� �ʹ�.
// �ʿ�Ӽ� : ���ӻ���, ������ ����,TextUI

// ����2 : 2���� ������ ���� �ȴ� .Ready ���¿��� Start���·� ����Ǹ� ������ ���۵ȴ�.

// ���� 3 : �÷��̾��� Hp�� 0���� ������ ���� �ؽ�Ʈ�� GAmeOver�� �ٲ��ְ� ���¶��� GameOver�ιٲ��ش�.
// �ʿ�Ӽ�3 : hp�� �� �ִ� playerMove

//���� 4 :  �÷��̾��� hp�� 0���϶��, �÷��̾��� �ִϸ��̼��� �����.
//�ʿ�Ӽ� : �÷��̾��� �ִϸ����� ������Ʈ

// ����5 : Setting  ��ư�� ������ Option UI �� ������. ���ÿ� ���� �ӵ��� �����Ѵ� (0 or 1)
//�ʿ�Ӽ� : Option UI���ӿ�����Ʈ. �Ͻ����� ����

// ����6 : ���ӿ����� Retry �� Quit ��ư�� Ȱ��ȭ�Ѵ�. 
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // �ʿ�Ӽ� :���ӻ���, ������ ����,TextUI

    public enum Gamestate
    {
        Ready, 
        start, 
        GameOver,
        Start,
        Pause
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

    //�ʿ�Ӽ� : �÷��̾��� �ִϸ����� ������Ʈ
    public GameObject optionUI;

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

            // ����6 : ���ӿ����� Retry �� Quit ��ư�� Ȱ��ȭ�Ѵ�. 
            GameObject retryBtn = stateText.transform.GetChild(0).gameObject;
            GameObject quitBtn = stateText.transform.GetChild(0).gameObject;
            retryBtn.SetActive(true);
            quitBtn.SetActive(true);

            //����7 : ���� ������ HP Bar�� WEapon Mode Text�� ��Ȱ��ȭ�Ѵ�.
            player.hpSlider.gameObject.SetActive(true);
            //player.GetComponent<PlayerFire>().weaponModeTxt.gameObject.SetActive(false);

            state = Gamestate.GameOver;

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheakGameOver();
    }

    // ����5 : Option  ��ư�� ������ Option UI �� ������. ���ÿ� ���� �ӵ��� �����Ѵ� (0 or 1)
    // option ȭ�� �ѱ�
    public void OpenOptionWindow()
    {
        //Option UI �� ������.
        optionUI.SetActive(true);   
        //���ÿ� ���� �ӵ��� �����Ѵ� (00
        Time.timeScale = 0;
        state = Gamestate.Pause;
    }
    //����ϱ�
    public void CloseOptionWindow()
    {
        //Option UI �� ������.
        optionUI.SetActive(false);
        //���ÿ� ���� �ӵ��� �����Ѵ� (1)
        Time.timeScale = 1;
        state = Gamestate.Start;
    }
    // �ٽ��ϱ� �ɼ�
    public void Restart()
    {
        //���ÿ� ���� �ӵ��� �����Ѵ� (1)
        Time.timeScale = 1;

        //�� ��ȣ�� �ٽ� �ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //�������� �ɼ�
    public void QuitGame()
    {
        Application.Quit();
    }
}
