﻿using AlliancesPlugin.Alliances.NewTerritories;
using AlliancesPlugin.KOTH;
using HarmonyLib;
using Sandbox.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Torch.Managers.PatchManager;
using VRage;
using VRage.Game;
using VRage.ObjectBuilders;
using VRageMath;

namespace AlliancesPlugin.Alliances
{
    [PatchShim]
    public static class MyProductionPatch
    {
      
        public static Dictionary<int, RefineryUpgrade> upgrades = new Dictionary<int, RefineryUpgrade>();
        public static Dictionary<int, AssemblerUpgrade> assemblerupgrades = new Dictionary<int, AssemblerUpgrade>();
        public static Dictionary<long, bool> IsInsideTerritory = new Dictionary<long, bool>();
        public static Dictionary<long, DateTime> TimeChecks = new Dictionary<long, DateTime>();
        public static Dictionary<long, Guid> InsideHere = new Dictionary<long, Guid>();

        public static double GetRefineryYieldMultiplier(long PlayerId, MyRefinery refin)
        {
            var faction = MySession.Static.Factions.GetPlayerFaction(PlayerId);

            if (faction == null)
            {
                if (AlliancePlugin.config.EnableOptionalWar)
                {
                    return AlliancePlugin.warcore.config.RefineryYieldMultiplierIfDisabled;
                }
            }
            else
            {
                double buff = 1;
                if (AlliancePlugin.warcore.participants.FactionsAtWar.Contains(faction.FactionId))
                {
                    buff += AlliancePlugin.warcore.config.RefineryYieldMultiplierIfEnabled;
                }
                var alliance = AlliancePlugin.GetAllianceNoLoading(MySession.Static.Factions.TryGetFactionByTag(faction.Tag));
                if (alliance == null || alliance.AssemblerUpgradeLevel <= 0) return buff;
                if (!upgrades.TryGetValue(alliance.RefineryUpgradeLevel, out var upgrade)) return buff;
                if (TimeChecks.TryGetValue(refin.EntityId, out DateTime time))
                {
                    if (DateTime.Now >= time)
                    {
                        TimeChecks[refin.EntityId] = DateTime.Now.AddMinutes(1);
                        if (InsideHere.TryGetValue(refin.EntityId, out Guid terId))
                        {
                            if (AlliancePlugin.Territories.TryGetValue(terId, out Territory ter))
                            {
                                float distance = Vector3.Distance(refin.CubeGrid.PositionComp.GetPosition(),
                                    new Vector3(ter.x, ter.y, ter.z));
                                if (distance <= ter.Radius)
                                {
                                    IsInsideTerritory.Remove(refin.EntityId);
                                    IsInsideTerritory.Add(refin.EntityId, true);
                                }
                                else
                                {
                                    InsideHere.Remove(refin.EntityId);
                                    IsInsideTerritory.Remove(refin.EntityId);
                                }
                            }
                        }
                        else
                        {
                            foreach (var ter in from ter in AlliancePlugin.Territories.Values
                                     where ter.Alliance != Guid.Empty && ter.Alliance == alliance.AllianceId
                                     let distance = Vector3.Distance(refin.CubeGrid.PositionComp.GetPosition(),
                                         new Vector3(ter.x, ter.y, ter.z))
                                     where distance <= ter.Radius
                                     select ter)
                            {
                                IsInsideTerritory.Remove(refin.EntityId);
                                IsInsideTerritory.Add(refin.EntityId, true);
                                InsideHere.Remove(refin.EntityId);

                                InsideHere.Add(refin.EntityId, ter.Id);
                            }
                        }
                    }
                }
                else
                {
                    TimeChecks.Add(refin.EntityId, DateTime.Now.AddMinutes(0.01));
                }

                //      AlliancePlugin.Log.Info(refin.BlockDefinition.Id.SubtypeName);
                if (IsInsideTerritory.TryGetValue(refin.EntityId, out bool isInside))
                {
                    if (isInside)
                    {
                        //   AlliancePlugin.Log.Info("inside territory");
                        buff += upgrade.getRefineryBuffTerritory(refin.BlockDefinition.Id.SubtypeName);
                    }
                    else
                    {
                        //   AlliancePlugin.Log.Info("not inside territory");
                        buff += upgrade.getRefineryBuff(refin.BlockDefinition.Id.SubtypeName);
                    }
                }
                else
                {
                    //   AlliancePlugin.Log.Info("not inside territory");
                    buff += upgrade.getRefineryBuff(refin.BlockDefinition.Id.SubtypeName);
                }

                return buff;
            }

            return 0;
        }
        public static float GetAssemblerSpeedMultiplier(long PlayerId, MyAssembler assembler)
        {

            return 0;
        }

        [HarmonyPatch(typeof(MyAssembler))]
        [HarmonyPatch("CalculateBlueprintProductionTime")]
        public class AssemblerPatch
        {
            static void Postfix(MyBlueprintDefinitionBase currentBlueprint, ref float __result, MyAssembler __instance)
            {

                if (__instance.GetOwnerFactionTag().Length > 0)
                {
                    Alliance alliance = AlliancePlugin.GetAllianceNoLoading(MySession.Static.Factions.TryGetFactionByTag(__instance.GetOwnerFactionTag()));
                    if (alliance == null)
                    {
                        return;
                    }
                    if (alliance.AssemblerUpgradeLevel == 0)
                    {
                        //     AlliancePlugin.Log.Info("no refinery upgrade");
                        return;
                    }

                    // MyAPIGateway.Multiplayer.RegisterMessageHandler(NET_ID, MessageHandler);

                    float buff = 1f;
                    //    AlliancePlugin.Log.Info("Buffed by " + buff.ToString());
                    if (assemblerupgrades.TryGetValue(alliance.AssemblerUpgradeLevel, out AssemblerUpgrade upgrade))
                    {
                        if (TimeChecks.TryGetValue(__instance.EntityId, out DateTime time))
                        {
                            if (DateTime.Now >= time)
                            {
                                TimeChecks[__instance.EntityId] = DateTime.Now.AddMinutes(1);
                                if (InsideHere.TryGetValue(__instance.EntityId, out Guid terId))
                                {
                                    if (AlliancePlugin.Territories.TryGetValue(terId, out Territory ter))
                                    {
                                        float distance = Vector3.Distance(__instance.CubeGrid.PositionComp.GetPosition(), new Vector3(ter.x, ter.y, ter.z));
                                        if (distance <= ter.Radius)
                                        {
                                            IsInsideTerritory.Remove(__instance.EntityId);
                                            IsInsideTerritory.Add(__instance.EntityId, true);
                                        }
                                        else
                                        {
                                            InsideHere.Remove(__instance.EntityId);
                                            IsInsideTerritory.Remove(__instance.EntityId);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (Territory ter in AlliancePlugin.Territories.Values)
                                    {
                                        if (ter.Alliance != Guid.Empty && ter.Alliance == alliance.AllianceId)
                                        {
                                            float distance = Vector3.Distance(__instance.CubeGrid.PositionComp.GetPosition(), new Vector3(ter.x, ter.y, ter.z));
                                            if (distance <= ter.Radius)
                                            {
                                                IsInsideTerritory.Remove(__instance.EntityId);
                                                IsInsideTerritory.Add(__instance.EntityId, true);
                                                InsideHere.Remove(__instance.EntityId);

                                                InsideHere.Add(__instance.EntityId, ter.Id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            TimeChecks.Add(__instance.EntityId, DateTime.Now.AddMinutes(0.01));
                        }
                        //      AlliancePlugin.Log.Info(refin.BlockDefinition.Id.SubtypeName);
                        if (IsInsideTerritory.TryGetValue(__instance.EntityId, out bool isInside))
                        {
                            if (isInside)
                            {

                                buff -= (float)upgrade.getAssemblerBuffTerritory(__instance.BlockDefinition.Id.SubtypeName);
                            }
                            else
                            {
                                buff -= (float)upgrade.getAssemblerBuff(__instance.BlockDefinition.Id.SubtypeName);
                            }
                        }
                        else
                        {
                            buff -= (float)upgrade.getAssemblerBuff(__instance.BlockDefinition.Id.SubtypeName);
                        }
                        //      AlliancePlugin.Log.Info(refin.BlockDefinition.Id.SubtypeName);

                        __result *= buff;
                    }
                }
            }
        }

    }
}
