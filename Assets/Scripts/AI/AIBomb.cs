using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBomb : MonoBehaviour
{
    public float moveSpeed = 1000.0f;
    GameObject enemy;
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
    //float timer;
    // Start is called before the first frame update
    void Start()
    {
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (isPlaying)
        //{
        //    if (enemy != null)
        //    {
        //        transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);//先调整方向，朝向目标    
        //    }
        //    //////////////开始移动////////////////////////
        //    transform.position = transform.position + transform.forward.normalized * moveSpeed;

        //}
    }
    void FixedUpdate()
    {

        //CheckCollision(previousPosition);
    }
    void CheckCollision(Vector3 prevPos)
    {
        RaycastHit hit;
        Vector3 direction = transform.position - prevPos;
        Ray ray = new Ray(prevPos, direction);
        float dist = Vector3.Distance(transform.position, prevPos);
        if (Physics.Raycast(ray, out hit, dist))
        {
            transform.position = hit.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
            Vector3 pos = hit.point;
            if (impactParticle != null)
            {
                Instantiate(impactParticle, pos, rot);//敌人播放攻击特效
            }
            Destroy(gameObject);

        }
    }
    public void DoAction(GameObject gameObject)
    {
        enemy = gameObject;
        //isPlaying = true;
        if (enemy != null)
        {
            transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);//先调整方向，朝向目标    
        }
        //////////////开始移动////////////////////////
        //transform.position = transform.position + transform.forward.normalized * moveSpeed;
       
    }
    public void OnAttackTarget(GameObject gameObject)
    {
        if (gameObject != null)
        {
            transform.rotation = Quaternion.LookRotation(gameObject.transform.position - transform.position);//先调整方向，朝向目标
            //////////////开始移动////////////////////////
            transform.position = transform.position + transform.forward.normalized * moveSpeed;
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        //print("hit by:" + collision.gameObject.name);
        if (collision.gameObject.tag == EnityType.Enemy.ToString())
        {
            ContactPoint contact = collision.contacts[0];
            if(impactParticle != null)
            {
                Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
                Vector3 pos = contact.point;
                Instantiate(impactParticle, pos, rot);//敌人播放攻击特效
            }
            Destroy(gameObject);
        }
    }
}
