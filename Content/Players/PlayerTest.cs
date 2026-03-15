using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using BoneTest.Content.Items.Tiles.Perks;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
namespace BoneTest.Content.Players
{
    public class PlayerPerks : ModPlayer
    {
        public int zombieMoney = 0;
        public bool isReloading = false;
        public bool hasJug = false;
        public bool hasSpeed = false;
        public bool hasCherry = false;
        public bool hasMuleKick=false;
        private bool hasMuleUpdated = false;
        public float magSizeMult= 1f;
        public bool hasStaminaUp=false;
        public bool hasDouble = false;
        public bool hasQuickRevive = false;
        
        public override void PostUpdateEquips()
        {
            /* JUg speed cherry mule stamina
            double quick
            */
            if (hasJug)
            {
                Player.endurance+=0.15f;
            }
            if (hasSpeed)
            {
                Player.moveSpeed+=0.5f;
            }
            if (hasDouble)
            {
                Player.GetAttackSpeed(DamageClass.Generic) += 0.3f;
            }
            if (hasMuleKick&&!hasMuleUpdated)
            {
                magSizeMult+=0.3f;
                hasMuleUpdated=true;
            }
            if (hasStaminaUp)
            {
                //3* the amount staminaUp gives you since the other effects is non-applicable
                Player.moveSpeed*=1.21f;
            }
            if(hasQuickRevive){}
            if(hasDouble){}

        }
        public override void PostUpdate()
        {
            if (isReloading&&hasCherry)
            {
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
        public override void SaveData(TagCompound tag) {
            if (hasJug) tag["hasJug"] = hasJug;
            if (hasSpeed) tag["hasSpeed"] = hasSpeed;
            if (hasCherry) tag["hasCherry"] = hasCherry;
            if (hasDouble) tag["hasDouble"] = hasDouble;
            if (hasQuickRevive) tag["hasQuickRevive"] = hasQuickRevive;
        }

        public override void LoadData(TagCompound tag) {
            if (tag.ContainsKey("hasJug"))hasJug = tag.GetBool("hasJug");
            if (tag.ContainsKey("hasSpeed"))hasSpeed = tag.GetBool("hasSpeed");
            if (tag.ContainsKey("hasCherry"))hasCherry = tag.GetBool("hasCherry");
            if (tag.ContainsKey("hasDouble"))hasDouble = tag.GetBool("hasDouble");
            if (tag.ContainsKey("hasQuickRevive"))hasQuickRevive = tag.GetBool("hasQuickRevive");

        }

    }
}