﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlliancesPlugin.NewCaptureSite
{
    public class CaptureSite
    {
        public class CaptureLog
        {

            public string siteName;
            public int totalCaps;
           
            public List<CaptureLogItem> caps = new List<CaptureLogItem>();

            private Dictionary<string, int> SortedCaps = new Dictionary<string, int>();
            public class CaptureLogItem
            {
                public string Name;
                public int CapAmount;
            }
            public void AddToCap(string name)
            {
                totalCaps += 1;
                if (SortedCaps.TryGetValue(name, out int value))
                {
                    SortedCaps[name] += 1;
                }
                else
                {
                    SortedCaps.Add(name, 1);
                }
            }
            public void LoadSorted()
            {
                foreach (CaptureLogItem item in caps)
                {
                    if (!SortedCaps.ContainsKey(item.Name))
                    {
                        SortedCaps.Add(item.Name, item.CapAmount);
                    }
                }
            }
            public void SaveSorted()
            {
                caps.Clear();
                foreach (KeyValuePair<string, int> pair in SortedCaps)
                {
                    CaptureLogItem item = new CaptureLogItem();
                    item.Name = pair.Key;
                    item.CapAmount = pair.Value;
                    caps.Add(item);
                }
            }
        }
        public Boolean IsChainCap = false;
        public int ChainStartNumber = 30;
        public int ChainEndNumber = 0;
        public Boolean DoSuitCaps = false;
        public CaptureLog caplog = new CaptureLog();
        public Boolean LockOnFail = true;
        public string Name = "Default, change this";
        public byte FDiscordR = 255;
        public byte FDiscordG = 255;
        public byte FDiscordB = 255;
        public ulong FactionDiscordChannelId = 0;
        public Boolean AllianceSite = false;
        public int amountCaptured = 0;

        public int MinutesBeforeCaptureStarts = 10;
        public Boolean ChangeCapSiteOnUnlock = true;
        public Boolean LockAfterSoManyHoursOpen = false;
        public int LockAgainAfterThisManyHours = 2;
        public Boolean CaptureStarted = false;
        public Boolean PickNewSiteOnUnlock = false;
        public Boolean Locked = false;
        public Boolean OnlyDoLootOnceAfterCap = false;
        public int SecondsForOneLootSpawnAfterCap = 600;
        //    public Boolean UnlockAtTheseTimes = false;
        //     public List<int> HoursToUnlockAfter = new List<int>();

        private LootLocation currentLoot = null;
        public LootLocation GetLootSite()
        {
            if (currentLoot != null)
            {
                return currentLoot;
            }
            foreach (LootLocation lot in loot)
            {
                if (lot.Num == GetCurrentLocation().LinkedLootLocation)
                {
                    currentLoot = lot;
                    return lot;
                }
            }
            return null;
        }
        public Location GetNewCapSite(Location ignore)
        {
            Random random = new Random();
            List<Location> temp = new List<Location>();
            foreach (Location loc in locations)
            {
                if (!loc.Name.Equals(ignore))
                {
                    if (random.NextDouble() <= loc.chance)
                    {
                        temp.Add(loc);
                    }
                }
            }
            if (temp.Count == 1)
            {
                return temp[0];
            }
            random = new Random();
            int r = random.Next(temp.Count);
            return temp[r];
        }
  
        public int SecondsBetweenCaptureCheck = 60;
        public int PointsPerCap = 10;
        public int PointsToCap = 100;
        public int CurrentSite = 1;
        public Guid AllianceOwner = Guid.Empty;
        public Guid CapturingAlliance = Guid.Empty;
        public long FactionOwner = 1;
        public long CapturingFaction = 0;
        public int hourCooldownAfterFail = 1;
        public int hoursToLockAfterNormalCap = 12;
        public DateTime nextCaptureAvailable = DateTime.Now;
        public Boolean doChatMessages = true;
        public Boolean doDiscordMessages = true;

        public DateTime nextCaptureInterval = DateTime.Now;

        public DateTime unlockTime = DateTime.Now;
        public void setLootSite(LootLocation loc)
        {

        }
        public Location GetCurrentLocation()
        {
            foreach (Location loc in locations)
            {
                if (loc.Num == this.CurrentSite)
                {
                    return loc;
                }
            }
            return null;
        }
        public List<Location> locations = new List<Location>();
        public List<LootLocation> loot = new List<LootLocation>();
    }
}
