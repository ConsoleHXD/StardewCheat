using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.GameData.Buffs;
using StardewValley.Menus;
using StardewValley.Tools;
using System;

namespace StardewCheat
{
    internal sealed class Cheat : Mod
    {
        CheatMenu cheatMenu;

        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.UpdateTicking += this.GameUpdateTicking;
            helper.Events.GameLoop.UpdateTicked += this.GameUpdateTicked;

            helper.Events.Display.RenderedHud += OnRenderingHud;


            cheatMenu = new CheatMenu(LogInfo);

            iButton.LogInfo += LogInfo;

            int current_y = 10;
            int switch_x = 250;


            
            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "Stardew Cheat").SetFont(Game1.dialogueFont));
            


            current_y += 60;

            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "加移速"));
            cheatMenu.texts.Add(new CheatMenu.Text(150, current_y, 50, "5", () => { return addSpeedValue.ToString(); }));
            cheatMenu.buttons.Add(new iButton(110, current_y, 30, 50, iButton.ButtonType.Normal, (x, y) => {
                addSpeedValue -= 1;
            }, "-"));
            cheatMenu.buttons.Add(new iButton(180, current_y, 30, 50, iButton.ButtonType.Normal, (x, y) => {
                addSpeedValue += 1;
            }, "+"));
            cheatMenu.buttons.Add(new iButton(switch_x, current_y, 100, 50, iButton.ButtonType.CheckBox, (x, y) => {
                // 加移速
                addSpeed = !addSpeed;
                LogWarning("AddPlayerSpeed: " + (Game1.player.addedSpeed != 0));
            }));

            current_y += 60;

            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "无限体力"));
            cheatMenu.buttons.Add(new iButton(switch_x, current_y, 100, 50, iButton.ButtonType.CheckBox, (x, y) => {
                // 无限体力
                infiniteStamina = !infiniteStamina;
                LogWarning("InfiniteStamina: " + infiniteStamina);
            }));

            current_y += 60;

            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "暂停时间"));
            cheatMenu.buttons.Add(new iButton(switch_x, current_y, 100, 50, iButton.ButtonType.CheckBox, (x, y) => {
                // 暂停时间
                pauseTime = !pauseTime;
                LogWarning("PauseTime: " + pauseTime);
            }));

            current_y += 60;

            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "放慢时间"));
            cheatMenu.buttons.Add(new iButton(switch_x, current_y, 100, 50, iButton.ButtonType.CheckBox, (x, y) => {
                // 放慢时间
                slowTime = !slowTime;
                if (slowTime) SlowDownTime();
                else SlowDownTime(1.0f);
                LogWarning("SlowDownTime: " + slowTime);
            }));

            current_y += 60;

            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "无限血量"));
            cheatMenu.buttons.Add(new iButton(switch_x, current_y, 100, 50, iButton.ButtonType.CheckBox, (x, y) => {
                // 无限血量
                infiniteHealth = !infiniteHealth;
                LogWarning("InfiniteHealth: " + infiniteHealth);
            }));

            current_y += 60;

            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "增加伤害"));
            cheatMenu.texts.Add(new CheatMenu.Text(150, current_y, 50, "50", () => { return addAttackValue.ToString(); }));
            cheatMenu.buttons.Add(new iButton(110, current_y, 30, 50, iButton.ButtonType.Normal, (x, y) => {
                addAttackValue -= 1;
            }, "-"));
            cheatMenu.buttons.Add(new iButton(180, current_y, 30, 50, iButton.ButtonType.Normal, (x, y) => {
                addAttackValue += 1;
            }, "+"));
            cheatMenu.buttons.Add(new iButton(switch_x, current_y, 100, 50, iButton.ButtonType.CheckBox, (x, y) => {
                // 增加伤害
                addAttack = !addAttack;
                AddAttack(addAttack ? addAttackValue : 0);
                LogWarning("AddAttack: " + addAttack);
            }));

            current_y += 60;

            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "鱼瞬间上钩"));
            cheatMenu.buttons.Add(new iButton(switch_x, current_y, 100, 50, iButton.ButtonType.CheckBox, (x, y) => {
                // 鱼瞬间上钩
                fastFishing = !fastFishing;
                FishingRod.maxFishingBiteTime = fastFishing ? 600 : 30000;
                FishingRod.minFishingBiteTime = fastFishing ? 0 : 600;
                LogWarning("FastFishing: " + fastFishing);
            }));

            current_y += 60;

            cheatMenu.texts.Add(new CheatMenu.Text(10, current_y, 50, "鱼瞬间捕获"));
            cheatMenu.buttons.Add(new iButton(switch_x, current_y, 100, 50, iButton.ButtonType.CheckBox, (x, y) => {
                // 鱼瞬间捕获
                fishDirectCaught = !fishDirectCaught;
                
                LogWarning("FishDirectCaught: " + fishDirectCaught);
            }));
        }

        bool addSpeed = false;
        int addSpeedValue = 5;
        bool infiniteStamina = false;
        bool infiniteHealth = false;
        bool slowTime = false;
        bool pauseTime = false;

        int addAttackValue = 50;
        bool addAttack = false;

        bool fastFishing = false;
        bool fishDirectCaught = false;

        bool showConsole = false;

        string log = "";

        public void LogInfo(string str)
        {
            this.Monitor.Log(str, LogLevel.Info);
            str = "INFO: " + str + "\n";

            List<string> log_lines = log.Split('\n').ToList<string>();
            if (log_lines.Count > 21)
            {
                log_lines.RemoveAt(0);
                log = string.Join("\n", log_lines) + str;
            }
            else
            {
                log += str;
            }
        }
        public void LogWarning(string str)
        {
            this.Monitor.Log(str, LogLevel.Warn);
            str = "WARN: " + str + "\n";

            List<string> log_lines = log.Split('\n').ToList<string>();
            if (log_lines.Count > 21)
            {
                log_lines.RemoveAt(0);
                log = string.Join("\n", log_lines) + str;
            }
            else
            {
                log += str;
            }
        }


        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F3)
            {
                // 打开调试控制台
                showConsole = !showConsole;
            }
            if (e.Button == SButton.F5)
            {
                cheatMenu.visible = !cheatMenu.visible;
            }

#if false
            if (e.Button == SButton.F7)
            {
                // 加移速
                AddPlayerSpeed(Game1.player.addedSpeed == 0 ? 5 : 0);
                LogWarning("AddPlayerSpeed: " + (Game1.player.addedSpeed != 0));
            }
            else if (e.Button == SButton.F8)
            {
                // 无限体力
                infiniteStamina = !infiniteStamina;
                LogWarning("InfiniteStamina: " + infiniteStamina);
            }
            else if (e.Button == SButton.F9)
            {
                // 暂停时间
                pauseTime = !pauseTime;
                LogWarning("PauseTime: " + pauseTime);
            }
            else if (e.Button == SButton.F10)
            {
                // 放慢时间
                slowTime = !slowTime;
                if (slowTime) SlowDownTime();
                else SlowDownTime(1.0f);
                LogWarning("SlowDownTime: " + slowTime);
            }
            else if (e.Button == SButton.F11)
            {
                // 无限血量
                infiniteHealth = !infiniteHealth;
                LogWarning("InfiniteHealth: " + infiniteHealth);
            }
            else if (e.Button == SButton.F6)
            {
                // 增加伤害
                addAttack = !addAttack;
                AddAttack(addAttack ? 50 : 0);
                LogWarning("AddAttack: " +  addAttack);
            }
#endif
            // 输出信息
            if (e.Button == SButton.F2)
            {
                LogInfo($"{Game1.player.Name} >>> Current player.addedSpeed {(Game1.player.addedSpeed)}");
                LogInfo($"{Game1.player.Name} >>> Current player.Speed {(Game1.player.getMovementSpeed())}");

                LogInfo($"Current Game Time: {Game1.timeOfDay}");
                LogInfo($"Game1.realMilliSecondsPerGameMinute: {Game1.realMilliSecondsPerGameMinute}");
                LogInfo($"Game1.realMilliSecondsPerGameTenMinutes: {Game1.realMilliSecondsPerGameTenMinutes}");
                LogInfo($"Game1.options.uiScale: {Game1.options.uiScale}, uiMode: {Game1.uiMode}, options.zoomLevel: {Game1.options.zoomLevel}");
            }

            cheatMenu.ButtonClicked(e);
        }


        public void OnRenderingHud(object? sender, RenderedHudEventArgs e)
        {
            SpriteBatch b = e.SpriteBatch;
            if (showConsole)
            {
                

                b.DrawString(
                    Game1.smallFont,
                    "Console",
                    new Vector2(100, 100),
                    Color.White
                );

                b.DrawString(
                    Game1.smallFont,
                    log,
                    new Vector2(100, 120),
                    Color.White
                );

                b.DrawString(
                    Game1.smallFont,
                    $"Game1.options.uiScale: {Game1.options.uiScale}, uiMode: {Game1.uiMode}, options.zoomLevel: {Game1.options.zoomLevel}",
                    new Vector2(100, 500),
                    Color.White
                );
            }

            cheatMenu.Render(e);
        }

        public void GameUpdateTicking(object? sender, UpdateTickingEventArgs e)
        {
        }


        public void GameUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (infiniteStamina)
            {
                SetPlayerStamina(114514);
            }
            if (infiniteHealth)
            {
                SetPlayerHealth(114514);
            }
            FishDirectCaught();

            AddPlayerSpeed(addSpeedValue);

            if (pauseTime)
            {
                // 要暂停时间，直接每帧把游戏的时间更新计时器清零
                Game1.gameTimeInterval = 0;
            }
        }
        
        public void SlowDownTime(float multiplier = 0.584f)
        {
            Game1.realMilliSecondsPerGameMinute = (int)(700.0f / multiplier);
            Game1.realMilliSecondsPerGameTenMinutes = Game1.realMilliSecondsPerGameMinute * 10;
        }

        // 增加玩家速度，增加Buff
        public void AddPlayerSpeed(int speed)
        {
            if (speed <= 0 || !addSpeed)
            {
                Game1.player.buffs.Remove("9");
                return;
            }
            if (addSpeed && Game1.player.addedSpeed >= speed) return;

            StardewValley.GameData.Buffs.BuffAttributesData buffData =
                new StardewValley.GameData.Buffs.BuffAttributesData();
            buffData.Speed = speed;

            Game1.player.applyBuff(new Buff("9", null, null, -2, null, -1,
                new StardewValley.Buffs.BuffEffects(buffData), null, null, null));
        }

        // 设置玩家速度，但该值会在游戏更新时也同步更新回来
        public void SetPlayerSpeed(int speed)
        {
            Game1.player.Speed = speed;
        }

        // 设置玩家体力
        public void SetPlayerStamina(int value)
        {
            Game1.player.Stamina = value;
        }

        // 设置血量
        public void SetPlayerHealth(int value)
        {
            Game1.player.health = Math.Min(value, Game1.player.maxHealth);
        }

        // 增加伤害
        public void AddAttack(int attack)
        {
            if (attack <= 0)
            {
                Game1.player.buffs.Remove(Buff.attack.ToString());
                return;
            }

            StardewValley.GameData.Buffs.BuffAttributesData buffData =
                new StardewValley.GameData.Buffs.BuffAttributesData();
            buffData.Attack = attack;

            Game1.player.applyBuff(new Buff(Buff.attack.ToString(), null, null, -2, null, -1,
                new StardewValley.Buffs.BuffEffects(buffData), null, null, null));
        }

        // 鱼直接捕获
        public void FishDirectCaught()
        {
            if (Game1.player.CurrentTool is FishingRod && fishDirectCaught)
            {
                // ((FishingRod)Game1.player.CurrentTool).castingPower = 1.0f; // 蓄力条
                if (((FishingRod)Game1.player.CurrentTool).isFishing && Game1.activeClickableMenu is BobberBar) // 要先确认是在钓鱼中，且当前显示了钓鱼小游戏的浮标条
                {
                    ((BobberBar)Game1.activeClickableMenu).distanceFromCatching = 1.0f;
                }
            }
        }
    }
}
