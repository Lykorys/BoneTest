using Terraria;
using Terraria.ModLoader;

namespace BlackOps3.Content.Systems
{
    public class TimeLockSystem : ModSystem
    {
        public static bool timeLock;
        private double lockedTime;
        private bool lockedDayState;
        private bool wasLocked;

        public override void PreUpdateWorld() {
            if(timeLock){
                if(!wasLocked){
                    lockedTime = Main.time;
                    lockedDayState = Main.dayTime;
                    wasLocked = true;
                }
                Main.time = lockedTime;
                Main.dayTime = lockedDayState;
            }
            else{
                wasLocked = false; 
            }

        }
    }
}