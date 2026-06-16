using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace StardewCheat
{
    internal class CheatMenu
    {
        public struct Text
        {
            public int x, y;
            public string text;

            public int height;

            public delegate string GetTextFunction();

            public GetTextFunction GetText;

            public SpriteFont font = Game1.smallFont;

            public string GetTextDefault()
            {
                return text;
            }

            public Text(int x, int y, string text)
            {
                this.x = x; this.y = y; this.text = text; height = 0;

                GetText = GetTextDefault;
            }
            public Text(int x, int y, string text, GetTextFunction getTextFunc)
            {
                this.x = x; this.y = y; this.text = text; height = 0;

                GetText = getTextFunc;
            }

            public Text(int x, int y, int height, string text)
            {
                this.x = x; this.y = y; this.text = text; this.height = height;

                GetText = GetTextDefault;
            }
            public Text(int x, int y, int height, string text, GetTextFunction getTextFunc)
            {
                this.x = x; this.y = y; this.text = text; this.height = height;

                GetText = getTextFunc;
            }

            public Text SetFont(SpriteFont font)
            {
                this.font = font;
                return this;
            }
        }

        public bool visible = false;
        public Rectangle bounds = new Rectangle(100, 100, 400, 600);

        public List<iButton> buttons = new List<iButton>();
        public List<Text> texts = new List<Text>();

        public delegate void LogInfoFunction(string text);
        
        public event LogInfoFunction LogInfo;

        public CheatMenu(LogInfoFunction infoFunction)
        {
            LogInfo = infoFunction;
        }

        public void ButtonClicked(ButtonPressedEventArgs e)
        {
            if (!visible) return;

            if (e.Button != StardewModdingAPI.SButton.MouseLeft) return;


            for (int i = 0; i < buttons.Count; i++)
            {
                // 要加uiScale 否则和在Rendered/Rendering事件中的结果不一样
                buttons[i].MouseClicked(Game1.getMouseX(true), Game1.getMouseY(true), bounds.X, bounds.Y);
            }
            // LogInfo("Cheat Menu Clicked: " + Game1.getMouseX() * (1f / Game1.options.zoomLevel) + ", " + Game1.getMouseY() * (1f / Game1.options.zoomLevel));
        }

        public void Render(RenderedHudEventArgs e)
        {
            if (!visible) return;

            SpriteBatch b = e.SpriteBatch;

            DrawRect(b, bounds.X, bounds.Y, bounds.Width, bounds.Height,
                new Microsoft.Xna.Framework.Color(0x77777760));


            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Render(bounds.X, bounds.Y, e);
            }

            for (int i = 0; i < texts.Count; i++)
            {
                
                DrawText(b, texts[i].x + this.bounds.X, texts[i].y + this.bounds.Y, texts[i].height, texts[i].GetText(), texts[i].font);
            }
        }


        public static int drawStyle = 1;
        public static void DrawRect(SpriteBatch b, int x, int y, int width, int height, Microsoft.Xna.Framework.Color color)
        {
            if (drawStyle == 0)
            {
                IClickableMenu.drawTextureBox(
                    b,
                    x,
                    y,
                    width,
                    height,
                    color
                    );
            }
            else
            {
                b.Draw(
                    Game1.staminaRect,
                    new Microsoft.Xna.Framework.Rectangle(
                        x,
                        y,
                        width,
                        height
                    ),
                    color
                );
            }
        }

        public static SpriteFont defaultFont = Game1.smallFont;

        public static void DrawText(SpriteBatch b, int x, int y, string text, SpriteFont font = null)
        {
            b.DrawString(font == null ? Game1.smallFont : font, text,
                new Microsoft.Xna.Framework.Vector2(x, y),
                Microsoft.Xna.Framework.Color.Black);
        }

        public static void DrawText(SpriteBatch b, int x, int y, string text, Microsoft.Xna.Framework.Color color, SpriteFont font = null)
        {
            b.DrawString(font == null ? Game1.smallFont : font, text,
                new Microsoft.Xna.Framework.Vector2(x, y),
                color);
        }

        // 当设置了高度时将会垂直居中渲染
        public static void DrawText(SpriteBatch b, int x, int y, int height, string text, SpriteFont font = null)
        {
            if (height < 0)
            {
                DrawText(b, x, y, text, font);
                return;
            }

            Microsoft.Xna.Framework.Vector2 text_size = Game1.smallFont.MeasureString(text);

            y = (int)(y + ((float)height - text_size.Y) / 2.0f);

            DrawText(b, x, y, text, font);
        }

        public static void DrawText(SpriteBatch b, int x, int y, int height, string text, Microsoft.Xna.Framework.Color color, SpriteFont font = null)
        {
            if (height < 0)
            {
                DrawText(b, x, y, text, color, font);
                return;
            }

            Microsoft.Xna.Framework.Vector2 text_size = Game1.smallFont.MeasureString(text);

            y = (int)(y + ((float)height - text_size.Y) / 2.0f);

            DrawText(b, x, y, text, color, font);
        }
    }
}
