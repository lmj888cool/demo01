using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bombPrefab;//子弹实体
    public GameObject muzzleflare;//枪口开枪特效
    public float min, max;
    public bool rapidFire;//是否有火焰
    public float rapidFireCooldown;//火焰冷却时间 

    public bool shotgunBehavior;//是否散弹
    public int shotgunPellets;//散弹个数
    public GameObject shellPrefab;//弹壳特效
    public bool hasShells;//是否播放开枪后弹壳特效



    public float attackSpeed = 1f;
    public float attackRadius;
    public int damaged;
    public int bombCount;
    public float bombMoveSpeed;
    public bool isHaveBomb;


    public float timeCumulative = float.MaxValue;
    public bool isCanAttack = false;
    public Transform spawnPosition;
    public GameObject target;//目标
    public Item equipItem;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeCumulative += Time.deltaTime;
    }
    public void OnAttackTarget(GameObject gameObject,float delay)
    {
        if (isHaveBomb)
        {
            if (bombPrefab != null)
            {
                target = gameObject;
                //Invoke("Do", delay-0.1f);
                //GameObject projectile = Instantiate(bombPrefab, spawnPosition.position, Quaternion.identity) as GameObject;
                //projectile.transform.LookAt(target.transform.position);
                //projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * 1000);

                GameObject projectile = Instantiate(bombPrefab, spawnPosition.position, Quaternion.identity) as GameObject;
                Vector3 pos = new Vector3(target.transform.position.x, Random.Range(0.0f, 2.0f), target.transform.position.z);
                projectile.transform.LookAt(pos);
                projectile.GetComponent<HyperbitProjectileScript>().isPlaying = true;
                projectile.GetComponent<HyperbitProjectileScript>().attackTargetType = target.gameObject.tag;
            }
        }
        else
        {
            AIBase aIBase = gameObject.GetComponent<AIBase>();
            if (aIBase)
            {
                aIBase.OnHited(this.gameObject);
            }
        }


    }
    private void Do()
    {
        //GameObject Shotgun;
        //Shotgun = Instantiate(bombPrefab, transform.TransformPoint(bombStartPos), bombPrefab.transform.rotation);
        //Shotgun.rotation = Quaternion.LookRotation(target.transform.position - transform.position);//先调整方向，朝向目标
        //bomb.GetComponent<AIBomb>().DoAction(target);
        //Shotgun.AddForce(Shotgun.transform.forward * Random.Range(1600,2100));
        if (bombPrefab != null)
        {
            GameObject projectile = Instantiate(bombPrefab, spawnPosition.position, Quaternion.identity) as GameObject;
            projectile.transform.LookAt(target.transform.position);
            projectile.GetComponent<HyperbitProjectileScript>().isPlaying = true;
        }
        
        //projectile.GetComponent<HyperbitProjectileScript>().impactNormal = hit.normal;
    }
    public void InitWeaponByEquip(Item equip)
    {
        if(bombPrefab != null)
        {
            Destroy(bombPrefab);
            bombPrefab = null;
        }
        ItemTableData itemTableData = DataManager.GetInstance().GetItemTableDataByItem(equip);
        this.equipItem = equip;
        this.attackSpeed = itemTableData.attackSpeed;
        this.attackRadius = itemTableData.attackRadius;
        this.damaged = itemTableData.damaged;
        this.bombCount = itemTableData.bombCount;
        this.bombMoveSpeed = itemTableData.bombMoveSpeed;
        this.isHaveBomb = itemTableData.isBomb == 1;
        if (this.isHaveBomb)//一般远程攻击才有子弹
        {

        }
        else//如果不是表示近战
        {
            Detonate detonate = this.gameObject.AddComponent<Detonate>();
            detonate.DetonateDamage = this.damaged;
            detonate.DamageCount = int.MaxValue;//近战武器无限攻击次数
        }
    }
}
