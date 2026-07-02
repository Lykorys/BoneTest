using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Systems;

namespace BlackOps3.Content.Items.Weapons.BO3.Pistols
{
    public class Bloundhound : ReloadableGun
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.Revolver;

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 20;
            Item.knockBack = 500f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.None;
            
            magCapacity = 8;
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
            LoadBullets();
        }
       
        public override void AddRecipes(){
            CreateRecipe()
            .AddIngredient(ItemID.DirtBlock,1)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
        
	}
}