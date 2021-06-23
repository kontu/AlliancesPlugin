﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlliancesPlugin.KOTH
{
    public class KothConfig
    {
        public double x = 100000;
        public double y = 100000;
        public double z = 100000;
        public Boolean RequireCaptureBlockForLootGen = true;
        public Boolean HasReward = true;
        public string KothName = "example";
        public string RewardTypeId = "Ingot";
        public string RewardSubTypeId = "Iron";
        public int RewardAmount = 1;
        public long SpaceMoneyReward = 0;
        public Boolean enabled = false;
        public Boolean EditTerritoryFile = false;
        public string TerritoryFilePath = "";
        public string KothBuildingOwner = "BOB";
        public long LootboxGridEntityId = 0;
        public string LootBoxTerminalName = "LOOT BOX";
        public Guid capturingNation = Guid.Empty;
        public int amountCaptured = 0;
        public int MinutesBeforeCaptureStarts = 10;
        public int CaptureRadiusInMetre = 20;
        public Boolean DoCaptureBlockHalfLootTime = false;
        public int SecondsBetweenCoreSpawn = 180;
        public int SecondsBetweenCaptureCheck = 60;
        public int PointsPerCap = 10;
        public int PointsToCap = 100;
        public int MinsPerCaptureBroadcast = 5;
        public int MetaPointsPerCapWithBonus = 1;
        public int MetaPointsPerCap = 1;
        public Guid owner = Guid.Empty;
        public string captureBlockType = "Beacon";
        public string captureBlockSubtype = "LargeBlockBeacon";
        public Boolean captureBlockNeedsToBeTurnedOn = true;
        public Boolean captureBlockNeedsToBroadcast = true;
        public int captureBlockBroadcastDistance = 10000;
        public int hourCooldownAfterFail = 1;
        public int hoursToLockAfterCap = 12;

        public DateTime nextCaptureAvailable = DateTime.Now;
        public Boolean doChatMessages = true;
        public Boolean doDiscordMessages = true;
        public ulong DiscordChannelId = 1;

        public DateTime nextCaptureInterval = DateTime.Now;
        public DateTime nextCoreSpawn = DateTime.Now;
        public DateTime nextBroadcast = DateTime.Now;
        public DateTime unlockTime = DateTime.Now;
        public Boolean CaptureStarted = false;

        public Boolean IsDenialPoint = false;
        public string DeniedKoth = "example";

    }
}
