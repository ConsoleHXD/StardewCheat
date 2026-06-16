using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace StardewCheat
{

    internal class iButton
    {

        public enum ButtonType
        {
            Normal = 0,
            CheckBox
        }
        public delegate void OnClickEvent(int x, int y);
        public event OnClickEvent OnClick;

        public delegate void LogInfoFunction(string text);
        public static event LogInfoFunction LogInfo;

        public int x;
        public int y;
        public int width;
        public int height;
        public ButtonType type;

        public string text;


        public bool iSwitch = false;

        private void iLogInfo(string text)
        {
            if (LogInfo != null)
                LogInfo(text);
        }

        public iButton(int x, int y, int width, int height, ButtonType type, string text = "")
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.type = type;
            this.text = text;
        }

        public iButton(int x, int y, int width, int height, ButtonType type, OnClickEvent callback, string text = "")
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.type = type;
            this.text = text;
            this.OnClick = callback;
        }

        public void MouseClicked(int mouseX, int mouseY, int parent_x, int parent_y)
        {
            //Microsoft.Xna.Framework.Rectangle rect =
            //    new Microsoft.Xna.Framework.Rectangle(parent_x +  this.x, parent_y + this.y, this.width, this.height);

            bool xIsIn = mouseX >= parent_x + this.x && mouseX <= parent_x + this.x + this.width;
            bool yIsIn = mouseY >= parent_y + this.y && mouseY <= parent_y + this.y + this.height;

            if (xIsIn && yIsIn)
            {
                if (type == ButtonType.CheckBox)
                    iSwitch = !iSwitch;
                OnClick(mouseX, mouseY);
            }
        }

        public void Render(int parent_x, int parent_y, RenderedHudEventArgs e)
        {
            SpriteBatch b = e.SpriteBatch;

            int abstract_x = parent_x + this.x;
            int abstract_y = parent_y + this.y;

            Microsoft.Xna.Framework.Color bg_color = (type == ButtonType.Normal
                ? Microsoft.Xna.Framework.Color.WhiteSmoke : (
                    iSwitch ? Microsoft.Xna.Framework.Color.LightSalmon : Microsoft.Xna.Framework.Color.Blue
                ));

            // 背景
            CheatMenu.DrawRect(
                b,
                abstract_x,
                abstract_y,
                this.width,
                this.height,
                bg_color
                );

            // 普通按钮绘制文本
            if (type == ButtonType.Normal)
            {
                CheatMenu.DrawText(
                    b,
                    abstract_x + 5,
                    abstract_y,
                    this.height,
                    text,
                    Microsoft.Xna.Framework.Color.Black
                    );
            }
            else // 绘制滑条
            {
                int margin = 5;

                int content_height = this.height - (margin * 2);
                int content_width = content_height;

                int content_y = abstract_y + margin;
                int content_x = iSwitch ? abstract_x + width - content_width - margin : abstract_x + margin;

                CheatMenu.DrawRect(
                    b,
                    content_x,
                    content_y,
                    content_width,
                    content_height,
                    Microsoft.Xna.Framework.Color.White
                    );
            }
        }
        
    }
}
