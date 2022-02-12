using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 攻撃される側
public class SpellDropManager : MonoBehaviour, IDropHandler

{
    public void OnDrop(PointerEventData eventData)
    {
        /* 攻撃 */
        // attackerカードを選択
        CardController spellCard = eventData.pointerDrag.GetComponent<CardController>();
        // defenderカードを選択
        CardController target = GetComponent<CardController>();

        if (spellCard == null || spellCard.model.cost > GameManager.instance.player.manaCost)
        {
            return;
        }
        if (spellCard.CanUseSpell())
        {
            spellCard.UseSpellTo(target);
        }
        
    }
}
