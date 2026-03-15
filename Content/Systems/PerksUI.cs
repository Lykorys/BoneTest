using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using BoneTest.Content.Players;

namespace BoneTest.Content.Systems
{
    public class PerksUI : ModSystem
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            PlayerPerks perks = Main.LocalPlayer.GetModPlayer<PlayerPerks>();

            ReLogic.Graphics.DynamicSpriteFont font = FontAssets.MouseText.Value;
            if(perks.hasJug)Terraria.Utils.DrawBorderStringFourWay(spriteBatch,font,"jug",Main.screenWidth/2,Main.screenHeight * 0.95f,Color.Yellow,Color.Black,Vector2.Zero);
        }
    }
}