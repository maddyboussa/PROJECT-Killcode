using Killcode.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    #region FIELDS
    [Header("General")]
    [SerializeField] protected WeaponStats weaponStats;
    [SerializeField] protected string weaponName;

    [Header("Attacks")]
    [SerializeField] protected string basicName;
    [SerializeField] protected string specialName;

    [Header("Cooldowns")]
    protected bool canBasic;
    protected bool canSpecial;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        canBasic = true;
        canSpecial = true;
    }

    /// <summary>
    /// Reduce the cooldown of a given ability
    /// </summary>
    /// <param name="cooldownToReduce">The WeaponStat cooldown to reference to</param>
    /// <param name="special">False if reducing the basic attack cooldown, True if reducing the special cooldown</param>
    /// <returns></returns>
    protected IEnumerator ReduceCooldown(WeaponStat cooldownToReduce, bool special)
    {
        float waitTime = weaponStats.GetStat(cooldownToReduce);
        float counter = 0f;

        while (counter < waitTime)
        {
            counter += Time.deltaTime;
            yield return null; //Don't freeze Unity
        }

        if(special)
        {
            canSpecial = true;
        } else
        {
            canBasic = true;
        }
    }

    public abstract void BasicAttack(Component sender, object data);

    public abstract void SpecialAttack(Component sender, object data);
}
