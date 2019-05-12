using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using static System.Console;

namespace MyGame
{
    delegate void GetLogs(string Value);

    class GameObjectException : Exception
    {
        public GameObjectException(string message)
            : base(message)
        {
        }
    }

    static class Game
    {

        //private static Bullet _bullet;
        private static List<Bullet> _bullets = new List<Bullet>();
        //private static Asteroid[] _asteroids;
        private static List<Asteroid> _asteroids = new List<Asteroid>();
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));
        private static IoLogs print = new IoLogs();//объект для логов

        public static int NumPoints { get; set; } = 0;
        public static int Aid { get; set; } = 3; //аптечка Активируется шифтом
        public static int CountAster { get; set; } = 1; //количество астероидов

        /// <summary>
        /// Вывод логов
        /// </summary>
        /// <param name="log"></param>
        /// <param name="Method"></param>
        static void WriteLogs(string log, GetLogs Method)
        { Method(log); }
        public static void Load()
        {
            _objs = new BaseObject[30];

            var rnd = new Random();
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }

            for (var i = 0; i < CountAster; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids.Add(new Asteroid(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r)));
            }
                       
        }
        /// <summary>
        /// Добавление астероидов
        /// </summary>
        public static void AddAster()
        {
            var rnd = new Random();
            CountAster++;
            for (var i = 0; i < CountAster; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids.Add(new Asteroid(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r)));
            }

        }

        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        static Game()
        {
        }

        public static BaseObject[] _objs;
        private static Timer _timer = new Timer();
        public static Random Rnd = new Random();

        public static void Init(Form form)
        {
            //Timer timer = new Timer { Interval = 150 };
            _timer.Start();
            _timer.Tick += Timer_Tick;

            // Графическое устройство для вывода графики            
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            //обработчики событий на KeyDown
            form.KeyDown += Form_KeyDown;
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
            Ship.MessageDie += Finish;
        }

        /// <summary>
        /// обработчик событий на KeyDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.ControlKey) _bullet = new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(4, 1));
            if (e.KeyCode == Keys.ControlKey) _bullets.Add(new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(4, 1)));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
            if (e.KeyCode == Keys.ShiftKey && Aid > 0)
            {
                var rnd = new Random();
                _ship?.EnergyHigh(rnd.Next(1, 10));
                Aid--;
            }
        }


        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
            {
                a?.Draw();
            }

            foreach (Bullet b in _bullets) b.Draw();
            _ship?.Draw();
            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                Buffer.Graphics.DrawString($"Кол-во очков:{NumPoints}", SystemFonts.DefaultFont, Brushes.White, 70, 0);//подсчет очков
                Buffer.Graphics.DrawString($"Кол-во аптечек:{Aid}", SystemFonts.DefaultFont, Brushes.White, 160, 0);//аптечка              
            }
            Buffer.Render();

        }

        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            foreach (Bullet b in _bullets) b.Update();

            if (_asteroids.Count == 0) AddAster();

            foreach (Asteroid a in _asteroids.ToArray())
            {
                a.Update();
                foreach (Bullet b in _bullets.ToArray())
                {
                    if (b.Collision(a))
                    {
                        System.Media.SystemSounds.Hand.Play();
                        _asteroids.Remove(a);
                        _bullets.Remove(b);
                        NumPoints++;
                    }                
                }
                if (_ship.Collision(a))
                {
                    _ship.EnergyLow(Rnd.Next(1, 10));
                    System.Media.SystemSounds.Asterisk.Play();
                    if (_ship.Energy <= 0) _ship.Die();
                }
            }
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString($"Конец игре Кол-во очков:{NumPoints}", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }


    }
}
