//================================================================================
//
//  Crate
//
//  武器箱のクラス
//
//================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate{

    /**************************************************
        Enumerations
    **************************************************/

    /// <summary>
    /// 箱のランク
    /// </summary>
    public enum Rank{
        Normal,
        Rare
    }

    /**************************************************
        Fields / Properties
    **************************************************/

    /// <summary>
    /// ランク
    /// </summary>
    public Rank rank{
        get;
        private set;
    }

    /// <summary>
    /// グレード
    /// </summary>
    public Weapon.Grade grade{
        get;
        private set;
    }

    /// <summary>
    /// 中の武器
    /// </summary>
    private Weapon weapon{
        get;
        set;
    }

    /// <summary>
    /// 散弾武器になる確率
    /// </summary>
    private float multipleProjectileChance{
        get{
            return 0.3f;
        }
    }

    /**************************************************
        Functions
    **************************************************/

    /// <summary>
    /// コンストラクター
    /// </summary>
    public Crate(Rank rank, Weapon.Grade grade){
        this.rank = rank;
        this.grade = grade;
    }

    /// <summary>
    /// ランダムな武器スタッツの設定
    /// </summary>
    private void SetWeaponStats(){
        weapon = new Weapon();

        int id = -1;
        do{
            id = UnityEngine.Random.Range(0, 100);
        }
        while(InventoryManager.weapons.Exists(target => target.id == id));

        weapon.id = id;
        weapon.grade = grade;

        weapon.fireMode = Weapon.FireMode.SemiAutomatic; /*(Weapon.FireMode)UnityEngine.Random.Range(0, Enum.GetValues(typeof(Weapon.FireMode)).Length);*/
        weapon.projectile = Weapon.Projectile.Bullet; /*(WeaponStats.Projectile)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeaponStats.Projectile)).Length);*/
        bool multipleProjectile = false; /*UnityEngine.Random.value <= multipleProjectileChance;*/

        float score = 0.0f;
        int statCount = 4;

        switch(weapon.fireMode){
            case Weapon.FireMode.SemiAutomatic:
                switch(weapon.projectile){
                    case Weapon.Projectile.Bullet:
                        if(multipleProjectile){
                            SetStat(ref weapon.projectileDamage, 3, 10, ref score);
                            SetStat(ref weapon.projectileSpeed, 20, 50, ref score);
                            SetStat(ref weapon.shotDelay, 0.5f, 1.0f, ref score, true);
                            SetStat(ref weapon.criticalChance, 0.0f, 0.3f, ref score);

                            SetStat(ref weapon.projectileCount, 3, 10, ref score);
                            SetStat(ref weapon.spreadRange, 25.0f, 55.0f, ref score, true);

                            statCount += 2;
                        }
                        else{
                            SetStat(ref weapon.projectileDamage, 35, 100, ref score);
                            SetStat(ref weapon.projectileSpeed, 25, 50, ref score);
                            SetStat(ref weapon.shotDelay, 0.25f, 1.0f, ref score, true);
                            SetStat(ref weapon.criticalChance, 0.0f, 0.3f, ref score);
                        }

                        break;
                    case Weapon.Projectile.Granade:
                    case Weapon.Projectile.Rocket:
                    case Weapon.Projectile.Flame:
                    case Weapon.Projectile.Laser:

                        break;
                }

                break;
            case Weapon.FireMode.Burst:
                switch(weapon.projectile){
                    case Weapon.Projectile.Bullet:
                        if(multipleProjectile){
                            SetStat(ref weapon.projectileDamage, 2, 8, ref score);
                            SetStat(ref weapon.projectileSpeed, 20, 50, ref score);
                            SetStat(ref weapon.shotDelay, 0.35f, 0.65f, ref score, true);
                            SetStat(ref weapon.criticalChance, 0.0f, 0.3f, ref score);

                            SetStat(ref weapon.burstRoundCount, 2, 5, ref score);
                            SetStat(ref weapon.burstShotDelay, 0.05f, 0.20f, ref score);

                            SetStat(ref weapon.projectileCount, 3, 8, ref score);
                            SetStat(ref weapon.spreadRange, 30.0f, 60.0f, ref score, true);

                            statCount += 4;
                        }
                        else{
                            SetStat(ref weapon.projectileDamage, 15, 30, ref score);
                            SetStat(ref weapon.projectileSpeed, 20, 50, ref score);
                            SetStat(ref weapon.shotDelay, 0.30f, 0.50f, ref score, true);
                            SetStat(ref weapon.criticalChance, 0.0f, 0.3f, ref score);

                            SetStat(ref weapon.burstRoundCount, 2, 5, ref score);
                            SetStat(ref weapon.burstShotDelay, 0.05f, 0.20f, ref score);

                            statCount += 2;
                        }

                        break;
                    case Weapon.Projectile.Granade:
                    case Weapon.Projectile.Rocket:
                    case Weapon.Projectile.Flame:
                    case Weapon.Projectile.Laser:

                        break;
                }

                break;
            case Weapon.FireMode.FullAutomatic:
                switch(weapon.projectile){
                    case Weapon.Projectile.Bullet:
                        if(multipleProjectile){
                            SetStat(ref weapon.projectileDamage, 1, 5, ref score);
                            SetStat(ref weapon.projectileSpeed, 20, 50, ref score);
                            SetStat(ref weapon.shotDelay, 0.15f, 0.45f, ref score, true);
                            SetStat(ref weapon.criticalChance, 0.0f, 0.3f, ref score);

                            SetStat(ref weapon.projectileCount, 3, 7, ref score);
                            SetStat(ref weapon.spreadRange, 30.0f, 6.0f, ref score, true);

                            statCount += 2;
                        }
                        else{
                            SetStat(ref weapon.projectileDamage, 10, 20, ref score);
                            SetStat(ref weapon.projectileSpeed, 20, 50, ref score);
                            SetStat(ref weapon.shotDelay, 0.05f, 0.20f, ref score, true);
                            SetStat(ref weapon.criticalChance, 0.0f, 0.3f, ref score);
                        }

                        break;
                    case Weapon.Projectile.Granade:
                    case Weapon.Projectile.Rocket:
                    case Weapon.Projectile.Flame:
                    case Weapon.Projectile.Laser:

                        break;
                }

                break;
        }

        score /= (float)statCount;

        weapon.score = (int)Math.Floor(score * 100);
        weapon.rank = (Weapon.Rank)((int)(score * 5));
    }

    /// <summary>
    /// スタッツの設定
    /// </summary>
    private void SetStat(ref int stat, int minimumValue, int maximumValue, ref float score, bool invertScore = false){
        float randomNumber = UnityEngine.Random.value;
        stat = (int)Math.Round(minimumValue + (maximumValue - minimumValue) * Mathf.Pow(randomNumber, 3.0f));

        if(invertScore){
            score += 1.0f - ((float)(stat - minimumValue) / (float)(maximumValue - minimumValue));
        }
        else{
            score += (float)(stat - minimumValue) / (float)(maximumValue - minimumValue);
        }
    }
    /// <summary>
    /// スタッツの設定
    /// </summary>
    private void SetStat(ref float stat, float minimumValue, float maximumValue, ref float score, bool invertScore = false){
        float randomNumber = UnityEngine.Random.value;
        stat = minimumValue + (maximumValue - minimumValue) * Mathf.Pow(randomNumber, 3.0f);

        if(invertScore){
            score += 1.0f - ((stat - minimumValue) / (maximumValue - minimumValue));
        }
        else{
            score += (stat - minimumValue) / (maximumValue - minimumValue);
        }
    }

    /// <summary>
    /// 武器の取得
    /// </summary>
    public Weapon Open() {
        SetWeaponStats();

        return weapon;
    }

}
