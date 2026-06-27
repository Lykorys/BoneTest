using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BlackOps3.Content.Items.Tiles.Perks;
using System.Linq;
using System.Reflection;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
namespace BlackOps3.Content.Players
{
    
    //TODO save perk limit after dc
    public class PlayerPerks : ModPlayer
    {
        public int zombieMoney = 10000;
        public bool isReloading = false;
        public float reloadSpeed = 1f;
        public float magSizeMult= 1f;
        public int maxPerks= 4;
        public Dictionary<string, Perk> ActivePerks { get; private set; } = new();
        public bool HasPerk(string perk) => ActivePerks.ContainsKey(perk);
        public void AddPerk(Perk perk) 
        {
            if(ActivePerks.Count<maxPerks)
                if (!ActivePerks.TryGetValue(perk.perkName, out Perk value))
                    ActivePerks.Add(perk.perkName, perk);
                else 
                    if(value.tier < value.maxTier) value.tier++;
        }

        public void ClearPerks() => ActivePerks = new();
        public void PerkAHolic()
        {
            var perkTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Perk)) && !type.IsAbstract);
            int oldMax = maxPerks;
            maxPerks= perkTypes.Count();
            foreach (Type type in perkTypes)
            {
                if (Activator.CreateInstance(type) is Perk perkInstance)
                {
                    if (!HasPerk(perkInstance.perkName)) 
                    {
                        AddPerk(perkInstance);
                    }
                    ActivePerks[perkInstance.perkName].tier=perkInstance.maxTier;
                }
            }
            maxPerks=oldMax;
            Main.NewText("Perkaholic Activated! All perks granted.",Color.Cyan);
        }
        
        public void RemovePerk(string perk) => ActivePerks.Remove(perk);
        public override void ResetEffects()
        {
            magSizeMult = 1f;
        }

        public override void PostUpdateEquips()
        {
            foreach (var perk in ActivePerks.Values)
            {
                perk.ApplyEffect(this);
                
            }
        }
        public override void PostUpdate()
        {
            if (isReloading&&HasPerk("ElectricCherry"))//TODO generalize this system to a perk.Condition
            {
                
                if(ActivePerks["ElectricCherry"].tier>=2){
                    Player.AddBuff(ModContent.BuffType<ElectricCherryBuff>(),5);
                    Main.NewText(ActivePerks["ElectricCherry"].tier);
                }
                Vector2 spawnPos = Player.Center;
                int projWidth = 30;
                int projHeight = 30;
                Main.NewText(Player.Center);
                
                spawnPos.X -= projWidth / 2f;
                spawnPos.Y -= projHeight / 2f;
                Main.NewText(spawnPos);
                Main.NewText("RELOADING");
                int proj = Projectile.NewProjectile(Player.GetSource_FromThis(),spawnPos,Vector2.Zero,ModContent.ProjectileType<CherryLightning>(),150,0f,Player.whoAmI);
                Main.projectile[proj].hostile = false;
                Main.projectile[proj].friendly = true;
                isReloading=false;
            }
        }
        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            ClearPerks();
            base.Kill(damage, hitDirection, pvp, damageSource);
        }
        /*
        public override void SaveData(TagCompound tag)
        {
            List<TagCompound> savedPerks = new List<TagCompound>();
            foreach (var val in ActivePerks)
            {
                TagCompound perkTag = new TagCompound();
                perkTag["name"] = val.Key;
                perkTag["tier"] = val.Value.tier;
                savedPerks.Add(perkTag);
            }
            tag["ownedPerksList"] = savedPerks;
        }

        public override void LoadData(TagCompound tag)
        {
            ActivePerks.Clear();
            if (tag.ContainsKey("ownedPerksList"))
            {
                var savedPerks = tag.GetList<TagCompound>("ownedPerksList");
                foreach (TagCompound perkTag in savedPerks)
                {
                    string perkName = perkTag.GetString("name");
                    int perkTier = perkTag.GetInt("tier");
                    Perk perkInstance = CreatePerkFromName(perkName);
                    if (perkInstance != null)
                    {
                        perkInstance.tier = perkTier; 
                        AddPerk(perkInstance);
                    }
                }
            }
        }*/
        

        public void applyPerkBuff(int type, int timeToAdd,float modifier)//todo try 
        {
        }
        private Perk CreatePerkFromName(string name) => name switch
        {
            "DoubleTap" => new DoubleTap(),
            "ElectricCherry" => new ElectricCherry(),
            "Juggernog" => new Juggernog(),
            "MuleKick" => new MuleKick(),
            "QuickRevive" => new QuickRevive(),
            "SpeedCola" => new SpeedCola(),
            "StaminUp" => new StaminUp(),
            "WidowsWine" => new WidowsWine(),
            _ => null
        };

    }
}