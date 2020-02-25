using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//动作枚举
public enum AnimatorParameters
{
    Idle,
    Walk,
    Run,
    MeleeRightAttack01=7,//近战攻击
    MeleeRightAttack02,
    MeleeRightAttack03,
    ProjectileRightAttack01=14,
    CrossbowShootAttack = 17,
    TakeDamage =25,
    Die,
    FlyIdle=33,
    FlyForward,
    FlyMeleeRightAttack01,
    FlyMeleeRightAttack02,
    FlyMeleeRightAttack03,
    FlyTakeDamage = 46,
    FlyDie = 47,
    Victory =55
}

public enum ActionParamtersType
{
    Float,
    Int,
    Bool,
    Trigger
}
public class AIBase : MonoBehaviour
{
    // Start is called before the first frame update
    //[System.NonSerialized]//will not be shown in the inspector or serialized
    [Header("生命值")]
    public int HP = 1000;
    private int MaxHp;

    [Header("搜索半径")]
    [Range(0.1f,100.0f)]//范围定义
    public float searchRadius = 100.0f;//搜索半径

    [Header("攻击半径(搜索半径 >=攻击半径)")]
    [Range(0.1f, 100.0f)]//范围定义
    public float attackRadius = 100.0f;//攻击半径

    [Header("搜索间隔")]
    [Range(0.016f, 0.16f)]//范围定义
    public float searchInterval = 0.016f;//搜索间隔
    protected float timeCumulative = 0.0f;

    [Header("自身类型")]
    public EnityType selfType;

    [Header("动作类")]
    public Animator animator;

    [Header("攻击对象类型")]
    public EnityType attackType;

    [Header("是否可以移动")]
    public bool isCanMove;

    [Header("移动速度")]
    [Range(0.01f, 0.5f)]
    public float moveSpeed= 0.1f;

    [Header("一次攻击目标的数量")]
    public int attackTargetNum = 1;//默认是1个

    [Header("死亡特效")]
    public GameObject dieParticle;

    public AnimatorParameters attackAnimatorType = AnimatorParameters.MeleeRightAttack02;
    public AnimatorParameters moveAnimatorType = AnimatorParameters.Run;



    public AIWeapon aiWeapon;
    private Quaternion initRotation;

    protected GameObject currentLockTarget;//当前锁定的目标

    private Enity enity;
    private bool isInit = false;


    void Start()
    {
        transform.gameObject.tag = selfType.ToString();
        aiWeapon = GetComponent<AIWeapon>();
        initRotation = transform.rotation;
        MaxHp = HP;
        //dieParticle = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", "explosion_stylized_large_jokerFire");
    }

    // Update is called once per frame
    void Update()
    {
        timeCumulative += Time.deltaTime;
        if (isInit)
        {
            //timeCumulative = 0.0f;
                ///////////////搜索周边敌人////////////////////////////
                //print("attackType:" + attackType.ToString());
            //if(EnemyManager.GetInstance().GetEnemysCount(attackType) == 0)
            {
                //transform.LookAt(Vector3.forward);
                //DoAction(AnimatorParameters.Victory, ActionParamtersType.Bool, 1);
                //return;
            }
            
            List<GameObject> targetList = GetGameObjectsByAttackTargetType(attackType);
            AnimatorStateInfo _animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (targetList.Count > 0)//有攻击目标
            {
                if(attackTargetNum > 0 && aiWeapon != null)//攻击数量
                {
                    GameObject lockTarget = null;//锁定最近的目标
                    float distanceTemp = float.MaxValue;
                    List<GameObject> inAttackRadiusTargetList = new List<GameObject>();//在攻击范围内的怪物
                    for (int i = 0; i < attackTargetNum; i++)
                    {
                        GameObject target = targetList[i];
                        float distance = Vector3.Distance(target.transform.position, transform.position);
                        if(distanceTemp > distance)
                        {
                            distanceTemp = distance;
                            lockTarget = target;
                        }
                        if (aiWeapon.attackRadius > distance)//在攻击范围内
                        {
                            inAttackRadiusTargetList.Add(target);
                        }
                    }
                    
                    if (inAttackRadiusTargetList.Count > 0)//有可以攻击的怪物
                    {
                        if(!EnityManager.GetInstance().hpManager.IsCreateHP(gameObject.name)){
                            UpdateHP();
                        }
                        
                        transform.rotation = Quaternion.LookRotation(lockTarget.transform.position - transform.position);//先调整方向，朝向最近目标
                        
                        ///////////////////好，攻击目标开始！//////////////////////////////
                        if(animator.parameterCount > (int)attackAnimatorType)
                        {
                            DoAction(moveAnimatorType, ActionParamtersType.Bool, 0);//如果在跑动，先停止跑动
                            if (aiWeapon.timeCumulative > aiWeapon.attackSpeed)
                            {
                                aiWeapon.timeCumulative = 0.0f;
                                aiWeapon.isCanAttack = true;
                                DoAction(attackAnimatorType, ActionParamtersType.Trigger);
                                
                            }
                            //AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
                            // 判断动画是否播放完成
                            if (_animatorStateInfo.normalizedTime >= 0.7f)
                            {

                                //if (animator.parameterCount > (int)attackAnimatorType)//判断是否有这个参数INDEX
                                {
                                    string _animatorParameterName = animator.GetParameter((int)attackAnimatorType).name;
                                    if (_animatorStateInfo.IsName(_animatorParameterName))
                                    {
                                        if (aiWeapon.isCanAttack)
                                        {
                                            //播放完毕，要执行的内容
                                            //print("info:" + info.ToString());
                                            aiWeapon.isCanAttack = false;
                                            aiWeapon.OnAttackTarget(lockTarget, 0);
                                        }
                                    }
                                }
                                

                            }

                        }
                        else
                        {
                            //没有武器自爆处理
                            //KillSelf();

                        }
                        
                    }
                    else//需要移动过去，如果可以移动的话
                    {
                        if (isCanMove)//先判断是否可以跑动
                        {
                            DoAction(moveAnimatorType, ActionParamtersType.Bool);//播放跑动动画
                            //transform.rotation = Quaternion.LookRotation(lockTarget.transform.position - transform.position);//先调整方向，朝向目标
                            transform.LookAt(lockTarget.transform.position);
                            if (animator.parameterCount > (int)moveAnimatorType && _animatorStateInfo.IsName(animator.GetParameter((int)moveAnimatorType).name))//判断在运动状态再移动，不然会出现还在攻击状态下漂移的情况
                            {
                                //////////////开始移动////////////////////////
                                //transform.position = transform.position + transform.forward.normalized * 0.1f;
                                //GameObject projectile = Instantiate(bombPrefab, spawnPosition.position, Quaternion.identity) as GameObject;
                                //projectile.transform.LookAt(target.transform.position);
                                GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward.normalized * moveSpeed);
                            }



                        }
                    }
                    currentLockTarget = lockTarget;
                }
            }
            else
            {
                if(selfType.ToString() == EnityType.Hero.ToString())
                {
                    DoAction(moveAnimatorType, ActionParamtersType.Bool, 1);
                    if (animator.parameterCount > (int)moveAnimatorType && _animatorStateInfo.IsName(animator.GetParameter((int)moveAnimatorType).name))//判断在运动状态再移动，不然会出现还在攻击状态下漂移的情况
                    {
                        transform.rotation = initRotation;
                        GetComponent<Rigidbody>().MovePosition(transform.position + transform.forward.normalized * moveSpeed);
                    }
                }
                
            }

        }
    }
    public void InitAIBaseByEnity(Enity enity)
    {
        this.enity = enity;

        selfType = enity.enityType;
        attackType = EnityManager.GetInstance().GetAttackEnityType(selfType);
        isCanMove = enity.heroTableData.isCanMove == 1;
        searchRadius = enity.heroTableData.searchRadius;
        moveSpeed = enity.heroTableData.moveSpeed;
        moveAnimatorType = AnimatorParameters.Run;
        animator = enity.gameObject.GetComponent<Animator>();
        //////////////////////////判断是否装备武器///////////////////////////////////
        Item equip = enity.GetEnityEquip();
        if (equip != null)
        {
            aiWeapon = gameObject.AddComponent<AIWeapon>();
            aiWeapon.InitWeaponByEquip(equip);
        }
        isInit = true;
    }
    void DoAction(AnimatorParameters _actionType, ActionParamtersType _ActionParamtersType,int _intValue =1,float _floatValue = 0.0f)//播放或停止动作
    {
        if (animator != null && animator.parameterCount > (int)_actionType)
        {
            string _animatorParameterName = animator.GetParameter((int)_actionType).name;
            switch (_ActionParamtersType)
            {
                case ActionParamtersType.Float:
                    if (animator.GetFloat(_animatorParameterName) != _floatValue)
                        animator.SetFloat(_animatorParameterName, _floatValue);
                    break;
                case ActionParamtersType.Int:
                    if (animator.GetInteger(_animatorParameterName) != _intValue)
                        animator.SetInteger(_animatorParameterName, _intValue);
                    break;
                case ActionParamtersType.Bool:
                    if (animator.GetBool(_animatorParameterName) != _intValue > 0)
                        animator.SetBool(_animatorParameterName, _intValue >0);
                    break;
                case ActionParamtersType.Trigger:
                    animator.SetTrigger(_animatorParameterName);
                    break;
                default:
                    break;
            }
        }
    }
    List<GameObject> GetGameObjectsByAttackTargetType(EnityType _attackType)
    {
        List<GameObject> targetList = new List<GameObject>();
        GameObject[] _gameObjects = GameObject.FindGameObjectsWithTag(_attackType.ToString());
        for (int i = 0; i < _gameObjects.Length; i++)
        {
            GameObject _gameObject = _gameObjects[i];
            float distance = Vector3.Distance(_gameObject.transform.position, transform.position);
            if (searchRadius > distance)
            {
                targetList.Add(_gameObject);
            }
        }
        //////////////////按照距离排序////////////////////////////
        return targetList;
    }
    void OnCollisionEnter(Collision collision)
    {
        //print("hit by:" + collision.gameObject.name);
        //if (collision.gameObject.tag != "FX")
        //{
        //    ContactPoint contact = collision.contacts[0];
        //    if (collision.gameObject.tag == "enemy")
        //    {
        //        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        //        Vector3 pos = contact.point;
        //        //Instantiate(impactPrefab, pos, rot);//敌人播放攻击特效
        //    }
        //}
    }
    void OnTriggerEnter(Collider hit)
    {
        //print("AIBase hit by:" + hit.gameObject.name + " self name is:" + gameObject.name);
        //if(dieParticle != null)
        {
            //print("hit by:" + collision.gameObject.name);
            OnHited(hit.gameObject);



        }
    }
    public void OnHited(GameObject hit)
    {
        if (hit.tag == EnityType.Bomb.ToString())//如果是子弹的话
        {
            if (selfType == EnityType.Enemy)
            {
                Detonate scriptDetonate = hit.GetComponent<Detonate>();
                if (scriptDetonate != null && scriptDetonate.IsCanDemage(gameObject.name))
                {
                    scriptDetonate.AddDamagedGameObject(gameObject.name);
                    TakeDamage(scriptDetonate.DetonateDamage);
                }
            }

        }
        else
        {
            Detonate scriptDetonate = hit.GetComponent<Detonate>();
            if (scriptDetonate != null && scriptDetonate.IsCanDemage(gameObject.name))
            {
                scriptDetonate.AddDamagedGameObject(gameObject.name);
                TakeDamage(scriptDetonate.DetonateDamage);
            }
        }
    }
    public void UpdateHP()
    {
        if (Camera.current != null)// && HP != MaxHp)
        {
            Vector3 hpPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);
            Vector3 screenPos = Camera.current.WorldToScreenPoint(hpPos);
            EnityManager.GetInstance().hpManager.UpdateEnityHPByEnityName(gameObject.name, screenPos, (float)HP / (float)MaxHp, (100 - gameObject.transform.position.z) / 100);
        }
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (gameObject.tag == EnityType.Hero.ToString())
        {
            print(gameObject.name + " 受到攻击：" + damage);
        }
        HP = HP < 0 ? 0 : HP;
        UpdateHP();
        if (HP <= 0)
        {
            KillSelf();
        }else{
            if(animator != null && animator.parameterCount > (int)AnimatorParameters.TakeDamage)
            {
                DoAction(AnimatorParameters.TakeDamage, ActionParamtersType.Trigger);
            }
            
        }
    }
    public void KillSelf()//自杀引起爆炸！
    {
        //if (dieParticle)
        {
            dieParticle = DataManager.GetInstance().CreateGameObjectFromAssetsBundle("enemy", "explosion_stylized_large_jokerFire");
            if(dieParticle != null)
            {
                dieParticle.transform.position = transform.position;
            }
        }
        if (animator != null && animator.parameterCount > (int)AnimatorParameters.Die)
        {
            DoAction(AnimatorParameters.Die, ActionParamtersType.Bool,1);
            Destroy(gameObject,3.0f);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
