using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SkillDefine Class
/// </summary>
public class SkillDefine
{
    public enum Skill
    {
        // スキル無し
        _None,
        // 会心の一撃
        Critical,
        // シールド破壊
        DefBreak,
        // 回復
        Heal,
        // ファイアボール
        FieBall,
    }
    /// <summary>
    /// Skill name
    /// </summary>
    public static Dictionary<Skill, string> dicSkillName = new Dictionary<Skill, string>()
    {
        {Skill._None, "スキル無し" },
        {Skill.Critical, "会心の一撃" },
        {Skill.DefBreak, "シールド破壊" },
        {Skill.Heal, "回復" },
        {Skill.FieBall, "ファイアボール" }
    };
    /// <summary>
    /// Skill info
    /// </summary>
    public static Dictionary<Skill, string> dicSkillInfo = new Dictionary<Skill, string>()
    {
        {Skill._None, "----" },
        {Skill.Critical, "ダメージ2倍の攻撃\n(1回限り)" },
        {Skill.DefBreak, "敵の防御力を０にします\n(ダメージは０)" },
        {Skill.Heal, "味方のHPを回復します" },
        {Skill.FieBall, "どの位置に居る敵も攻撃できます\n(ダメージは半分)" }
    };
}
