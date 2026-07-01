using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Systems;

namespace BlackOps3.Content.Items.Weapons.BO3.Pistols
{
    public class LCAR9 : ReloadableGun
    {
        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.knockBack = 500f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.None;

            
            magCapacity = 20;
            reloadTime = (int)(60 * 1.5);
            shootSound = new SoundStyle("BlackOps3/Content/Sound/Weapons/MR6shoot")
            {
                Volume = 0.8f,
                Pitch = 0.1f,
                MaxInstances = 3
            };
            reloadSound = new SoundStyle("BlackOps3/Content/Sound/Weapons/MR6reload")
            {
                Volume = 0.8f,
                Pitch = 0.1f,
                MaxInstances = 3
            };
            whenToPlaySound = Item.useAnimation / Item.useTime;
        }
        public override void SetStaticDefaults() {
            Terraria.Localization.Language.GetOrRegister("Mods.BlackOps3.Items.LCAR9.DisplayName", () => "L-CAR 9");
        }
       
        public override void AddRecipes(){
            CreateRecipe()
            .AddIngredient(ItemID.DirtBlock,1)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
        
	}
}