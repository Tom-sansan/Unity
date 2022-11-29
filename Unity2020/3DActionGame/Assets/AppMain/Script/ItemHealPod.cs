using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Heal Item Class
/// </summary>
public class ItemHealPod : ItemBase
{
    /// <summary>
    /// Amount of recovery
    /// </summary>
    [Header("Item Param")]
    [SerializeField]
    private int healPoint = 10;

    protected override void Start()
    {
        base.Start();
    }

    protected override void ItemAction(Collider collider)
    {
        base.ItemAction(collider);
        var player = collider.gameObject.GetComponent<PlayerController>();
        player.OnHeal(healPoint);
    }
}
