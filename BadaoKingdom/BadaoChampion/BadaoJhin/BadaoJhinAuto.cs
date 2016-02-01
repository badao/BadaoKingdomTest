﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace BadaoKingdom.BadaoChampion.BadaoJhin
{
    public static class BadaoJhinAuto
    {
        public static void BadaoActiavate()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (BadaoJhinHelper.UseRAuto()/* && ObjectManager.Player.IsCastingInterruptableSpell()*/)
            {
                Game.PrintChat("1");
                var target = TargetSelector.GetTarget(BadaoMainVariables.R.Range, TargetSelector.DamageType.Physical,
                    true, HeroManager.Enemies.Where(x => x.IsValid && !BadaoChecker.BadaoInTheCone(
                         x.Position.To2D(), ObjectManager.Player.Position.To2D(),
                         ObjectManager.Player.Position.To2D() 
                         + ObjectManager.Player.Direction.To2D().Normalized().Perpendicular() * BadaoMainVariables.R.Range, 60)));
                if (target.BadaoIsValidTarget())
                {
                    Game.PrintChat("2");
                    BadaoMainVariables.R.Cast(target);
                }
            }
            if (ObjectManager.Player.IsCastingInterruptableSpell())
                return;
            if (BadaoJhinHelper.UseAutoKS())
            {
                foreach (var hero in HeroManager.Enemies.Where(x => x.BadaoIsValidTarget(BadaoMainVariables.W.Range)))
                {
                    if (BadaoMainVariables.W.IsReady() && BadaoMainVariables.W.GetDamage(hero) >= hero.Health)
                    {
                        var x = BadaoMainVariables.W.GetPrediction(hero).CastPosition;
                        var y = BadaoMainVariables.W.GetPrediction(hero).CollisionObjects;
                        if (!y.Any(z => z.IsChampion()) && ObjectManager.Player.Distance(x) <= BadaoMainVariables.W.Range)
                        {
                            if (BadaoMainVariables.W.Cast(x))
                                break;
                        }
                    }
                    if (BadaoMainVariables.Q.IsReady() && BadaoMainVariables.Q.GetDamage(hero) >= hero.Health
                        && hero.BadaoIsValidTarget(BadaoMainVariables.Q.Range))
                    {
                        BadaoMainVariables.Q.Cast(hero);
                    }
                }
            }
            if (!BadaoJhinHelper.CanAutoMana())
                return;
            if (BadaoJhinHelper.UseWAuto())
            {
                foreach (var hero in HeroManager.Enemies.Where(x => x.BadaoIsValidTarget(BadaoMainVariables.W.Range)))
                {
                    if (hero.HasBuffOfType(BuffType.Slow) && BadaoJhinHelper.HasJhinPassive(hero))
                    {
                        var x = BadaoMainVariables.W.GetPrediction(hero).CastPosition;
                        var y = BadaoMainVariables.W.GetPrediction(hero).CollisionObjects;
                        if (!y.Any(z => z.IsChampion()) && ObjectManager.Player.Distance(x) <= BadaoMainVariables.W.Range)
                        {
                            if (BadaoMainVariables.W.Cast(x))
                                break;
                        }
                    }
                }
            }
        }
    }
}
