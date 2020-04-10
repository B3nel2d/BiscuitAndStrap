//================================================================================
//
//  Weapon
//
//  武器のクラス
//
//================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : Item{

    /**************************************************
        Enumerations
    **************************************************/

    /// <summary>
    /// 武器のグレード
    /// </summary>
    public enum Grade{
        I,
        II,
        III
    }

    /// <summary>
    /// 武器の種類
    /// </summary>
    public enum Type{
        Pistol,
        Rifle,
        Shotgun,
        Launcher
    }

    /// <summary>
    /// 射撃方式
    /// </summary>
    public enum FireMode{
        SemiAutomatic,
        Burst,
        FullAutomatic
    }

    /// <summary>
    /// 射出物の種類
    /// </summary>
    public enum Projectile{
        Bullet,
        Granade,
        Rocket,
        Flame,
        Laser
    }

    /// <summary>
    /// 武器のランク(レア度)
    /// </summary>
    public enum Rank{
        Bronze,
        Silver,
        Gold,
        Platinum,
        Diamond
    }

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// 武器のユニークなID
    /// </summary>
    [field: SerializeField, RenameField("ID")]
    public int id;

    /// <summary>
    /// グレード
    /// </summary>
    [field: SerializeField, RenameField("Grade")]
    public Grade grade;

    /// <summary>
    /// 種類
    /// </summary>
    [field: SerializeField, RenameField("Type")]
    public Type type;

    /// <summary>
    /// 射撃方式
    /// </summary>
    [field: SerializeField, RenameField("Fire Mode")]
    public FireMode fireMode;

    /// <summary>
    /// 射出物
    /// </summary>
    [field: SerializeField, RenameField("Projectile")]
    public Projectile projectile;

    /// <summary>
    /// ランク
    /// </summary>
    [field: SerializeField, RenameField("Rank")]
    public Rank rank;

    /// <summary>
    /// スコア
    /// </summary>
    [field: SerializeField, RenameField("Score")]
    public int score;

    /// <summary>
    /// バースト射撃の発射数
    /// </summary>
    [field: SerializeField, RenameField("Burst Round Count")]
    public int burstRoundCount;

    /// <summary>
    /// バースト射撃の間隔
    /// </summary>
    [field: SerializeField, RenameField("Burst Shot Delay")]
    public float burstShotDelay;

    /// <summary>
    /// 散弾の射出物の数
    /// </summary>
    [field: SerializeField, RenameField("Projectile Count")]
    public int projectileCount;

    /// <summary>
    /// 散弾の拡散範囲
    /// </summary>
    [field: SerializeField, RenameField("Spread Range")]
    public float spreadRange;

    /// <summary>
    /// 爆発の範囲
    /// </summary>
    [field: SerializeField, RenameField("Explosion Range")]
    public float explosionRange;

    /// <summary>
    /// 射出物のダメージ
    /// </summary>
    [field: SerializeField, RenameField("Projectile Damage")]
    public int projectileDamage;

    /// <summary>
    /// 射出物の飛翔速度
    /// </summary>
    [field: SerializeField, RenameField("Projectile Speed")]
    public float projectileSpeed;

    /// <summary>
    /// 射撃の間隔
    /// </summary>
    [field: SerializeField, RenameField("Shot Delay")]
    public float shotDelay;

    /// <summary>
    /// クリティカルの発生確率
    /// </summary>
    [field: SerializeField, RenameField("Critical Chance")]
    public float criticalChance;

    /**************************************************
        Functions
    **************************************************/

    /// <summary>
    /// コンストラクター
    /// </summary>
    public Weapon(){
        id = -1;
        name = "Favorite Buddy";
        grade = Grade.I;
        type = Type.Pistol;
        fireMode = FireMode.SemiAutomatic;
        projectile = Projectile.Bullet;
        rank = Rank.Bronze;
        score = 0;
        burstRoundCount = 0;
        burstShotDelay = 0.0f;
        projectileCount = 1;
        spreadRange = 0.0f;
        explosionRange = 0.0f;
        projectileDamage = 35;
        projectileSpeed = 25.0f;
        shotDelay = 1.0f;
        criticalChance = 0.0f;
    }

}
