using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

namespace Test
{
    public class Role : MonoBehaviour
    {
        public int id = 0;
        Player player { get { return ReInput.players.GetPlayer(id); } }
        public int attackSelector = -1;
        public int str = 2;
        public int hp = 10;

        public GameObject test;

        [SerializeField] Image[] arrows = new Image[3];
        [SerializeField] Image shield = null;
        [SerializeField] bool testMode = false;
        Rigidbody2D rb = null;
        Animator animator = null;
        PlayerActions Actions;

        bool isControled = false;
        bool isDamaging = false;
        bool isAttacking = false;
        bool isAbleToSelect = true;
        bool isJumping = false;

        float jumpTimeCounter = 0f;
        /// <summary>
        /// Selector ID: -1.NULLselector 0.Up 1.Middle 2.Down
        /// </summary>
        int selector = -1;

        public void WaitForAnimationFinish(string name, System.Action callback)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;

            float sec = ac.animationClips.FirstOrDefault(c => c.name.ToLower().Contains(name)).length;

            this.AbleToDo(sec, callback);
        }

        public void GetDefend(Role enemy)
        {
            isControled = true;
            animator.SetTrigger("exit");
            Vector2 force = ((transform.position.x - enemy.transform.position.x) > 0
                ? Vector2.right : Vector2.left
            ) * 7;
            rb.AddForce(
                force,
                ForceMode2D.Impulse
            );

            string name = "attack" + attackSelector;
            this.AbleToDo(0.1f, () => WaitForAnimationFinish(name, () => isControled = false));
        }

        public void Hurt(Role enemy)
        {
            if (enemy.attackSelector == selector && selector != -1 && CheckFaceEnemy(enemy.transform))
            {
                shield.transform.position = arrows[selector].transform.position;
                shield.gameObject.SetActive(true);
                this.AbleToDo(0.75f, () => shield.gameObject.SetActive(false));
                Vector2 force = (Vector2)(transform.position - enemy.transform.position) * 1;
                rb.AddForce(
                    force,
                    ForceMode2D.Impulse
                );
                enemy.GetDefend(this);
            }
            else
            {
                if (isDamaging) return;
                isDamaging = true;
                animator.SetTrigger("hurt");
                WaitForAnimationFinish("hurt", () => isDamaging = false);
                hp -= enemy.str;
                if (hp <= 0) Destroy(this.gameObject);
                isControled = true;
                Vector2 force = (Vector2)(transform.position - enemy.transform.position + Vector3.up * 2) * 5;
                this.AbleToDo(0.1f, () => isControled = false);
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
            if (isControled) return;
            rb.velocity = new Vector2(
                (player.GetAxis(Actions.Actions[0]) * Vector2.right).x * 10,
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
            if (player.GetButtonDown(Actions.Actions[1]))
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
            //Defend();
            Attack();
        }
        public void Selector()//Change arrow by mouse
        {
            var mousePosition = Camera.main.ScreenToWorldPoint
                (
                    new Vector3
                    (
                        Input.mousePosition.x, 
                        Input.mousePosition.y, 
                        Mathf.Abs(Camera.main.gameObject.transform.position.z)
                    )
                ) - transform.position;
            var angle = Vector2.Angle(Vector2.down, mousePosition);
            //Debug.Log(angle);
            //test.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            //test.GetComponent<LineRenderer>().SetPositions(new Vector3[] { transform.position, test.transform.position });
            if (angle > 0 && angle < 70)
                SelectArrow(2);
            else if(angle > 70 && angle < 110)
                SelectArrow(1);
            else if (angle > 110 && angle < 180)
                SelectArrow(0);
        }

        public void SelectArrow(int id)
        {
            ActiveArrow(id);
            selector = id;
        }

        public void Defend()
        {
            #region Old Defence
            //if (player.GetButton("SelectorTop"))
            //{
            //    SelectArrow(0);
            //}
            //else if (
            //    (transform.localScale.x > 0 && player.GetButton("SelectorFront"))
            //    || (transform.localScale.x < 0 && player.GetButton("SelectorBack"))

            //)
            //{
            //    SelectArrow(1);
            //}
            //else if (player.GetButton("SelectorDown"))
            //{
            //    SelectArrow(2);
            //}
            //else
            //{
            //    SelectArrow(-1);
            //}
            #endregion

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
            if (player.GetButtonDown(Actions.Actions[3]))
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
                    Hurt(other.transform.parent.GetComponent<Role>());
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
        private void Start()
        {
            Actions = new PlayerActions(id);
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
            Selector();
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
public class PlayerActions
{
    /// <summary>
    /// Actions ID: 0.MoveHorizontal 1.Jump 2.Dash 3.Attack 4.Defence 5.PickUp 6.SpacialAtk 7.Selector
    /// </summary>
    public string[] Actions = { "MoveHorizontal", "Jump", "Dash", "Attack", "Defence", "PickUp", "SpacialAtk", "Selector" };
    public int Id;
    /// <param name="id">1.Keyborad 2. Controller</param>
    public PlayerActions(int id)
    {
        if(id == 1)//Keyboard Control
        {
            for(var i = 0; i < Actions.Length; i++)
            {
                Actions[i] = "K_" + Actions[i];
            }
            Id = id; 
        }
        else if(id == 2)//Controller Control
        {
            for (var i = 0; i < Actions.Length; i++)
            {
                Actions[i] = "C_" + Actions[i];
            }
            Id = id;
        }
        else
        {
            Debug.LogError("PlayerActions Don't exist id.");
        }
    }
}