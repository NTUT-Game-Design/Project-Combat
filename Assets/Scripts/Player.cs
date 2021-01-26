using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class Player : MonoBehaviour
    {

        public int attackSelector = -1;
        public int str = 2;
        public int hp = 10;

        [SerializeField] Image[] arrows = new Image[3];
        [SerializeField] Image shield = null;
        [SerializeField] bool testMode = false;
        Rigidbody2D rb = null;
        Animator animator = null;

        bool isDamaging = false;
        bool isAttacking = false;
        bool isAbleToSelect = true;
        bool isJumping = false;

        float jumpTimeCounter = 0f;

        int selector = -1;

        public void WaitForAnimationFinish(string name, System.Action callback)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;

            float sec = ac.animationClips.FirstOrDefault(c => c.name.ToLower().Contains(name)).length;

            this.AbleToDo(sec, callback);
        }

        public void Hurt(Player enemy)
        {
            if (enemy.attackSelector == selector && selector != -1 && CheckFaceEnemy(enemy.transform))
            {
                shield.transform.position = arrows[selector].transform.position;
                shield.gameObject.SetActive(true);
                this.AbleToDo(0.75f, () => shield.gameObject.SetActive(false));
            }
            else
            {
                if (isDamaging) return;
                isDamaging = true;
                animator.SetTrigger("hurt");
                WaitForAnimationFinish("hurt", () => isDamaging = false);
                hp -= enemy.str;
                Vector2 force = (Vector2)(transform.position - enemy.transform.position + Vector3.up * 2) * 5;
                rb.AddForce(
                    force,
                    ForceMode2D.Impulse
                );
            }
        }

        bool CheckFaceEnemy(Transform enemy)
        {
            if (transform.localScale.x < 0 && transform.position.x - enemy.position.x < 0)
                return false;
            if (transform.localScale.x > 0 && transform.position.x - enemy.position.x > 0)
                return false;
            return true;
        }

        public void Move()
        {
            rb.velocity = new Vector2(
                (Input.GetAxis("Horizontal") * Vector2.right).x * 10,
                rb.velocity.y
            );
            transform.localScale = new Vector3(transform.localScale.x >= 0
                ? rb.velocity.x >= 0 ? 1 : -1
                : rb.velocity.x <= 0 ? -1 : 1
            , 1, 1);
            animator.SetFloat("movement", Mathf.Abs(rb.velocity.x));
        }

        public void Jump()
        {
            if (isJumping) return;
            if (Input.GetKeyDown(KeyCode.V))
            {
                isJumping = true;
                SelectArrow(-1);
                rb.velocity = new Vector2(rb.velocity.x, 20);
            }
        }

        IEnumerator JumpCoroutine()
        {
            while (jumpTimeCounter < 0.25f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 25);
                yield return null;
            }
        }

        public void Combat()
        {
            if (isJumping) return;
            if (isAbleToSelect == false) return;
            Defend();
            Attack();
        }

        public void SelectArrow(int id)
        {
            ActiveArrow(id);
            selector = id;
        }

        public void Defend()
        {
            Debug.Log(1);
            if (Input.GetKey(KeyCode.J))
            {
                SelectArrow(0);
            }
            else if (Input.GetKey(KeyCode.K))
            {
                SelectArrow(1);
            }
            else if (Input.GetKey(KeyCode.L))
            {
                SelectArrow(2);
            }
            else
            {
                SelectArrow(-1);
            }
        }

        public void ActiveArrow(int id)
        {
            int index = 0;
            foreach (Image arrow in arrows)
            {
                arrow.gameObject.SetActive((index == id) ? true : false);
                index++;
            }
        }

        public void Attack()
        {
            if (isAttacking || selector == -1) return;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isAttacking = true;
                isAbleToSelect = false;
                attackSelector = selector;
                animator.SetTrigger("attack" + attackSelector);
                WaitForAnimationFinish("attack" + attackSelector, () =>
                    {
                        isAttacking = false;
                        isAbleToSelect = true;
                    }
                );
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.tag)
            {
                case "PlayerWeapon":
                    Hurt(other.transform.parent.GetComponent<Player>());
                    break;
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            isJumping = false;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (testMode)
            {
                ActiveArrow(1);
                selector = 1;
                return;
            }
            Move();
            Jump();
            Combat();
        }
    }

    public static class PlayerExtension
    {
        public static void AbleToDo<T>(this T source, float sec, System.Action callback) where T : MonoBehaviour
        {
            source.StartCoroutine(DelaySecAndDo(sec, callback));
        }

        public static IEnumerator DelaySecAndDo(float sec, System.Action callback)
        {
            yield return new WaitForSeconds(sec);
            callback();
        }
    }
}