using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float _chaseRange = 10;
    [SerializeField] private float _attackRange = 50;
    [SerializeField] private float _speed = 10;
    [SerializeField] private LayerMask _layerPlayer;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private ScriptableobjectPlayer _playerData;
    [SerializeField] private ScriptableobjectPlayer _bossData;
    [SerializeField] private int _amount = -10;
    [SerializeField] private int _damge = -20;
    [SerializeField] private float _timeBack = 2f;
    private bool isInChaseRange = false,isInAttackRange = false;
    private int hashIsWalking;
    private int haskIsRunning;
    private int hashIsAttack;
    private int hashIsDie;
    private int hashTrigRun;
    private int hashIsDeath;
    public bool isAttacking = false;
    private bool canInflict = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hashIsWalking = Animator.StringToHash("isWalking");
        haskIsRunning = Animator.StringToHash("isRunning");
        hashIsAttack = Animator.StringToHash("isAttack");
        hashIsDie = Animator.StringToHash("isDie");
        hashTrigRun = Animator.StringToHash("TrigRun");
        hashIsDeath = Animator.StringToHash("IsDeath");
        GetComponent<HealthBar>().SetMaxHealth(_bossData._health);
    }

    // Update is called once per frame
    void Update()
    {
        if(_bossData._health != 0)
        {
            isInChaseRange = Physics.CheckSphere(transform.position, _chaseRange, _layerPlayer);
            isInAttackRange = Physics.CheckSphere(_attackPoint.position, _attackRange, _layerPlayer);
            if (!isInAttackRange && isInChaseRange && !isAttacking)
            {
                Vector3 newPos = _player.position;
                newPos.y = 0;
                anim.SetBool(hashIsWalking, true);
                transform.LookAt(_player.position);
                transform.position = Vector3.MoveTowards(transform.position, newPos, _speed * Time.deltaTime);
            }
            else if (isInChaseRange && isInAttackRange && _playerData._health != 0)
            {
                if (!isAttacking)
                {
                    transform.LookAt(_player.position);
                    anim.SetBool(hashIsWalking, false);
                    anim.SetTrigger(hashIsAttack);
                }
                else
                {
                    Collider[] player = Physics.OverlapSphere(_attackPoint.position, _attackRange, _layerPlayer);
                    foreach (var elem in player)
                    {
                        if (elem.CompareTag("Player"))
                        {
                            canInflict = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                anim.SetBool(hashIsWalking, false);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _chaseRange);
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other);
            _bossData.UpdateHealth(_amount);
            GetComponent<HealthBar>().SetHealth(_bossData._health);
            if(_bossData._health == 0)
            {
                anim.SetTrigger(hashIsDie);
            }
        }
    }

    public void AttackTime()
    {
        if (canInflict)
        {
            if (isInAttackRange)
            {
                _playerData.UpdateHealth(_damge);
                _player.gameObject.GetComponent<HealthBar>().SetHealth(_playerData._health);
                if(_playerData._health == 0) _player.gameObject.GetComponent<Animator>().SetTrigger(hashIsDeath);
                canInflict = false;
            }
        }
    }
}
