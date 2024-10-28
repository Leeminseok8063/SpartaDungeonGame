using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMethod : MonoBehaviour
{
    [SerializeField] int bulletDamage = 0;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent<MonsterObject>(out MonsterObject mob))
        {
            mob.DamageToMonster(bulletDamage);
            Destroy(this.gameObject);
        }
    }
}
