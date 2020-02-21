using UnityEngine;
using System.Collections;
 
public class HyperbitProjectileScript : MonoBehaviour
{
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
 
    private bool hasCollided = false;
    public bool isPlaying = false;
    public string attackTargetType;
    public float moveSpeed = 0.5f;
    public int hitDamage = 100;//伤害值

    public float destroyDelayTime = 3.0f;//自毁时间
    protected float timeCumulative = 0.0f;
    void Start()
    {
        transform.gameObject.tag = EnityType.Bomb.ToString();
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        if (muzzleParticle){
        muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
        Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
    }
    // Update is called once per frame
    void Update()
    {

        if (isPlaying)
        {
            //if (enemy != null)
            {
                //transform.rotation = Quaternion.LookRotation(enemy.transform.position - transform.position);//先调整方向，朝向目标    
            }
            //////////////开始移动////////////////////////
            transform.position = transform.position + transform.forward.normalized * moveSpeed;

        }
        if(timeCumulative > destroyDelayTime)
        {
            Destroy(gameObject);
        }
        timeCumulative += Time.deltaTime;
    }
    //void OnTriggerEnter(Collider hit)
    //{
    //    print("HyperbitProjectileScript hit by:" + hit.gameObject.name);
    //}
    void OnTriggerEnter(Collider hit)
    {
        //print("hit by:" + hit.gameObject.name);
        if (hit.gameObject.tag == attackTargetType)
        {
            if (!hasCollided)
            {
                hasCollided = true;
                //transform.DetachChildren();
                impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
                //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

                if (hit.gameObject.tag == "Destructible") // Projectile will destroy objects tagged as Destructible
                {
                    Destroy(hit.gameObject);
                }


                //yield WaitForSeconds (0.05);
                foreach (GameObject trail in trailParticles)
                {
                    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                    curTrail.transform.parent = null;
                    Destroy(curTrail, 3f);
                }
                Destroy(projectileParticle, 3f);
                Destroy(impactParticle, 5f);
                Destroy(gameObject);
                //projectileParticle.Stop();

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
                //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++)
                {
                    ParticleSystem trail = trails[i];
                    if (!trail.gameObject.name.Contains("Trail"))
                        continue;

                    trail.transform.SetParent(null);
                    Destroy(trail.gameObject, 2);
                }
            }
        }
    }
}