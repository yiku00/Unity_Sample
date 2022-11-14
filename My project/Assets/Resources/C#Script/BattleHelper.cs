using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHelper : MonoBehaviour
{
    // Start is called before the first frame update


    private bool CanbeDamaged = true;
    private int m_facingDirection;
    private Color DefaultRGB;
    private Vector2 AttackBoxPosition;
    private Animator m_animator;
    private SpriteRenderer CharacterRenderer;
    private Rigidbody2D m_body2d;

    public float Damage;
    public float MaxHp;
    public float CurrentHp;
    public bool IsAttacking;
    public bool CanBeDestroy = true;
    public Vector2 AttackBoxOffset = new Vector2(1, 1);
    public Vector2 AttackBoxSize = new Vector2(1, 1);
    [SerializeField] public LayerMask TargetLayer;




    void Start()
    {
        AttackBoxPosition = transform.position;
        AttackBoxOffset = new Vector2(1, 1);
        AttackBoxSize = new Vector2(1, 1);
        CharacterRenderer = GetComponent<SpriteRenderer>();
        CurrentHp = MaxHp;
        m_animator = GetComponent<Animator>();
        DefaultRGB = GetComponent<SpriteRenderer>().color;
        m_body2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_facingDirection = CharacterRenderer.flipX ? -1 : 1;
        AttackBoxPosition = new Vector2((transform.position.x + AttackBoxOffset.x * m_facingDirection), transform.position.y + AttackBoxOffset.y);
        if (IsAttacking)
        {
            Collider2D[] HitCheck = Physics2D.OverlapBoxAll(AttackBoxPosition, AttackBoxSize, 90f, TargetLayer);
            if (HitCheck.Length > 0)
            {
                if(GetComponent<Collider2D>() == HitCheck[0])
                {
                    return;
                }
                DamageInfo obj = new DamageInfo();
                obj.damage = Damage;
                obj.DamagingObject = GetComponent<Collider2D>();
                HitCheck[0].SendMessage("ApplyDamage", obj);
                IsAttacking = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireCube(AttackBoxPosition, AttackBoxSize);
    }

    public void ApplyDamage(DamageInfo Dmg)
    {
        if (CanbeDamaged)
        {
            if (Dmg.damage < 0 || CurrentHp <= 0)
                return;
            CanbeDamaged = false;
            CurrentHp -= Dmg.damage;
            Debug.Log("Current Hp = " + CurrentHp);

            if (CurrentHp <= 0)
            {
                CurrentHp = 0f;
                OnDead();

            }
            else
            {
                OnHit(Dmg.DamagingObject);
            }

        }

    }

    public void InitHp(float Hp)
    {
        CurrentHp = MaxHp = Hp;
    }

    private void OnHit(Collider2D collision)
    {
        m_animator.SetTrigger("Hurt");
        Invoke("resetCanbeDamaged", 1.5f);
        GetComponent<SpriteRenderer>().color = new Color(DefaultRGB.r, DefaultRGB.g, DefaultRGB.b, 0.7f);
        int dir = transform.position.x - collision.transform.position.x > 0 ? 1 : -1;
        m_body2d.AddForce(new Vector2(dir, 1f) * 3f, ForceMode2D.Impulse);
    }

    private void OnDead()
    {
        m_animator.SetTrigger("Death");
        gameObject.SendMessage("CharaOnDead");
        if (CanBeDestroy)
            Destroy(gameObject, 5f);
    }
    private void resetCanbeDamaged()
    {
        CanbeDamaged = true;
        GetComponent<SpriteRenderer>().color = new Color(DefaultRGB.r, DefaultRGB.g, DefaultRGB.b, 1f);
    }
}
