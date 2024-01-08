using DG.Tweening;
using UnityEngine;

/// <summary>
/// Processing class for activating effects of cards on the play board
/// </summary>
public class PlayBoardManager : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Each card zone on the play board (stored in order from left to right)
    /// </summary>
    [SerializeField]
    private CardZone[] boardCardZones = null;
    /// <summary>
    /// Transform of frame object for displaying cards during effect activation
    /// </summary>
    [SerializeField]
    private Transform playingCardFrameTrs = null;

    #endregion SerializeField

    #region Public Variables
    /// <summary>
    /// Number of card slots in the play board
    /// </summary>
    public const int PlayBoardCardNum = 5;
    /// <summary>
    /// Combat screen manager class
    /// </summary>
    [HideInInspector]
    public BattleManager battleManager;

    // Variables for storing persistent effects of cards (arrays for those storing effect amounts by character)
    // persistent system until the end of the battle
    /// <summary>
    /// // Number of times weapon damage was inflicted.
    /// </summary>
    public int[] weaponCount { get; private set; }
    /// <summary>
    /// Number of times HP was restored
    /// </summary>
    public int[] healCount { get; private set; }

    #endregion Public Variables

    /// <summary>
    /// Initial and end position of frame object (distance in X direction from edge card area)
    /// </summary>
    private const float FrameObjPositionFixX = 8.0f;
    /// <summary>
    /// Frame object movement animation time
    /// </summary>
    private const float FrameAnimTime = 0.3f;
    /// <summary>
    /// FieldManager class
    /// </summary>
    private FieldManager fieldManager;
    /// <summary>
    /// CharacterManager class
    /// </summary>
    private CharacterManager characterManager;
    /// <summary>
    /// Card Effect Execution Sequence
    /// </summary>
    private Sequence playSequence;
    /// <summary>
    /// Transform for each card zone
    /// </summary>
    private Transform[] cardZonesTrs;

    // Sustained system until end of turn
    /// <summary>
    /// "Leakage" damage reduction
    /// 「漏電」与ダメージ低下量
    /// </summary>
    private int leakagePoint;
    /// <summary>
    /// "Augmentation" damage reduction
    /// 「増強」与ダメージ低下量
    /// </summary>
    private int augmentPoint;
    /// <summary>
    /// Value that reduces damage received from opponents.
    /// 相手から受けるダメージを減らす数値
    /// </summary>
    private int[] defencePoint;
    /// <summary>
    /// Flag during application of recovery stop effect
    /// 回復停止効果適用中フラグ
    /// </summary>
    private bool[] noHealFlag;
    /// <summary>
    /// Flag during application of weapon disabling effect
    /// 武器無効効果適用中フラグ
    /// </summary>
    private bool[] noWeaponFlag;

    // Sustained system until the next card
    /// <summary>
    /// Character ID during the application of the "Kiai" effect
    /// 「気合」効果適用中のキャラID
    /// </summary>
    private int kiaiCharaID;
    /// <summary>
    /// Amount of "electrocution" damage reduction
    /// 「感電」与ダメージ低下量
    /// </summary>
    private int electrocutionPoint;
    /// <summary>
    /// Flag that blocks the activation of a card
    /// カードの発動を封じるフラグ
    /// </summary>
    private bool isSeal;
    /// <summary>
    /// Character ID whose card activation is blocked
    /// カードの発動が封じられているキャラID
    /// </summary>
    private int stunCharaID;

    #region Private Variables

    /// <summary>
    /// Time interval between each card execution
    /// </summary>
    private const float PlayIntervalTime = 0.2f;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Initialization (called from BattleManager.cs)
    /// </summary>
    /// <param name="battleManager"></param>
    public void Init(BattleManager battleManager)
    {
        this.battleManager = battleManager;
        fieldManager = battleManager.fieldManager;
        characterManager = battleManager.characterManager;
        // Initialize the card's persistence effect
        ClearAllSustainedEffects();
        // Get Transform for each card zone
        cardZonesTrs = new Transform[PlayBoardCardNum];
        for (int i = 0; i < PlayBoardCardNum; i++)
            cardZonesTrs[i] = boardCardZones[i].transform;
        // Move frame object to start position
        playingCardFrameTrs.position = GetFrameObjectStartPos();
    }
    /// <summary>
    /// Cards on the play board are activated in order from left to right
    /// </summary>
    /// <param name="boardCards">Card array to be activated</param>
    public void BoardCardsPlay(Card[] boardCards)
    {
        // Move frame object to start position
        playingCardFrameTrs.position = GetFrameObjectStartPos();
        // Sequence: Execute the effects of the cards in order
        playSequence = DOTween.Sequence();
        // index should not be here because it doesn't work in sequence.AppendCallback
        // int index = 0;
        for (int i = 0; i < PlayBoardCardNum; i++)
        {
            // Keep the number in the array that is currently being executed
            // (counter variable i is prepared because it has been set to 5 when the effect process is executed)
            int index = i;
            // Movement of frame object
            playSequence.Append(playingCardFrameTrs
                .DOMove(GetPlayZonePos(i), FrameAnimTime)
                .SetEase(Ease.OutQuart));
            // Processing for each card to be activated (skip if there is no card)
            if (boardCards[index] != null)
            {
                // Card exits
                // Get the character ID of the user of this card
                int useCharaID = boardCards[index].controllerCharaID;
                // Card effect activation
                playSequence.AppendCallback(() =>
                {
                    if (!isSeal && stunCharaID != useCharaID)
                    {
                        // Not sealed or stunned
                        // Effect activation
                        PlayCard(boardCards[index], useCharaID, index);
                    }
                    else
                    {
                        // If the card is sealed or stunned, the card does not trigger and the sustained effect is initialized
                        ClearOneCardSustainedEffects();
                    }

                });
                // Tween semi-transparent cards
                playSequence.Append(boardCards[index].HideFadeTween());
                // Check to see if the battle end conditions are met
                playSequence.AppendCallback(() =>
                {
                    if (characterManager.IsPlayerDefeated() ||  // Player Defeated
                        characterManager.IsEnemyDefeated())     // Defeat enemy
                    {
                        // Sequence termination
                        // TODO:
                        // playSequence.Complete();
                        // playSequence.Kill();
                        return;
                    }
                });
            }
            else
            {
                // No card exits
                playSequence.AppendCallback(() =>
                    ClearOneCardSustainedEffects()
                );
            }
            // Set time interval
            playSequence.AppendInterval(PlayIntervalTime);
        }
        // Move frame object to end position
        playSequence.Append(playingCardFrameTrs
            .DOMove(GetFrameObjectEndPos(), FrameAnimTime)
            .SetEase(Ease.OutQuart));
        // Processing at the end of Sequence
        playSequence.OnComplete(() =>
        {
            // Delete all card objects on the board
            foreach (var card in boardCards)
                if (card != null) fieldManager.DestroyCardObject(card);
            // Initialize all effects that last only one turn
            ClearOneTurnSustainedEffects();
            // End-of-turn call
            battleManager.TurnEnd();
        });
    }
    /// <summary>
    /// Get Vector2 coordinates of target play zone
    /// </summary>
    /// <param name="areaID">Zone ID (0-4)</param>
    /// <returns>Position value</returns>
    public Vector2 GetPlayZonePos(int areaID) =>
        cardZonesTrs[areaID].position;
    /// <summary>
    /// Initialize all persistent effects of the card
    /// </summary>
    public void ClearAllSustainedEffects()
    {
        // Initialize all effects that last only one turn
        ClearOneTurnSustainedEffects();
        // Restore the player's maximum HP
        characterManager.RecoverMaxHPPlayer();
        // Initialize the number of times weapon damage was inflicted
        weaponCount = new int[Card.CharaNum];
        // Initialize the number of times HP is recovered
        healCount = new int[Card.CharaNum];
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Activate all effects of card
    /// </summary>
    /// <param name="targetCard">Target card</param>
    /// <param name="useCharaID">Character ID of the user of this card</param>
    /// <param name="boardIndex">Order of this card on the play board (0-4)</param>
    /// <returns>Success flag of effect activation (true: activation succeeded)</returns>
    private bool PlayCard(Card targetCard, int useCharaID, int boardIndex)
    {
        // Target character ID
        int targetCharaID = useCharaID ^ 1;

        #region Amount of each effect of the card
        // Damage Amount
        int damagePoint = 0;
        // Amount of damage done to self
        int selfDamagePoint = 0;
        // Recovery amount
        int healPoint = 0;
        // Amount of damage to max HP
        int burnPoint = 0;
        // Amount of damage to own maximum HP
        int selfBurnPoint = 0;
        // Damage weakening amount
        int weakPoint = 0;
        // Damage multiplier
        int damageMulti = 1;
        // Physical absorption flag
        bool isAbsorption = false;
        // Recovery⇔Damage change flag
        bool isBloodPact = false;
        // Rebound damage flag
        bool isReflection = false;
        #endregion Amount of each effect of the card

        #region Get and reflect various sustained effects
        // Focus Energy(気合溜め)
        bool isKiai = kiaiCharaID == useCharaID;
        // Increase in "enhanced" damage(「増強」与ダメージ増加量)
        damagePoint += augmentPoint;
        // Amount of "electrocution" damage reduction(「感電」与ダメージ低下量)
        weakPoint += electrocutionPoint;
        // Amount of "leakage" damage reduction(「漏電」与ダメージ低下量)
        weakPoint += leakagePoint;
        #endregion Get and reflect various sustained effects

        // Clear sustained effects up to this card.
        ClearOneCardSustainedEffects();
        // Execute each effect in the card
        // 1: Effects that take precedence over other effects
        foreach (var effect in targetCard.effects)
        {
            switch (effect.cardEffect)
            {
                // Inversion(反転)
                case CardEffectDefine.CardEffect.Reverse:
                    useCharaID = targetCharaID;
                    targetCharaID = useCharaID ^ 1;
                    break;

                #region strength-limiting system

                // Intensity n limited(強度n限定)
                case CardEffectDefine.CardEffect.ForceEqual:
                    if (effect.value != targetCard.forcePoint)
                        // If card intensity is different from the specified value, all effects are nullified.
                        return false;
                    break;
                // Intensity n or greater(強度n以上)
                case CardEffectDefine.CardEffect.ForceHigher:
                    if (effect.value > targetCard.forcePoint)
                        // If the card strength is outside the specified range, all effects are disabled
                        return false;
                    break;
                // Intensity n or less(強度n以下)
                case CardEffectDefine.CardEffect.ForceLess:
                    if (effect.value < targetCard.forcePoint)
                        // If the card strength is outside the specified range, all effects are disabled
                        return false;
                    break;

                #endregion strength-limiting system
            }
        }
        // 2: Normal effect
        foreach (var effect in targetCard.effects)
        {
            switch (effect.cardEffect)
            {
                #region Damage effect

                // Damage(ダメージ)
                case CardEffectDefine.CardEffect.Damage:
                    damagePoint += effect.value;
                    break;
                // Weapon Damage(武器ダメージ)
                case CardEffectDefine.CardEffect.WeaponDmg:
                    if (!noWeaponFlag[targetCharaID])
                    {
                        // The opponent is not flagged as weapon disabled.
                        damagePoint += effect.value;
                        // Additional damage when "Kiai
                        if (isKiai) damagePoint += effect.value;
                        // Number of Attacks Count
                        weaponCount[useCharaID]++;
                    }
                    break;
                // assault(突撃)
                case CardEffectDefine.CardEffect.Assault:
                    if (boardIndex == 0)
                        damagePoint += effect.value;
                    break;
                // Pursuit(追撃)
                case CardEffectDefine.CardEffect.Pursuit:
                    if (boardIndex == PlayBoardCardNum - 1)
                        damagePoint += effect.value;
                    break;
                // Destroy(駆逐)
                case CardEffectDefine.CardEffect.CatchingUp:
                    if (characterManager.nowHP[targetCharaID] * 2 <= characterManager.maxHP[targetCharaID])
                        damagePoint += effect.value;
                    break;
                // Burns(火傷)
                case CardEffectDefine.CardEffect.Burn:
                    burnPoint += effect.value;
                    break;
                // Double damage(ダメージ２倍)
                case CardEffectDefine.CardEffect.DoubleDamage:
                    damageMulti *= 2;
                    break;
                // Damage tripled(ダメージ３倍)
                case CardEffectDefine.CardEffect.TripleDamage:
                    damageMulti *= 3;
                    break;
                // Hypergravity(超重力)
                case CardEffectDefine.CardEffect.Hypergravity:
                    damagePoint += characterManager.nowHP[targetCharaID] / 4;
                    break;
                // Self-injury(自傷)
                case CardEffectDefine.CardEffect.SelfInjury:
                    selfDamagePoint += effect.value;
                    break;
                // Reaction(反動)
                case CardEffectDefine.CardEffect.Reaction:
                    isReflection = true;
                    break;

                #endregion Damage effect

                #region recovery

                // Heal(回復)
                case CardEffectDefine.CardEffect.Heal:
                    healPoint += effect.value;
                    // Recovery Count(回復回数カウント)
                    healCount[useCharaID]++;
                    break;
                // SelfPredation(自己捕食)
                case CardEffectDefine.CardEffect.SelfPredation:
                    // Maximum HP reduced by half(HP最大値半減)
                    selfBurnPoint += characterManager.maxHP[useCharaID] / 2;
                    // Heal(回復)
                    healPoint += characterManager.maxHP[useCharaID] - characterManager.nowHP[useCharaID];
                    // Recovery Count(回復回数カウント)
                    healCount[useCharaID]++;
                    break;
                // Absorption(吸収)
                case CardEffectDefine.CardEffect.Absorption:
                    isAbsorption = true;
                    break;
                // Blood Pact(血の盟約)
                case CardEffectDefine.CardEffect.BloodPact:
                    isBloodPact = true;
                    break;

                #endregion recovery

                #region Support and interference system(支援・妨害系)

                // Weakening(弱体化)
                case CardEffectDefine.CardEffect.Weakness:
                    weakPoint += effect.value;
                    break;
                // Electric shock(感電)
                case CardEffectDefine.CardEffect.Electrocution:
                    electrocutionPoint += effect.value;
                    break;
                // Short circuit(漏電)
                case CardEffectDefine.CardEffect.Leakage:
                    leakagePoint += effect.value;
                    break;
                // Self-defense(防衛)
                case CardEffectDefine.CardEffect.Defense:
                    defencePoint[useCharaID] += effect.value;
                    break;
                // Augmentation(増強)
                case CardEffectDefine.CardEffect.Augment:
                    augmentPoint += effect.value;
                    break;
                // Focus Energy(気合溜め)
                case CardEffectDefine.CardEffect.Kiai:
                    kiaiCharaID = useCharaID;
                    break;
                // Recovery stop(回復停止)
                case CardEffectDefine.CardEffect.NoHeal:
                    noHealFlag[targetCharaID] = true;
                    break;
                // Seal(封印)
                case CardEffectDefine.CardEffect.Seal:
                    isSeal = true;
                    break;
                // Stun(スタン)
                case CardEffectDefine.CardEffect.Stun:
                    stunCharaID = useCharaID;
                    break;
                // Deck regeneration(山札再生)
                case CardEffectDefine.CardEffect.DeckRegen:
                    // Number of deck cards recovered only when used by player(プレイヤー使用時のみ山札枚数回復)
                    if (useCharaID == Card.CharaIDPlayer)
                        fieldManager.AddDeckCardsNum(effect.value);
                    break;
                // Weapon disabled(武器無効)
                case CardEffectDefine.CardEffect.NoWeapon:
                    noWeaponFlag[useCharaID] = true;
                    break;

                #endregion Support and interference system(支援・妨害系)

                #region Status Abnormalities(状態異常系)

                // Poison(毒)
                case CardEffectDefine.CardEffect.Poison:
                    characterManager.ChangeStatusEffect(targetCharaID, StatusEffectIcon.StatusEffectType.Poison, effect.value);
                    break;
                // Detoxification(解毒)
                case CardEffectDefine.CardEffect.Detox:
                    characterManager.ChangeStatusEffect(targetCharaID, StatusEffectIcon.StatusEffectType.Poison, -999);
                    break;
                // Flame(炎上)
                case CardEffectDefine.CardEffect.Flame:
                    characterManager.ChangeStatusEffect(targetCharaID, StatusEffectIcon.StatusEffectType.Flame, effect.value);
                    break;

                #endregion Status Abnormalities(状態異常系)
            }
        }

        // 3: Performing effects that refer to the number of actions
        foreach (var effect in targetCard.effects)
        {
            switch (effect.cardEffect)
            {
                // Rush(ラッシュ)
                case CardEffectDefine.CardEffect.Rush:
                    // Number of weapon attacks*3 damage(武器攻撃回数*3のダメージ)
                    damagePoint += weaponCount[useCharaID] * 3;
                    break;
                // HealRush(ヒールラッシュ)
                case CardEffectDefine.CardEffect.HealRush:
                    // Recovery times*3(回復回数*3の回復)
                    healPoint += healCount[useCharaID] * 3;
                    // Heal count(回復回数カウント)
                    healCount[useCharaID]++;
                    break;
            }
        }

        // Blood pact (healing to damage) effect applied(血の盟約(回復を与ダメージに)効果適用)
        if (isBloodPact)
        {
            damagePoint += healPoint;
            healPoint = 0;
        }
        // Weakness and multiplier applied to the amount of damage inflicted
        // Weakening application(弱体化適用)
        damagePoint -= weakPoint;
        // Application of Weakening by Defense(防衛による弱体化適用)
        damagePoint -= defencePoint[targetCharaID];
        // Damage scaling factor applied(ダメージ倍率適用)
        damagePoint = damagePoint * damageMulti;
        // Ensure that the damage is not negative(ダメージがマイナスにならないようにする)
        if (damagePoint < 0) damagePoint = 0;
        // Calculate the damage to self from the reaction(反動による自身へのダメージを計算)
        if (isReflection) selfDamagePoint += damagePoint / 2;

        // Various calculated values are applied to each target(各種計算数値を対象ごとに適用)
        // Damage to max HP
        characterManager.ChangeStatusMaxHP(targetCharaID, -burnPoint);
        // Damage
        characterManager.ChangeStatusNowHP(targetCharaID, -damagePoint);
        // Damage to own maximum HP
        characterManager.ChangeStatusMaxHP(useCharaID, -selfBurnPoint);
        // Damage to self
        characterManager.ChangeStatusNowHP(useCharaID, -selfDamagePoint);
        // Heal
        if (!noHealFlag[useCharaID])
        {
            characterManager.ChangeStatusNowHP(useCharaID, healPoint);
            // Energy absorption(体力吸収)
            if (isAbsorption)
                characterManager.ChangeStatusNowHP(useCharaID, damagePoint);
        }
        return true;
    }
    /// <summary>
    /// Returns true if the effect of the card is being executed
    /// </summary>
    /// <returns></returns>
    private bool IsPlayingCards()
    {
        if (playSequence != null) return playSequence.IsPlaying();
        return false;
    }
    /// <summary>
    /// Get the start position of the frame object
    /// </summary>
    /// <returns></returns>
    private Vector2 GetFrameObjectStartPos()
    {
        Vector2 res = cardZonesTrs[0].position;
        res.x -= FrameObjPositionFixX;
        return res;
    }
    /// <summary>
    /// Get the end position of the frame object
    /// </summary>
    /// <returns></returns>
    private Vector2 GetFrameObjectEndPos()
    {
        Vector2 res = cardZonesTrs[PlayBoardCardNum - 1].position;
        res.x += FrameObjPositionFixX;
        return res;
    }
    /// <summary>
    /// Initialize all effects that last only one turn
    /// </summary>
    private void ClearOneTurnSustainedEffects()
    {
        // Initialize all effects that last only for one card
        ClearOneCardSustainedEffects();
        // Leakage point initialization(漏電ポイント初期化)
        leakagePoint = 0;
        // Incremental point initialization(増強ポイント初期化)
        augmentPoint = 0;
        // Initialization of defense points(防衛ポイント初期化)
        defencePoint = new int[Card.CharaNum];
        // Recovery stop effect flag initialization(回復停止効果フラグ初期化)
        noHealFlag = new bool[Card.CharaNum];
        // Initialization of weapon disabling effect flag(武器無効効果フラグ初期化)
        noWeaponFlag = new bool[Card.CharaNum];
    }
    /// <summary>
    /// Initialize all effects that last only for one card
    /// </summary>
    private void ClearOneCardSustainedEffects()
    {
        // Aiki-Tamenuri End(気合溜め終了)
        kiaiCharaID = Card.CharaIDNone;
        // Initialization of damage reduction(与ダメージ低下量初期化)
        electrocutionPoint = 0;
        // Card seal flag initialization(カード封印フラグ初期化)
        isSeal = false;
        stunCharaID = Card.CharaIDNone;
    }

    #endregion Private Methods

    #endregion Methods
}
