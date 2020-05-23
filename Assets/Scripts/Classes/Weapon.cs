//================================================================================
//
//  Weapon
//
//  武器のクラス
//
//================================================================================

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
    /// 武器の射撃方式
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
    /// 武器のランク
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
    /// ユニークな識別子
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
    /// バースト射撃の発射間隔
    /// </summary>
    [field: SerializeField, RenameField("Burst Shot Delay")]
    public float burstShotDelay;

    /// <summary>
    /// 射出物の発射数
    /// </summary>
    [field: SerializeField, RenameField("Projectile Count")]
    public int projectileCount;

    /// <summary>
    /// 射出物の最大拡散角度
    /// </summary>
    [field: SerializeField, RenameField("Spread Range")]
    public float spreadRange;

    /// <summary>
    /// 爆発範囲の半径
    /// </summary>
    [field: SerializeField, RenameField("Explosion Range")]
    public float explosionRange;

    /// <summary>
    /// 射出物のダメージ
    /// </summary>
    [field: SerializeField, RenameField("Projectile Damage")]
    public int projectileDamage;

    /// <summary>
    /// 射出物の飛行速度
    /// </summary>
    [field: SerializeField, RenameField("Projectile Speed")]
    public float projectileSpeed;

    /// <summary>
    /// 射撃のディレイの長さ
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
    /// コンストラクタ
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
