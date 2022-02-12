using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] AI enemyAI;
    [SerializeField] UIManager uIManager;
    public GamePlayerManager player;
    public GamePlayerManager enemy;
    [SerializeField] GamePlayerManager gamePlayerManager;

    public Transform playerHandTransform,
                               playerFieldTransform,
                               enemyHandTransform,
                               enemyFieldTransform;
    [SerializeField] CardController cardPrefab;

    public bool isPlayerTurn;



    public Transform playerHero;




    // 時間管理
    public int timeCount;



    // シングルトン化（どこからでもアクセスできるようにする）
    public static GameManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        uIManager.HideResultPanel();
        uIManager.ShowHeroHp(player.heroHp, enemy.heroHp);
        uIManager.ShowManaCost(player.manaCost, enemy.manaCost);
        SettingInitHand();
        isPlayerTurn = true;
        TurnCalc();
    }


    public void ReduceManaCost(int cost, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            player.manaCost -= cost;
        }
        else
        {
            enemy.manaCost -= cost;
        }
        uIManager.ShowManaCost(player.manaCost, enemy.manaCost);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SettingInitHand()
    {
        // カードをそれぞれに３枚配る
        for (int i = 0; i < 3; i++)
        {
            GiveCardToHand(player.deck, playerHandTransform);
            GiveCardToHand(enemy.deck, enemyHandTransform);
        }
    }

    void GiveCardToHand(List<int> deck, Transform hand)
    {
        if (deck.Count == 0 || playerHandTransform.GetComponentsInChildren<CardController>().Length >= 5)
        {
            return;
        }
        int cardID = deck[0];
        deck.RemoveAt(0);
        CreateCard(cardID, hand);
    }


    void CreateCard(int cardID, Transform hand)
    {
        CardController card = Instantiate(cardPrefab, hand, false);
        if (hand.name == "PlayerHand")
        {
            card.Init(cardID, true);

        }
        else
        {
            card.Init(cardID, false);

        }
    }

    void TurnCalc()
    {
        StopAllCoroutines();
        StartCoroutine(CountDown());
        if (isPlayerTurn)
        {
            PlayerTurn();
        }
        else
        {
            StartCoroutine(enemyAI.EnemyTurn());
        }
    }

    IEnumerator CountDown()
    {
        timeCount = 10;
        uIManager.UpdateTime(timeCount);

        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1); // １秒待機
            timeCount--;
            uIManager.UpdateTime(timeCount);
        }
        ChangeTurn();
    }

    public void OnClickEndButton()
    {
        if (isPlayerTurn)
        {
            ChangeTurn();
        }
    }

    public CardController[] GetEnemyFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return enemyFieldTransform.GetComponentsInChildren<CardController>();
        }
        return playerFieldTransform.GetComponentsInChildren<CardController>();
    }

    public CardController[] GetFriendFieldCards(bool isPlayer)
    {
        if (isPlayer)
        {
            return playerFieldTransform.GetComponentsInChildren<CardController>();
        }
        return enemyFieldTransform.GetComponentsInChildren<CardController>();
    }

    public void ChangeTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, false);
        CardController[] enemyFieldCardList = enemyFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(enemyFieldCardList, false);

        if (isPlayerTurn)
        {
            player.defaultManaCost++;
            player.manaCost = player.defaultManaCost;
            GiveCardToHand(player.deck, playerHandTransform);
        }
        else
        {
            enemy.defaultManaCost++;
            enemy.manaCost = enemy.defaultManaCost;
            GiveCardToHand(enemy.deck, enemyHandTransform);
        }
        uIManager.ShowManaCost(player.manaCost, enemy.manaCost);
        TurnCalc();
    }

    public void SettingCanAttackView(CardController[] fieldCardList, bool canAttack)
    {
        foreach(CardController card in fieldCardList){
        card.SetCanAttack(canAttack);
        }
    }

    void PlayerTurn()
    {
        // フィールドのカードを攻撃可能にする
        CardController[] playerFieldCardList = playerFieldTransform.GetComponentsInChildren<CardController>();
        SettingCanAttackView(playerFieldCardList, true);

        foreach (CardController card in playerFieldCardList)
        {

            card.SetCanAttack(true); // カードを攻撃可能にする
        }

    }

    

    public void CardsBattle(CardController attacker, CardController defender)
    {
        attacker.Attack(defender);
        defender.Attack(attacker);

        attacker.CheckAlive();
        defender.CheckAlive();
    }

    public void AttackToHero(CardController attacker)
    {
        if (attacker.model.isPlayerCard)
        {
            enemy.heroHp -= attacker.model.at;
        }
        else
        {
            player.heroHp -= attacker.model.at;
        }
        attacker.SetCanAttack(false);
        uIManager.ShowHeroHp(player.heroHp, enemy.heroHp);
    }

    public void HealToHero(CardController healer)
    {
        if (healer.model.isPlayerCard)
        {
            player.heroHp += healer.model.at;
        }
        else
        {
            enemy.heroHp += healer.model.at;
        }
        uIManager.ShowHeroHp(player.heroHp, enemy.heroHp);
    }

    public void CheckHeroHp()
    {
        if (player.heroHp <= 0 || enemy.heroHp <= 0)
        {
            ShowResultPanel(player.heroHp);

        }
    }

    void ShowResultPanel(int heroHp)
    {
        StopAllCoroutines();
        uIManager.ShowResultPanel(heroHp);
    }

}
