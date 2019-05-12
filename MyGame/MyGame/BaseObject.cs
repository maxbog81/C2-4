﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    interface ICollision
    {
        bool Collision(ICollision obj);
        Rectangle Rect { get; }
    }

    public delegate void Message();

    abstract class BaseObject : ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }
        public abstract void Draw();
        public abstract void Update();

        // Так как переданный объект тоже должен будет реализовывать интерфейс ICollision, мы 
        // можем использовать его свойство Rect и метод IntersectsWith для обнаружения пересечения с
        // нашим объектом (а можно наоборот)
        public bool Collision(ICollision o) => o.Rect.IntersectsWith(this.Rect);

        public Rectangle Rect => new Rectangle(Pos, Size);

    }

}
