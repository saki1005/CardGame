using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GamePlayerManager : MonoBehaviour
{
    public List<int> deck = new List<int>() {};

    public int heroHp;
    public int manaCost;
    public int defaultManaCost;

    public void Init(List<int> cardDeck)
    {
        this.deck = cardDeck;
        heroHp = 10;
        manaCost = 1;
        defaultManaCost = 1;
    }
}
