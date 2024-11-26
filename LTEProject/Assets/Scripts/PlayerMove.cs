using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    RaycastHit2D rayHit;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*Jump : Space �� ������ ������� �νĵ�*/
        if (Input.GetButtonDown("Jump") && !anim.GetBool("IsJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("IsJumping", true);
        }
        //��ư���� ���� ���� �� ����
        if(Input.GetButtonUp("Horizontal"))
        {
            /*normalized : ���� ũ�⸦ 1�� ����(�������� ����)*/
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        //������ȯ
        if (Input.GetButtonDown("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        /*�ȴ� ���ϸ��̼� ���� */ 
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
        {
            /*SetBool : bool �� true / false �� ������ */
            anim.SetBool("IsWalking", false);
        }
        else
        {
            anim.SetBool("IsWalking", true);
        }
    }

    void FixedUpdate()
    {
        /*Horizontal : ���� �̵� (2D ���� X��)*/
        /*Vertical : ���� �̵� (2D ���� Y��)*/
        float height = Input.GetAxisRaw("Horizontal");
        //Debug.Log("height �Է� : " + height);

        /*AddForce : ���� ������Ʈ�� �̵��ϰų�, �̵��ӵ� �Ǵ� ������ ������ �� ����ϴ� �Լ�*/
        rigid.AddForce(Vector2.right*height, ForceMode2D.Impulse);
        /*Mathf.Abs : ������ ���ϴ� �Լ�*/

        if(rigid.velocity.x > maxSpeed) //������ �ִ� �ӵ� ���޽�
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed*(-1)) //���� �ִ� �ӵ� ���޽�
        {
            rigid.velocity = new Vector2(maxSpeed*(-1), rigid.velocity.y);
        }

        /*Mathf.Abs : ������ ���ϴ� �Լ�.*/
        Debug.Log("���� �ӵ� : " + Mathf.Abs(rigid.velocity.x));

        /*����*/
        if(rigid.velocity.y < 0)
        {
            /*RayCast : ������Ʈ �˻��� ���ؼ� Ray�� ��� ���.*/
            /*DrawRay :  ������ �󿡼� �� Ray�� �׸��� �Լ�.*/
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 0, 0));

            /*RaycastHit : ray�� ���� ������Ʈ*/
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHit.collider != null)
            {
                if(rayHit.distance < 0.5f)
                {
                    anim.SetBool("IsJumping", false);
                }
                Debug.Log(rayHit.collider.name);
            }
            
        }
    }
}
