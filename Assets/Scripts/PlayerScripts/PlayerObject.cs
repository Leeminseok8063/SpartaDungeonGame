using Assets.Scripts.PlayerScripts;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerObject : MonoBehaviour
{
    MovementController movementController;
    PlayerGameInterface gameInterface;
    Animator animator;

    [Header("PlayerState")]
    public int health;
    public int shield;
    public int energe;
    public int damage;
    public float energeRecoverTime;
    
    private int maxHealth;
    private int maxShield;
    private int maxEnerge;

    public bool isDamaged = false;
    private float EnergeTimer = 0;

    GameObject currentTarget;
    GameObject currentItemObject;
    GameObject equipItemObject;
    MonsterObject mobTarget;
    
    public LayerMask interactMask;
    public RuntimeAnimatorController gunAnimatorController;
    public GameObject GunObject;
    public GameObject GunFirePosition;
    public GameObject Projectile;

    Vector3 currentDir = Vector3.zero;
    Vector3 currentCheckPoint = Vector3.zero;
    //================================
    


    private void Awake()
    {
        GameManager.Instance.Player = this;
        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        movementController = GetComponent<MovementController>();
        gameInterface = GetComponent<PlayerGameInterface>();
        PlayerInit();
        gameInterface.UpdateGameInterface();
    }

    private void Update()
    {
        EnergeTimer += Time.deltaTime;
        if(EnergeTimer > energeRecoverTime && energe < maxEnerge)
        {
            EnergeTimer = 0;
            IncreaseEnerge();
        }
        else if(energe == maxEnerge)
        {
            EnergeTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        FallingCheck();
        FrontCasting();
    }

    private void LateUpdate()
    {
        CheckInteractToItem();
    }

    public void IncreaseEnerge()
    {
        energe = Mathf.Min(maxEnerge, ++energe);
        gameInterface.UpdateGameInterface();
    }
    public void IncreaseShield()
    {
        shield = Mathf.Min(maxShield, ++shield);
        gameInterface.UpdateGameInterface();
    }
    public void IncreaseHealth()
    {      
        health = Mathf.Min(maxHealth, ++health);
        gameInterface.UpdateGameInterface();
    }
    public IEnumerator DamageToPlayer()
    {
        if (shield == 0)
        {
            health = Mathf.Max(0, --health);
        }
        else
        {
            shield = Mathf.Max(0, --shield);
        }

        if (health == 0) SceneManager.LoadScene("DevScene");
        gameInterface.UpdateGameInterface();
        animator.SetTrigger("isDamaged");
        isDamaged = true;
        movementController.GetRigidbody().velocity = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        isDamaged = false;
    }
    public bool DecreaseEnerge()
    {
        if (energe == 0) return false;
        
        energe = Mathf.Max(0, --energe);
        gameInterface.UpdateGameInterface();
        return true;
    }

    public void MobTargetToDamage()
    {
        if(mobTarget != null)
        {
           mobTarget.DamageToMonster(damage);
        }
    }

    public void GetWeapon()
    {
        animator.runtimeAnimatorController = gunAnimatorController;
        GunObject.SetActive(true);
    }

    private void PlayerInit()
    {
        currentCheckPoint = new Vector3(-45.2000008f, 17.6000004f, -43.9000015f); //¤¾¤¾..
        maxHealth = health;
        maxShield = shield;
        maxEnerge = energe;
        shield = 0;
    }

    private void FrontCasting()
    {
        Vector3 box = new Vector3(2.5f, 3f, 2.5f);
        Vector3 center = transform.position + (transform.forward * 2f) + (Vector3.up * 1f);
        Collider[] allItems = Physics.OverlapBox(center, box / 2, Quaternion.identity, interactMask);
        if(allItems.Length != 0)
        {
            foreach(Collider coll in allItems)
            {
                if (coll.TryGetComponent<ItemObject>(out ItemObject item))
                {
                    gameInterface.interactPanel.gameObject.SetActive(true);
                    gameInterface.ItemNameText.text = item.info.itemName;
                    gameInterface.ItemDescText.text = item.info.itemDesc;
                    currentItemObject = item.gameObject;
                }
                else if(coll.TryGetComponent<MonsterObject>(out MonsterObject mob))
                {
                    //StartCoroutine(mob.DamageToMonster(damage));
                    mobTarget = mob;
                }
            }         
        }
        else
        {
            gameInterface.interactPanel.gameObject.SetActive(false);
            currentItemObject = null;
            mobTarget = null;
        }

        Funtions.DrawBoxCasting(box, center);
    }

    private void CheckInteractToItem()
    {
        if (currentItemObject == null || !movementController.isInteract) return;
        ItemObject item = currentItemObject.GetComponent<ItemObject>();
        switch(item.info.type)
        {
            case ITEMTYPE.CONSUME:
                IncreaseHealth();
                break;
            case ITEMTYPE.JUMPER:
                equipItemObject = currentItemObject;
                movementController.BaseJumpVal = item.info.power;
                break;
            case ITEMTYPE.SHIELD:
                IncreaseShield();
                break;
            case ITEMTYPE.CHECKPOINT:
                currentCheckPoint = item.GetCheckPointPos();
                break;
            case ITEMTYPE.INTERACTABLE:
                item.TriggerInteractItem();
                return;
            case ITEMTYPE.WEAPON:
                GetWeapon();
                break;
        }
        //movementController.isInteract = false;
        Destroy(currentItemObject);
        currentItemObject = null;
    }

    private void TeleportToCheckPoint()
    {
        gameObject.transform.position = currentCheckPoint;
    }

    private void FallingCheck()
    {
        if(gameObject.transform.position.y < -10)
        {
            StartCoroutine(DamageToPlayer());
            TeleportToCheckPoint();
        }     
    }
    
}
