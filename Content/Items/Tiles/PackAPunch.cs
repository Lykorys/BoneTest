using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using BlackOps3.Content.Systems;
using static BlackOps3.Content.Systems.PackAPunchProcesses;

namespace BlackOps3.Content.Items.Tiles
{
    public class PackAPunchTile : ModTile
    {
        public override string Texture => "Terraria/Images/Tiles_77";

        public override void SetStaticDefaults() {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<PackAPunchEntity>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);
            AddMapEntry(new Color(100, 100, 100));
        }

        public override bool RightClick(int i, int j) {
            int frameX = Main.tile[i, j].TileFrameX / 18;
            int frameY = Main.tile[i, j].TileFrameY / 18;
            int x = i - (frameX % 3); 
            int y = j - (frameY % 2);

            if (!TileEntity.ByPosition.TryGetValue(new Point16(x, y), out TileEntity te) || !(te is PackAPunchEntity entity))
                return false;

            Player player = Main.LocalPlayer;
            
            bool isPackable = PunchUpgrades.Any(recipe => recipe.Input == player.HeldItem.type);
            bool isReloadable = !player.HeldItem.IsAir && player.HeldItem.ModItem is Reloadable;

            if (entity.itemSlot != null && !entity.itemSlot.IsAir) {
                player.QuickSpawnItem(player.GetSource_TileInteraction(x, y), entity.itemSlot);
                entity.itemSlot.TurnToAir(); 
                entity.timer = 0;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab, new Vector2(x * 16, y * 16));
            } 
            else if (isReloadable && isPackable) {
                for (int k = 0; k < 10; k++) {
                    Dust.NewDust(player.Center, 10, 10, DustID.MagicMirror, 
                        (x * 16 - player.Center.X) / 10, (y * 16 - player.Center.Y) / 10);
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, new Vector2(x * 16, y * 16));
                
                entity.itemSlot = player.HeldItem.Clone();
                entity.itemSlot.stack = 1;
                player.HeldItem.stack--;
                if (player.HeldItem.stack <= 0) player.HeldItem.TurnToAir();
                entity.timer = 0; 
            }

            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, entity.ID, x, y);
            }
            
            return true;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch) {
            if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0) {
                if (TileEntity.ByPosition.TryGetValue(new Point16(i, j), out TileEntity te) && te is PackAPunchEntity pEntity) {
                    if (pEntity.itemSlot == null || pEntity.itemSlot.IsAir) return;
                    Texture2D texture = Terraria.GameContent.TextureAssets.Item[pEntity.itemSlot.type].Value;
                    Vector2 tileCenter = new(i * 16 + 24, j * 16 + 16);
                    Vector2 drawPos = tileCenter - Main.screenPosition;
                    float scale = 1f;
                    if (pEntity.timer < 120) {
                        float progress = (float)pEntity.timer / 120f;
                        scale = 1f - progress; 
                    }
                    else if (pEntity.timer >= 120 && pEntity.timer < 780) {
                        scale = 1f;
                    }
                    else if (pEntity.timer >= 780 && pEntity.timer < 900) {
                        float progress = (float)(pEntity.timer - 780) / 120f;
                        scale = 1f - progress;
                    }

                    if (scale > 0f) {
                        spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, texture.Size() / 2f, scale, SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }

    public class PackAPunchEntity : ModTileEntity
    {
        public Item itemSlot = new Item(); 
        public int timer = 0;
        public PunchProcess recipe;

        public override bool IsTileValidForEntity(int x, int y) => 
            Main.tile[x, y].HasTile && Main.tile[x, y].TileType == ModContent.TileType<PackAPunchTile>();

        public override void Update() {
            if (Main.netMode == NetmodeID.MultiplayerClient) return;
            if (itemSlot == null) itemSlot = new Item();

            if (timer == 0 && !itemSlot.IsAir) {
                recipe = PunchUpgrades.FirstOrDefault(x => x.Input == itemSlot.type);
            }

            if (recipe != null) {
                timer++;
                if (timer % 10 == 0) {
                    Dust.NewDust(Position.ToWorldCoordinates(), 32, 32, DustID.PurpleTorch);
                }
                if (timer == 120) {
                    AdvancedPopupRequest failRequest = new AdvancedPopupRequest {
                        Text = "Done",
                        Color = Color.Yellow,
                        DurationInFrames = 120,
                        Velocity = new Vector2(0f, 0f)
                    };
                    PopupText.NewText(failRequest, Position.ToWorldCoordinates() + new Vector2(8, 0));
                    
                    if (itemSlot.ModItem is Reloadable reloadableItem) {
                        reloadableItem.UpgradePap();
                    }
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item37, Position.ToWorldCoordinates());
                }
                if(timer % 260==0)
                {
                    AdvancedPopupRequest failRequest = new AdvancedPopupRequest {
                        Text = "...",
                        Color = Color.Orange,
                        DurationInFrames = 120,
                        Velocity = new Vector2(0f, 3f)
                    };
                    PopupText.NewText(failRequest, Position.ToWorldCoordinates() + new Vector2(8, 0));
                }
                if (timer >= 900) {
                    AdvancedPopupRequest failRequest = new AdvancedPopupRequest {
                        Text = "Too late!",
                        Color = Color.Red,
                        DurationInFrames = 90,
                        Velocity = new Vector2(0f, -1f)
                    };
                    PopupText.NewText(failRequest, Position.ToWorldCoordinates() + new Vector2(8, 0));
                    timer = 0;
                    itemSlot.TurnToAir();
                    recipe = null;
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
                }
            }
            else {
                timer = 0;
            }
        }

        public override void SaveData(TagCompound tag) {
            tag.Add("itemSlot", ItemIO.Save(itemSlot));
            tag.Add("timer", timer);
        }

        public override void LoadData(TagCompound tag) {
            itemSlot = ItemIO.Load(tag.GetCompound("itemSlot"));
            timer = tag.GetInt("timer");
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
            int x = i - (Main.tile[i, j].TileFrameX / 18 % 3);
            int y = j - (Main.tile[i, j].TileFrameY / 18 % 2);

            if (Main.netMode == NetmodeID.MultiplayerClient) {
                NetMessage.SendTileSquare(Main.myPlayer, x, y, 3, 2);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, x, y, Type, 0, 0, 0, 0);
                return -1;
            }
            return Place(x, y);
        }
    }

    public class PackAPunch : ModItem
    {
        public override string Texture => "Terraria/Images/Item_221";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<PackAPunchTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood, 10) 
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}