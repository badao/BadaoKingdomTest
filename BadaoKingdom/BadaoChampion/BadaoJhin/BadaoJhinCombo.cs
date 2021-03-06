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
    public static class BadaoJhinCombo
    {
        public static void BadaoActivate()
        {
            Game.OnUpdate += Game_OnUpdate;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
        }

        private static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            if (BadaoMainVariables.R.Instance.SData.Name == "JhinRShot")
                return;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (BadaoMainVariables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                return;
            if (BadaoMainVariables.R.Instance.SData.Name == "JhinRShot")
                return;
            if (BadaoJhinHelper.UseQCombo())
            {
                var info = BadaoJhinHelper.GetQInfo();
                var target = info.Where(x => x.BounceTargets.LastOrDefault(y => y.Target is Obj_AI_Hero) != null)
                    .OrderBy(x => x.BounceTargets.LastOrDefault(y => y.Target is Obj_AI_Hero).DeathCount)
                    .ThenByDescending(x => x.BounceTargets.IndexOf(x.BounceTargets.LastOrDefault(y => y.Target is Obj_AI_Hero)))
                    .LastOrDefault();
                if (target != null)
                {
                    BadaoMainVariables.Q.Cast(target.QTarget);
                }
            }
            if (BadaoJhinHelper.UseWCombo())
            {
                var target = TargetSelector.GetTarget(BadaoMainVariables.W.Range, TargetSelector.DamageType.Physical);
                if (target.BadaoIsValidTarget())
                {
                    var x = BadaoMainVariables.W.GetPrediction(target).CastPosition;
                    var y = BadaoMainVariables.W.GetPrediction(target).CollisionObjects;
                    if (!y.Any(z => z.IsChampion()) && ObjectManager.Player.Distance(x) <= BadaoMainVariables.W.Range)
                    {
                        BadaoMainVariables.W.Cast(x);
                    }
                    else
                    {
                        foreach (var hero in HeroManager.Enemies.Where(a => a.BadaoIsValidTarget() && BadaoJhinHelper.HasJhinPassive(a)))
                        {
                            var b = BadaoMainVariables.W.GetPrediction(hero).CastPosition;
                            var c = BadaoMainVariables.W.GetPrediction(hero).CollisionObjects;
                            if (!c.Any(d => d.IsChampion()) && ObjectManager.Player.Distance(b) <= BadaoMainVariables.W.Range)
                            {
                                if (BadaoMainVariables.W.Cast(x))
                                    break;
                            }
                        }
                    }
                }
            }
            if (BadaoJhinHelper.UseECombo())
            {
                var target = TargetSelector.GetTarget(BadaoMainVariables.E.Range, TargetSelector.DamageType.Physical);
                if (target.BadaoIsValidTarget())
                {
                    BadaoMainVariables.E.Cast(target.Position);
                }
            }
        }
    }
}
