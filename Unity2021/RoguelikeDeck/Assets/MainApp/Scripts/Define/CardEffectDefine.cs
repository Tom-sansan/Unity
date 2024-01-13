using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Card Effect Define Class
/// </summary>
[Serializable]
public class CardEffectDefine
{
    #region Enum

    /// <summary>
    /// Card Effect Definition
    /// </summary>
    public enum CardEffect
    {
        Damage, // Damage (ダメージ)
        WeaponDmg, // Weapon Damage (武器ダメージ)
        Assault, // Assault (突撃)
        Pursuit, // Pursuit (追撃)
        CatchingUp, // Pursuit (追い討ち)
        Burn, // Burn (火傷)
        DoubleDamage, // double damage (ダメージ２倍)
        TripleDamage, // triple damage (ダメージ３倍)
        Hypergravity, // Hypergravity (超重力)
        SelfInjury, // self-injury (自傷)
        Rush, // Rush (ラッシュ)

        Heal, // Recovery (回復)
        SelfPredation, // Self-Predation (自己捕食)
        Absorption, // Absorption (吸収)
        BloodPact, // Blood Pact (血の盟約)

        HealRush, // Heal Rush (ヒールラッシュ)
        Weakness, // Weakness (弱体化)
        Electrocution, // Electric Shock (感電)
        Leakage, // Leakage (漏電)
        Defense, // Defense (防衛)
        Augment, // Augmentation (増強)
        Kiai, // Kiai Tame (気合溜め)
        NoHeal, // No Recovery (回復停止)
        Seal, // seal (封印)
        Stun, // stun (スタン)
        DeckRegen, // deck regeneration (山札再生)
        Reverse, // Reverse (反転)

        Poison, // Poison (毒)
        Detox, // Detoxification (解毒)
        Flame, // Flame (炎上)

        ForceEqual, // limited to strength n (強度n限定)
        ForceHigher, // above intensity n (強度n以上)
        ForceLess, // strength n or less (強度n以下)
        BaseOnly, // limited to body (本体限定)
        PartsOnly, // limited to material (素材限定)
        NoCompo, // Composites are not possible (合成不可)

        Reaction, // Reaction (反動)
        NoWeapon, // weapon disabled (武器無効)

        BonusGold, // (bonus) money ((ボーナス用)お金)
        BonusEXP, // (bonus) experience ((ボーナス用)経験値)
        BonusHeal, // (Bonus) strength recovery ((ボーナス用)体力回復)

        _MAX,
    }
    /// <summary>
    /// Composite mode definition
    /// </summary>
    public enum EffectCompoMode
    {
        Possible,   // Composable
        Impossible, // Composite impossible
        OnlyNew,    // New addition only
        OnlyOwn,    // Composite is possible only between player and card
        OnlyOwnNew  // Composite is possible only between player and card (new addition only)
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables

    /// <summary>
    /// Card effect
    /// </summary>
    [Header("効果の種類")]
    public CardEffect cardEffect;
    /// <summary>
    /// Effect value
    /// </summary>
    [Header("効果値")]
    public int value;

    #region Effect type definition

    /// <summary>
    /// Effect name (JP)
    /// </summary>
    public static readonly Dictionary<CardEffect, string> DicEffectNameJP = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage, "ダメージ {0}" },
        {CardEffect.Heal, "回復 {0}" },
        {CardEffect.Weakness, "弱体化 {0}" },
        {CardEffect.DoubleDamage, "ダメージ二倍" },
        {CardEffect.Absorption, "体力吸収" },
        {CardEffect.ForceEqual, "強度 {0} 限定" },
        {CardEffect.Assault, "突撃 {0}" },
        {CardEffect.Pursuit, "追撃 {0}" },
        {CardEffect.WeaponDmg, "武器ダメージ {0}" },
        {CardEffect.Kiai, "気合溜め" },
        {CardEffect.Rush, "ラッシュ" },
        {CardEffect.HealRush, "ヒールラッシュ" },
        {CardEffect.SelfPredation, "自己捕食" },
        {CardEffect.BloodPact, "血の盟約" },
        {CardEffect.Poison, "毒 {0}" },
        {CardEffect.Detox, "解毒" },
        {CardEffect.Defense, "防衛 {0}" },
        {CardEffect.Seal, "封印" },
        {CardEffect.Burn, "火傷 {0}" },
        {CardEffect.Flame, "炎上 {0}" },
        {CardEffect.CatchingUp, "駆逐 {0}" },
        {CardEffect.Electrocution, "感電 {0}" },
        {CardEffect.Leakage, "漏電 {0}" },
        {CardEffect.Augment, "増強 {0}" },
        {CardEffect.TripleDamage, "ダメージ三倍" },
        {CardEffect.Reverse, "反転" },
        {CardEffect.Hypergravity, "超重力" },
        {CardEffect.ForceHigher, "強度 {0} 以上" },
        {CardEffect.ForceLess, "強度 {0} 以下" },
        {CardEffect.BaseOnly, "本体限定" },
        {CardEffect.PartsOnly, "素材限定" },
        {CardEffect.NoCompo, "合成無効" },
        {CardEffect.SelfInjury, "自傷 {0}" },
        {CardEffect.DeckRegen, "山札再生 {0}" },
        {CardEffect.NoHeal, "回復停止" },
        {CardEffect.Stun, "スタン" },
        {CardEffect.Reaction, "反動" },
        {CardEffect.NoWeapon, "武器無効" },
        {CardEffect.BonusGold, "金貨 {0}" },
        {CardEffect.BonusEXP, "経験値 {0}" },
        {CardEffect.BonusHeal, "回復 {0}" },
    };
    /// <summary>
    /// Effect name (EN)
    /// </summary>
    public static readonly Dictionary<CardEffect, string> DicEffectNameEN = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage, "Damage {0}" },
        {CardEffect.Heal, "Heal {0}" },
        {CardEffect.Weakness, "Weakness {0}" },
        {CardEffect.DoubleDamage, "Double Damage" },
        {CardEffect.Absorption, "Absorption" },
        {CardEffect.ForceEqual, "Force only {0}" },
        {CardEffect.Assault, "Assault {0}" },
        {CardEffect.Pursuit, "Pursuit {0}" },
        {CardEffect.WeaponDmg, "Weapon Dmg {0}" },
        {CardEffect.Kiai, "Kiai" },
        {CardEffect.Rush, "Rush" },
        {CardEffect.HealRush, "Heal rush" },
        {CardEffect.SelfPredation, "Self predation" },
        {CardEffect.BloodPact, "Blood pact" },
        {CardEffect.Poison, "Poison {0}" },
        {CardEffect.Detox, "Detox" },
        {CardEffect.Defense, "Defense {0}" },
        {CardEffect.Seal, "Seal" },
        {CardEffect.Burn, "Burn {0}" },
        {CardEffect.Flame, "Flame {0}" },
        {CardEffect.CatchingUp, "Catching Up {0}" },
        {CardEffect.Electrocution, "Electrocution {0}" },
        {CardEffect.Leakage, "Leakage {0}" },
        {CardEffect.Augment, "Augment {0}" },
        {CardEffect.TripleDamage, "Triple Damage" },
        {CardEffect.Reverse, "Reverse" },
        {CardEffect.Hypergravity, "Hypergravity" },
        {CardEffect.ForceHigher, "Force {0} or higher" },
        {CardEffect.ForceLess, "Force {0} or less" },
        {CardEffect.BaseOnly, "Base only" },
        {CardEffect.PartsOnly, "Parts Only" },
        {CardEffect.NoCompo, "No Compo" },
        {CardEffect.SelfInjury, "Self injury {0}" },
        {CardEffect.DeckRegen, "Deck Regen {0}" },
        {CardEffect.NoHeal, "No Heal" },
        {CardEffect.Stun, "Stun" },
        {CardEffect.Reaction, "Reaction" },
        {CardEffect.NoWeapon, "No Weapon" },
        {CardEffect.BonusGold, "Gold {0}" },
        {CardEffect.BonusEXP, "EXP {0}" },
        {CardEffect.BonusHeal, "Heal {0}" },
    };
    /// <summary>
    /// Effect Description (JP)
    /// </summary>
    public static readonly Dictionary<CardEffect, string> DicEffectExplainJP = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage, "相手に {0} のダメージを与える" },
        {CardEffect.Heal, "自分の体力を {0} 回復する" },
        {CardEffect.Weakness, "ダメージの効果を {0} 減少させる\n(マイナスにはならない)" },
        {CardEffect.DoubleDamage, "相手に与えるダメージを２倍にする\n(状態異常には無効)(同名効果と重複なし)" },
        {CardEffect.Absorption, "相手に与えたダメージ分自身を回復" },
        {CardEffect.ForceEqual, "カードの強度が {0} の場合のみ発動" },
        {CardEffect.Assault, "左端でこのカードが発動された時\n{0} のダメージを与える" },
        {CardEffect.Pursuit, "右端でこのカードが発動された時\n{0} のダメージを与える" },
        {CardEffect.WeaponDmg, "相手に {0} のダメージを与える" },
        {CardEffect.Kiai, "右隣のカードで自分が与える武器ダメージを2倍にする" },
        {CardEffect.Rush, "武器ダメージ効果を持つカードの使用回数×3ポイントのダメージ\n (現在 {0} 回・戦闘後にリセット)" },
        {CardEffect.HealRush, "回復効果を持つカードの使用回数×3ポイント体力を回復\n (現在 {0} 回・戦闘後にリセット)" },
        {CardEffect.SelfPredation, "自分の体力を全回復するが、最大体力を半分にする" },
        {CardEffect.BloodPact, "このカードの体力回復量を\n相手に与えるダメージ量に変更する" },
        {CardEffect.Poison, "相手に毒を {0} 与える\n(毒：ターン終了時にダメージ)" },
        {CardEffect.Detox, "自分の毒を取り除く" },
        {CardEffect.Defense, "ターン終了まで相手が与えるダメージを {0} 減らす" },
        {CardEffect.Seal, "右隣のカードの発動を封じる" },
        {CardEffect.Burn, "相手の最大体力を {0} 減らす" },
        {CardEffect.Flame, "相手に炎上を {0} 与える\n(炎上：最大体力減少時に追加ダメージ)" },
        {CardEffect.CatchingUp, "発動時に相手の体力が半分以下の時\n{0} の追加ダメージを与える" },
        {CardEffect.Electrocution, "右隣のカードが与えるダメージを {0} 減らす" },
        {CardEffect.Leakage, "次以降の全てのカードが与えるダメージを {0} 減らす\n(ターン終了まで)" },
        {CardEffect.Augment, "次以降の全てのカードが与えるダメージを {0} 増やす\n(ターン終了まで)(ダメージ効果が無くても対象)" },
        {CardEffect.TripleDamage, "相手に与えるダメージを３倍にする\n(状態異常には無効)(同名効果と重複なし)" },
        {CardEffect.Reverse, "このカードが相手に与える効果を自分に、\n自分が受ける効果を相手に与える" },
        {CardEffect.Hypergravity, "相手の残り体力の4分の1のダメージを与える" },
        {CardEffect.ForceHigher, "カードの強度が {0} 以上の場合のみ発動" },
        {CardEffect.ForceLess, "カードの強度が {0} 以下の場合のみ発動" },
        {CardEffect.BaseOnly, "このカードの合成は本体としてのみ可能" },
        {CardEffect.PartsOnly, "このカードの合成は素材としてのみ可能" },
        {CardEffect.NoCompo, "このカードは合成できない" },
        {CardEffect.SelfInjury, "自分は {0} の体力を失う" },
        {CardEffect.DeckRegen, "デッキの残りカード枚数を {0} 枚追加する" },
        {CardEffect.NoHeal, "ターン終了まで相手の体力回復を封じる" },
        {CardEffect.Stun, "右隣の自分のカードは発動できない" },
        {CardEffect.Reaction, "相手に与えたダメージの半分だけ自分の残り体力を減らす\n（端数切り捨て）" },
        {CardEffect.NoWeapon, "ターン終了まで相手から武器ダメージを受けない" },
        {CardEffect.BonusGold, "(ボーナス専用)\nお金を{0}得る" },
        {CardEffect.BonusEXP, "(ボーナス専用)\n経験値を{0}得る" },
        {CardEffect.BonusHeal, "(ボーナス専用)\n体力を{0}回復する" },
    };
    /// <summary>
    /// Effect Description (EN)
    /// </summary>
    public static readonly Dictionary<CardEffect, string> DicEffectExplainEN = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage, "Deals {0} damage to the opponent" },
        {CardEffect.Heal, "heals {0} of own HP" },
        {CardEffect.Weakness, "Decreases damage by {0} (not negative)" },
        {CardEffect.DoubleDamage, "Double the damage dealt to the opponent\n(not valid for abnormal conditions)(No overlap with effect of same name)" },
        {CardEffect.Absorption, "Heals itself by the amount of damage\ninflicted on the opponent" },
        {CardEffect.ForceEqual, "Activates only when the force of the card is {0}" },
        {CardEffect.Assault, "When this card is activated on the left side,\nit deals {0} damage" },
        {CardEffect.Pursuit, "When this card is activated on the right side,\nit deals {0} damage" },
        {CardEffect.WeaponDmg, "Deals {0} damage to the opponent" },
        {CardEffect.Kiai, "Double the weapon damage you deal\nwith the card to your right" },
        {CardEffect.Rush, "Number of times a card with weapon damage effect is used x 3 points\nof damage (currently {0} times, reset after the battle)" },
        {CardEffect.HealRush, "Number of times a card with recovery effect is used x 3 points of\nrecovery (currently {0} times, reset after each battle)" },
        {CardEffect.SelfPredation, "Restores full own HP,\nbut reduces maximum strength to half" },
        {CardEffect.BloodPact, "Change the amount of HP heal of this card to\nthe amount of damage dealt to the opponent" },
        {CardEffect.Poison, "Deals {0} poison to the opponent\n(Poison : Damage at end of turn)" },
        {CardEffect.Detox, "Removes own poison" },
        {CardEffect.Defense, "Reduces damage dealt by the opponent by {0} until end of turn" },
        {CardEffect.Seal, "Block the activation of the card to the right" },
        {CardEffect.Burn, "Reduces the opponent's maximum HP by {0}" },
        {CardEffect.Flame, "Deals {0} flame to the opponent\n(Additional damage when maximum HP is reduced)" },
        {CardEffect.CatchingUp, "When the opponent's strength is less than half at\nthe time of activation, {0} additional damage" },
        {CardEffect.Electrocution, "Reduces the damage dealt by the card to the right\nby {0}" },
        {CardEffect.Leakage, "Reduces the damage dealt by all subsequent cards\nby {0} (until end of turn)" },
        {CardEffect.Augment, "Increases the damage dealt by all subsequent cards\nby {0}(until end of turn)(even if there is no damage effect)" },
        {CardEffect.TripleDamage, "Triple the damage dealt to the opponent\n(not valid for abnormal conditions)(No overlap with effect of same name)" },
        {CardEffect.Reverse, "The effect this card gives to the opponent is given to you,\nand the effect you receive is given to the opponent." },
        {CardEffect.Hypergravity, "Deals 1/4 of the damage of the opponent's remaining HP" },
        {CardEffect.ForceHigher, "Only activated if the force of the card is {0} or more" },
        {CardEffect.ForceLess, "Only activated if the force of the card is {0} or less" },
        {CardEffect.BaseOnly, "This card can only be synthesized as a base" },
        {CardEffect.PartsOnly, "This card can only be synthesized as a parts" },
        {CardEffect.NoCompo, "This card cannot be Composability" },
        {CardEffect.SelfInjury, "Lose {0} HP" },
        {CardEffect.DeckRegen, "Adds {0} cards to the number of cards remaining in the deck" },
        {CardEffect.NoHeal, "Block the opponent's HP recovery until the end of the turn" },
        {CardEffect.Stun, "Your right neighbor's card cannot be activated" },
        {CardEffect.Reaction, "Reduce your HP by half of the damage inflicted\non the opponent (rounding down fractions)" },
        {CardEffect.NoWeapon, "No weapon damage from opponent until end of turn." },
        {CardEffect.BonusGold, "(Bonus only)\nGain {0} Gold" },
        {CardEffect.BonusEXP, "(Bonus only)\nGain {0} EXP" },
        {CardEffect.BonusHeal, "(Bonus only)\nRestore HP {0} times" },
    };
    /// <summary>
    /// Composite mode
    /// </summary>
    public static readonly Dictionary<CardEffect, EffectCompoMode> DicEffectCompoMode = new Dictionary<CardEffect, EffectCompoMode>()
    {
        {CardEffect.Damage, EffectCompoMode.Possible },
        {CardEffect.Heal, EffectCompoMode.Possible },
        {CardEffect.Weakness, EffectCompoMode.Possible },
        {CardEffect.DoubleDamage, EffectCompoMode.Possible },
        {CardEffect.Absorption, EffectCompoMode.Possible },
        {CardEffect.ForceEqual, EffectCompoMode.OnlyOwnNew },
        {CardEffect.Assault, EffectCompoMode.Possible },
        {CardEffect.Pursuit, EffectCompoMode.Possible },
        {CardEffect.WeaponDmg, EffectCompoMode.Possible },
        {CardEffect.Kiai, EffectCompoMode.Possible },
        {CardEffect.Rush, EffectCompoMode.Possible },
        {CardEffect.HealRush, EffectCompoMode.Possible },
        {CardEffect.SelfPredation, EffectCompoMode.Possible },
        {CardEffect.BloodPact, EffectCompoMode.Possible },
        {CardEffect.Poison, EffectCompoMode.Possible },
        {CardEffect.Detox, EffectCompoMode.Possible },
        {CardEffect.Defense, EffectCompoMode.Possible },
        {CardEffect.Seal, EffectCompoMode.Possible },
        {CardEffect.Burn, EffectCompoMode.Possible },
        {CardEffect.Flame, EffectCompoMode.Possible },
        {CardEffect.CatchingUp, EffectCompoMode.Possible },
        {CardEffect.Electrocution, EffectCompoMode.Possible },
        {CardEffect.Leakage, EffectCompoMode.Possible },
        {CardEffect.Augment, EffectCompoMode.Possible },
        {CardEffect.TripleDamage, EffectCompoMode.Possible },
        {CardEffect.Reverse, EffectCompoMode.Possible },
        {CardEffect.Hypergravity, EffectCompoMode.Possible },
        {CardEffect.ForceHigher, EffectCompoMode.OnlyOwnNew },
        {CardEffect.ForceLess, EffectCompoMode.OnlyOwnNew },
        {CardEffect.BaseOnly, EffectCompoMode.Impossible },
        {CardEffect.PartsOnly, EffectCompoMode.Possible },
        {CardEffect.NoCompo, EffectCompoMode.Impossible },
        {CardEffect.SelfInjury, EffectCompoMode.Possible },
        {CardEffect.DeckRegen, EffectCompoMode.Possible },
        {CardEffect.NoHeal, EffectCompoMode.Possible },
        {CardEffect.Stun, EffectCompoMode.Possible },
        {CardEffect.Reaction, EffectCompoMode.Possible },
        {CardEffect.NoWeapon, EffectCompoMode.Impossible },
        {CardEffect.BonusGold, EffectCompoMode.Impossible },
        {CardEffect.BonusEXP, EffectCompoMode.Impossible },
        {CardEffect.BonusHeal, EffectCompoMode.Impossible },
    };

    #endregion Effect type definition

    #region composite mode definition

    /// <summary>
    /// Effect name (JP)
    /// </summary>
    public static readonly Dictionary<EffectCompoMode, string> DicCompoModeNameJP = new Dictionary<EffectCompoMode, string>()
    {
        {EffectCompoMode.Possible, "合成可能" },
        {EffectCompoMode.Impossible, "合成不可能" },
        {EffectCompoMode.OnlyNew, "追加限定" },
        {EffectCompoMode.OnlyOwn, "自カード限定" },
        {EffectCompoMode.OnlyOwnNew, "追加・自カード限定" },
    };
    /// <summary>
    /// Effect name (EN)
    /// </summary>
    public static readonly Dictionary<EffectCompoMode, string> DicCompoModeNameEN = new Dictionary<EffectCompoMode, string>()
    {
        {EffectCompoMode.Possible, "Composability" },
        {EffectCompoMode.Impossible, "Not Composability" },
        {EffectCompoMode.OnlyNew, "Additional limited" },
        {EffectCompoMode.OnlyOwn, "Own card only" },
        {EffectCompoMode.OnlyOwnNew, "Additional and own card only" },
    };

    #endregion composite mode definition

    #endregion Public Variables

    #region Private Variables

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

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
