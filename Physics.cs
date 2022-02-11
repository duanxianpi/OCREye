using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCREye
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Physics2D;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Factories;
    using Physics2D.Object;
    using Physics2D.Object.Tools;
    using Physics2D.Force.Zones;
    using Physics2D.Force;
    using OCREye.Graphic;
    using System.Diagnostics;
    using System.Windows.Input;

    class Physics : PhysicsGraphic, IDrawable
    {
        //边框点坐标
        private readonly List<Vector2D> edgePoints;

        //阻力区
        private readonly Zone dragZone;

        //阻力
        private readonly ParticleDrag drag = new ParticleDrag(2, 1);

        private readonly int windowHeight = 0;

        private readonly int windowWidth = 0;

        private double radians = 0;

        //粒子
        private Particle Pb;

        /// <summary>
        /// 当前状态
        /// </summary>
        private State state = State.Up;

        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Vector2D mousePosition = Vector2D.Zero;

        /// <summary>
        /// 固定点操纵杆
        /// </summary>
        private Vector2D handle = Vector2D.Zero;

        private Vector2D startPosition = Vector2D.Zero;

        private Image svgImage;

        Vector2D d = Vector2D.Zero;
        Vector2D perPosition = Vector2D.Zero;

        /// <summary>
        /// 状态枚举
        /// </summary>
        private enum State
        {
            Down, Drag, Up
        }

        public Physics(Image image, int w, int h)
            : base(image)
        {
            svgImage = image;
            this.radians = svgImage.Height / 2;

            this.windowHeight = h;
            this.windowWidth = w;

            this.edgePoints = new List<Vector2D>() {
                new Vector2D(0, 0).ToSimUnits(),
                new Vector2D(this.windowWidth, 0).ToSimUnits(),
                new Vector2D(this.windowWidth, this.windowHeight).ToSimUnits(),
                new Vector2D(0, this.windowHeight).ToSimUnits()
            };

            Settings.ContactIteration = 20;

            this.Pb = this.PhysicsWorld.CreateParticle(new Vector2D(windowWidth / 2, windowHeight / 2).ToSimUnits(), new Vector2D(0, 0), 7, 2);

            this.PhysicsWorld.AddObject(this.Pb);

            // 为顶点绑定形状
            Pb.BindShape(new Circle(this.radians.ToSimUnits()));

            //设置边缘
            this.PhysicsWorld.CreatePolygonEdge(this.edgePoints.ToArray());

            // 增加重力
            this.PhysicsWorld.CreateGravity(0);

            //添加绘画队列
            this.DrawQueue.Add(this);

            this.Start = true;

            //设置帧数
            this.Slot = 1 / 180.0;

            this.dragZone = new RectangleZone(
                0.ToSimUnits(),
                0.ToSimUnits(),
                this.windowWidth.ToSimUnits(),
                this.windowHeight.ToSimUnits());

            this.dragZone.Add(this.drag);
            this.PhysicsWorld.Zones.Add(this.dragZone);
        }



        protected override void UpdatePhysics(double duration)
        {

            if (this.state == State.Drag)
            {
                this.Pb.Position = this.mousePosition - this.handle + new Vector2D(radians, radians).ToSimUnits();
            }
            //if (System.Math.Abs(this.Pb.Velocity.X) < 0.2 || System.Math.Abs(this.Pb.Velocity.Y) < 0.2)
            //{
            //    this.Pb.Velocity = Vector2D.Zero;
            //}

            BoaderCheck();

            this.PhysicsWorld.Update(duration);
        }

        public void imageDown(double x, double y)
        {
                this.state = State.Down;
                this.Pb.Velocity = Vector2D.Zero;
                handle.Set(x, y);

        }

        public void Move(double x, double y)
        {

            if (this.state != State.Up)
            {
                if (perPosition.Equals(Vector2D.Zero))
                {
                    perPosition.Set(x, y);
                }
                this.mousePosition.Set(x, y);
                d = mousePosition - perPosition;
                perPosition.Set(x, y);
                this.state = State.Drag;
            }
        }

        public void Up()
        {

            if (this.state == State.Drag)
            {
                this.Pb.Velocity = d / this.Slot;
            }
            else if (this.state == State.Down) {
                
            }

            this.state = State.Up;



        }

        public void newShape(double radians)
        {
            this.radians = radians;
            Pb.BindShape(new Circle(radians.ToSimUnits()));
        }

        public void Draw(WriteableBitmap bitmap)
        {
            //var points = new List<int>();
            // 绘制物体


            Canvas.SetLeft(this.svgImage, (this.Pb.Position.X).ToDisplayUnits() - radians);
            Canvas.SetTop(this.svgImage, (this.Pb.Position.Y).ToDisplayUnits() - radians);
            //bitmap.FillEllipseCentered((this.Pb.Position.X.ToDisplayUnits()),
            //    (this.Pb.Position.Y.ToDisplayUnits()), 40, 40, Colors.DarkRed);
            //points.Clear();
            //foreach (var point in this.edgePoints)
            //{
            //    points.Add(point.X.ToDisplayUnits());
            //    points.Add(point.Y.ToDisplayUnits());
            //}

            //points.Add(this.edgePoints[0].X.ToDisplayUnits());
            //points.Add(this.edgePoints[0].Y.ToDisplayUnits());
            //bitmap.DrawPolyline(points.ToArray(), Colors.Black);
        }

        public void BoaderCheck()
        {
            if(Pb.Position.X.ToDisplayUnits()+radians > windowWidth)
            {
                Pb.Position.X = (windowWidth - radians).ToSimUnits();
            }
            if (Pb.Position.Y.ToDisplayUnits() + radians > windowHeight)
            {
                Pb.Position.Y = (windowHeight - radians).ToSimUnits();
            }
            if (Pb.Position.X.ToDisplayUnits() - radians < 0)
            {
                Pb.Position.X = (radians).ToSimUnits();
            }
            if (Pb.Position.Y.ToDisplayUnits() + radians < 0)
            {
                Pb.Position.Y = (radians).ToSimUnits();
            }
        }
    }
}
