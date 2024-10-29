using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

enum MOBSTATE
{
    IDLE,
    MOVE,
    ATTACK,
    DEATH,
}

public class MonsterObject : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private MOBSTATE state;

    public float minDist;
    public float maxDist;
    public float calculDelay;
    public float detectDist;
    public float attackDist;
    public float health;

    private float localTimer;
    private bool isMove = false;
    private bool isAttack = false;

    private Vector3 dist = Vector3.zero;
    private PlayerObject targetPlayer;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        state = MOBSTATE.IDLE;
        targetPlayer = GameManager.Instance.Player;
    }
    private void Update()
    {
        switch (state)
        {
            case MOBSTATE.IDLE:
                IdleStateUpdate();
                break;
            case MOBSTATE.MOVE:
                MoveStateUpdate();
                break;
            case MOBSTATE.ATTACK:
                AttackStateUpdate();
                break;
            case MOBSTATE.DEATH:
                DeathStateUpdate();
                return;
        }
        
        AnimUpdate();
    }

    private void IdleStateUpdate()
    {
        localTimer += Time.deltaTime;
        navMeshAgent.isStopped = true;
        isMove = false;
        if ((targetPlayer.transform.position - transform.position).magnitude < detectDist)
        {
            localTimer = 0;
            state = MOBSTATE.ATTACK;
            isMove = true;
        }
        else if (localTimer > calculDelay)
        {
            localTimer = 0;
            CalculatePath();
            state = MOBSTATE.MOVE;
            isMove = true;
        }
    }
    private void MoveStateUpdate()
    {
        if ((dist - transform.position).magnitude < 0.1f)
        {
            state = MOBSTATE.IDLE;
            navMeshAgent.isStopped = true;
        }
    }
    private void AttackStateUpdate()
    {
        if ((targetPlayer.transform.position - transform.position).magnitude > detectDist * 2)
        {
            state = MOBSTATE.IDLE;
            return;
        }
        else if ((targetPlayer.transform.position - transform.position).magnitude < 2f && !isAttack)
        {
            StartCoroutine(AttackToPlayer());
        }
        else
        {
            CalculateTargetPath();
        }
    }
    private void DeathStateUpdate()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("MonsterArmature|Death") && stateInfo.normalizedTime >= 1f)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    private void AnimUpdate()
    {
        animator.SetBool("isMove", isMove);
    }

    private void CalculatePath()
    {      
        navMeshAgent.isStopped = false;
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minDist, maxDist)), out hit, maxDist, NavMesh.AllAreas);
        navMeshAgent.SetDestination(hit.position);  
        dist = hit.position;
    }
    private void CalculateTargetPath()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(targetPlayer.transform.position);
    }
    
    private IEnumerator AttackToPlayer()
    {
        isAttack = true;
        navMeshAgent.isStopped = true;
        animator.SetTrigger("isAttack");
        Vector3 dir = targetPlayer.transform.position - transform.position;
        
        Ray ray = new Ray(transform.position, dir.normalized);
        if(Physics.Raycast(ray, out RaycastHit hit, attackDist, LayerMask.GetMask("Player")))
        {
            StartCoroutine(hit.collider.gameObject.GetComponent<PlayerObject>().DamageToPlayer());
        }
        
        yield return new WaitForSeconds(1f);
        isAttack = false;

    } 
    public void DamageToMonster(int damage)
    {
        if(!(state == MOBSTATE.DEATH))
        {
            health = Mathf.Max(0, health - damage);
            animator.SetTrigger("isDamaged");

            if(health == 0)
            {
                state = MOBSTATE.DEATH;
                navMeshAgent.isStopped=true;
                animator.SetTrigger("isDeath");

            }
        }
        
    }

}
