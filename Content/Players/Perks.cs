using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
namespace BoneTest.Content.Players
{
    public abstract class Perk
    {
        
        public string perkName => GetType().Name;
        public int tier = 1;
        public abstract Texture2D perkLogo {get;}
        public abstract void ApplyEffect(PlayerPerks playerPerks);
    }
    public class DoubleTap : Perk
    {
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BoneTest/Content/Players/PerksLogo/DoubleTapLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            perkPlayer.Player.GetAttackSpeed(DamageClass.Generic) += 0.3f;
        }
    }

    public class ElectricCherry : Perk
    {
        //speed boost after reload
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BoneTest/Content/Players/PerksLogo/ElectricCherryLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {   
            
        }
    }

    public class Juggernog : Perk
    {
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BoneTest/Content/Players/PerksLogo/JuggernogLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            if(tier>=1)perkPlayer.Player.endurance+=0.15f;
            if (tier >= 2)
            {
                perkPlayer.Player.moveSpeed+=5f;
            }
            
        }
    }

    public class MuleKick : Perk
    {
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BoneTest/Content/Players/PerksLogo/MuleKickLogo").Value;
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            perkPlayer.magSizeMult += 0.3f;
        }
    }

    public class QuickRevive : Perk
    {  
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BoneTest/Content/Players/PerksLogo/QuickReviveLogo").Value;
        


        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
        }
    }

    public class SpeedCola : Perk
    {
        public override Texture2D perkLogo =>ModContent.Request<Texture2D>("BoneTest/Content/Players/PerksLogo/SpeedColaLogo").Value;

        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            perkPlayer.Player.moveSpeed += 0.5f;
        }
    }

    public class StaminUp : Perk
    {
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BoneTest/Content/Players/PerksLogo/StaminUpLogo").Value;

        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
            perkPlayer.Player.moveSpeed *= 1.21f;
        }
    }

    public class WidowsWine : Perk
    {
 
        public override Texture2D perkLogo => ModContent.Request<Texture2D>("BoneTest/Content/Players/PerksLogo/ElectricCherryLogo").Value;
        
        public override void ApplyEffect(PlayerPerks perkPlayer)
        {
        }
    }
}