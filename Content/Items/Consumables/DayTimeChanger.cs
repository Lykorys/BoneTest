using BlackOps3.Content.Systems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BlackOps3.Content.Items.Consumables
{
	public class DayTimeChanger : ModItem
	{
		public int midDayMode = 0;

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = false;
		}

		public override bool AltFunctionUse(Player player) => true;
        
		public override bool? UseItem(Player player) {
			if(player.altFunctionUse == 2)
			{
				if(midDayMode==2) midDayMode= 0;
				else if(midDayMode==0) midDayMode= 1;
				else if(midDayMode==1) midDayMode= 2;
				SoundEngine.PlaySound(SoundID.Roar,player.position);
			}
			else
			{
				if (player.whoAmI == Main.myPlayer) {
					Main.NewText("feur");
					Main.NewText(midDayMode);
					switch (midDayMode)
					{
						
						case 0:
							if (Main.dayTime)
							{
								Main.dayTime= false;
								Main.time=Main.nightLength/2;//on passe a la nuit
							}
							else
							{
								Main.dayTime= true;
								Main.time=Main.dayLength/2;
							}
						break;
						case 1:
							if (Main.dayTime)
							{
								Main.dayTime= false;
								Main.time=0;//on passe a la nuit
							}
							else
							{
								Main.dayTime= true;
								Main.time=0;
							}
						break;
						case 2:
							TimeLockSystem.timeLock= !TimeLockSystem.timeLock;
						break;
					}
					SoundEngine.PlaySound(SoundID.CoinPickup, player.position);
					// Sync of world data on the server in MP
					if (Main.netMode == NetmodeID.Server) {
						NetMessage.SendData(MessageID.WorldData);
					}
				}
			}
			return true;
		}
	}
}
