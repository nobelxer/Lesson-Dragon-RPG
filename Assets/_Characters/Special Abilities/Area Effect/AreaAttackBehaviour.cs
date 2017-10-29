using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;
using RPG.Core;

public class AreaAttackBehaviour : MonoBehaviour, ISpacialAbility {

    AreaEffectConfig config;

    public void SetConfig(AreaEffectConfig configToSet)
    {
        this.config = configToSet;
    }

	// Use this for initialization
	void Start () {
        print("Area Effect Behaviour attached to " + gameObject.name);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Use(AbilityUseParams useParams)
    {
        print("Area Effect used by " + gameObject.name);
        RaycastHit[] hits = Physics.SphereCastAll(
            transform.position,
            config.GetRadius(),
            Vector3.up,
            config.GetRadius()
        );
        foreach(RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget(); //TODO ok rick?
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
