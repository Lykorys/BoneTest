using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;

namespace BlackOps3.Content.Items.Overwritten
{
    public class BossDropModifier : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.WallofFlesh)
            {

                npcLoot.Add(ItemDropRule.Common(ItemID.Diamond, chanceDenominator: 5, minimumDropped: 1, maximumDropped: 3));
            }
        }
        public class BagDropModifier : GlobalItem
        {
            public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
            {
                if (item.type == ItemID.WallOfFleshBossBag)
                {
                    itemLoot.Add(ItemDropRule.Common(ItemID.Diamond, chanceDenominator: 5, minimumDropped: 1, maximumDropped: 3));
                }
            }
        }
    }
}