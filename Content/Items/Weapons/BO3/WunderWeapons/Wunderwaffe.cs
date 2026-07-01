using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using BlackOps3.Content.Systems;

namespace BlackOps3.Content.Items.Weapons.BO3.WunderWeapons
{
    public class Wunderwaffe : WunderWeapon
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.ClockworkAssaultRifle;

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Green;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 200000;
            Item.knockBack = 500f;
            Item.noMelee = true;
            Item.shoot = ProjectileID.Bullet;
            Item.shootSpeed = 20f;
            Item.useAmmo = AmmoID.None;

            
            ammo = 150;
            magCapacity = 3;
            ammoReserve = 15;
            reloadTime = (int)(60 * 6);
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
            Terraria.Localization.Language.GetOrRegister("Mods.BlackOps3.Items.WunderWaffe.DisplayName", () => "Wunderwaffe DG-2");
        }

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            if (ammo > 0)
            {
                Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<WunderwaffeProjectile>(), damage, knockback, player.whoAmI);
                playSound();
                removeBullets(1);
            }
            return false;
		}
       
        public override void AddRecipes(){
            CreateRecipe()
            .AddIngredient(ItemID.DirtBlock,1)
            .AddTile(TileID.WorkBenches)
            .Register();
        }
        
	}
}