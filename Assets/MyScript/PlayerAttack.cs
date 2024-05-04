using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEngine.AI;

namespace Character
{
    public class PlayerAttack : MonoBehaviour
    {
        private const char NEW_LINE = '\n';
        private const char EQUALS = '=';

        protected Enemy enemyPlayer;
        public Animator playerAn;
        public Transform attackPoint;
        protected CharacterStatus status;
        private float attackRange;
        public LayerMask enemyLayer;
        private float attackRate;
        private float nextAttackTime = 0f;
        private int upgradeSelected;
        private const int indexClip = 1;
        
        // Start is called before the first frame update
        void Start()
        {
            upgradeSelected = PlayerPrefs.GetInt("upgradeSelected", 1);
            status = GetComponent<CharacterStatus>();
            enemyPlayer = GetComponent<Enemy>();

            string filePath = "File/characterAttack" + upgradeSelected + "Features";
            TextAsset data = Resources.Load<TextAsset>(filePath);
            string[] lines = data.text.Split(NEW_LINE);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] token = line.Split(EQUALS);

                switch (token[0])
                {
                    case "attackRange":
                        attackRange = float.Parse(token[1], CultureInfo.InvariantCulture);
                        break;
                    case "attackRate":
                        attackRate = float.Parse(token[1], CultureInfo.InvariantCulture);
                        Debug.Log("damageTimeout: " + attackRate);
                        break;
                    default:
                        break;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.time >= nextAttackTime)
            {
                if (status.IsAttacking)
                {
                    playerAn.SetTrigger("isAttacking1");
                    Attack();
                    nextAttackTime = Time.time + 1f / attackRate;
                    FindObjectOfType<MoveCharacter>().enabled = false;
                }else{
                        FindObjectOfType<MoveCharacter>().enabled = true;
                     }
                }
            }
       void Attack()
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider enemy in hitEnemies)
            {
                  StartCoroutine(EnemyDeath(enemy.gameObject));
            }    
        }
        IEnumerator EnemyDeath(GameObject enemy)
        {
            enemy.gameObject.GetComponent<EnemyController>().enabled = false;
            enemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            enemy.gameObject.GetComponent<AutoShooting>().enabled = false;
            enemy.gameObject.tag = "Untagged";
            Animator enemyAn = enemy.gameObject.GetComponent<Animator>();
            enemyAn.SetBool("isAttacking", false);
            enemyAn.SetBool("isShooting", false);
            enemyAn.SetBool("isDeath", true);
            var animController = enemyAn.runtimeAnimatorController;
            var clip = animController.animationClips[indexClip];

            yield return new WaitForSeconds(clip.length);
            Destroy(enemy,3.0f);
        }

        private void OnDrawGizmosSelected()
        {
            if (attackPoint.position == null)
                return;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}