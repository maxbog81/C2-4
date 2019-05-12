using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Богатов Максим 

//1.	Добавить в программу коллекцию астероидов.Как только она заканчивается(все астероиды сбиты), формируется новая коллекция, в которой на один астероид больше.

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {

            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width,
                Height = Screen.PrimaryScreen.Bounds.Height
        };
        Game.Init(form);
                form.Show();
                Game.Load();
                Game.Draw();
                Application.Run(form);


        }
}
}
