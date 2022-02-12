using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public string name;
    public int hp;
    public int at;
    public int cost;
    public Sprite icon;
    public Sprite bg;
    public ABILITY ability;
    public SPELL spell;
    public bool isAlive;
    public bool canAttack;
    public bool isFieldCard;
    public bool isPlayerCard;

    public CardModel(int cardID, bool isPlayer)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card"+cardID);
        name = cardEntity.name;
        hp = cardEntity.hp;
        at = cardEntity.at;
        cost = cardEntity.cost;
        icon = cardEntity.icon;
        bg = cardEntity.bg;
        ability = cardEntity.ability;
        spell = cardEntity.spell;

        isAlive = true;
        isPlayerCard = isPlayer;
    }

    void Damage(int dmg)
    {
        hp -= dmg;
        if(hp <= 0)
        {
            hp = 0;
            isAlive = false;
        }
    }

    void RecoveryHP(int point)
    {
        hp += point;
    }
    public void Heal(CardController card)
    {
        card.model.RecoveryHP(at);
    }

    public void Attack(CardController card)
    {
        card.model.Damage(at); 
    }
}
