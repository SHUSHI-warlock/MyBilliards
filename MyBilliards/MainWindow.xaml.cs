using MyBilliards.Body;
using MyBilliards.Converter;
using MyBilliardsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MyBilliards
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameControl gameControl = null;

        Club club ;
        //变换

        public MainWindow()
        {
            InitializeComponent();

            gameControl = GameControl.getInstance();
            //初始化UI
            InitUI();
            
            ShowLogicBorder();

            //Console console = new Console();
            //console.Show();

            gameControl.InitGame();

            test();
        }

        public void test()
        {
            gameControl.StartGame();
            //gameControl.test();

        }

        //初始化UI
        public void InitUI()
        {
            for(int i=0;i<gameControl.Balls.Length;i++)
            {
                //添加ui
                Ball ball = new Ball();
                ball.Name = String.Format("_{0}_ball", i);
                Desk.Children.Add(ball);
                //ball.SetValue(Canvas.TopProperty)

                //挂载数据
                ball.SetBody(gameControl.Balls[i]);

                //设置绑定
                Binding bindingTop = new Binding("Position");
                bindingTop.Converter = new PostionY_Converter();
                ball.SetBinding(Canvas.TopProperty, bindingTop);

                Binding bindingLeft = new Binding("Position");
                bindingLeft.Converter = new PostionX_Converter();
                ball.SetBinding(Canvas.LeftProperty, bindingLeft);

                Binding BindingVis = new Binding("IsInHole");
                BindingVis.Converter = new VisibityConverter();
                ball.SetBinding(Canvas.VisibilityProperty, BindingVis);
                //
            }


            club = new Club();
            club.Name = "Club";
            Desk.Children.Add(club);

            club.SetBody(gameControl.Players[0].club);

            //设置绑定
            Binding binding_Top = new Binding("HitPoint");
            binding_Top.Converter = new PostionY_Converter();
            club.SetBinding(Canvas.TopProperty, binding_Top);

            Binding binding_Left = new Binding("HitPoint");
            binding_Left.Converter = new PostionX_Converter();
            club.SetBinding(Canvas.LeftProperty, binding_Left);

            Binding Binding_Vis = new Binding("IsHide");
            Binding_Vis.Converter = new VisibityConverter();
            club.SetBinding(Canvas.VisibilityProperty, Binding_Vis);

            


            //club.RenderTransform = new RotateTransform(5,642,15);
            /*
            club.RenderTransform.SetValue(RotateTransform.AngleProperty, 5);
            club.RenderTransform.SetValue(RotateTransform.CenterXProperty, 642);
            club.RenderTransform.SetValue(RotateTransform.CenterYProperty, 15);
            */

            /*
            white_ball.SetBody(gameControl.Balls[0]);
            Ball temp;
            for (int i = 1; i < 16; i++)
            {
                temp = (Ball)FindName(String.Format("_{0}_ball", i));
                temp.SetBody(gameControl.Balls[i]);
            }
           */
            System.Console.WriteLine("初始完毕");
        }

        //画出逻辑边界对应位置
        public void ShowLogicBorder()
        {
            DeskBackground.Opacity = 0.5;
            //画出边框
            for (int i = 0; i < 18; i++)
            {
                Line line1 = new Line();
                line1.Stroke = System.Windows.Media.Brushes.Black;
                line1.StrokeThickness = 3;
                line1.X1 = UIConstant.Logic2AbsX(gameControl.Walls[i].X1)+ Constant.Ball_Radius;
                line1.Y1 = UIConstant.Logic2AbsY(gameControl.Walls[i].Y1)+ Constant.Ball_Radius;
                line1.X2 = UIConstant.Logic2AbsX(gameControl.Walls[i].X2)+ Constant.Ball_Radius;
                line1.Y2 = UIConstant.Logic2AbsY(gameControl.Walls[i].Y2)+ Constant.Ball_Radius;
                Desk.Children.Add(line1);
                
            }

            //画出洞口
            for (int i = 0; i < 6; i++)
            {
                Ellipse circle = new Ellipse();
                circle.Stroke = System.Windows.Media.Brushes.Black;
                circle.StrokeThickness = 3;
                circle.Width = 2 * gameControl.Holes[i].GetRadius();
                circle.Height = 2 * gameControl.Holes[i].GetRadius();
                Canvas.SetLeft(circle, UIConstant.Logic2AbsX(gameControl.Holes[i].Position.X) + Constant.Ball_Radius - gameControl.Holes[i].GetRadius());
                Canvas.SetTop(circle, UIConstant.Logic2AbsY(gameControl.Holes[i].Position.Y) + Constant.Ball_Radius - gameControl.Holes[i].GetRadius());
                Desk.Children.Add(circle);
            }


            for (int i = 0; i < 12; i++)
            {
                Ellipse circle = new Ellipse();
                circle.Stroke = System.Windows.Media.Brushes.Red;
                circle.StrokeThickness = 2;
                circle.Width = 5;
                circle.Height = 5;
                Canvas.SetLeft(circle, UIConstant.Logic2AbsX(gameControl.Corners[i].X) + Constant.Ball_Radius -1);
                Canvas.SetTop(circle, UIConstant.Logic2AbsY(gameControl.Corners[i].Y) + Constant.Ball_Radius -1);
                Desk.Children.Add(circle);
            }
        }

        //点击按钮，摆球
        private void Desk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Desk.Focus();

            if (gameControl.status == Status.SetBallandWaitHit && !gameControl.isXuLi)
            {
                if(gameControl.setLock)
                {
                    gameControl.setLock = false;
                    //球杆隐藏
                    gameControl.currPlayer.club.IsHide = true;

                }
                else
                {
                    gameControl.setLock = true;
                    //球杆显现
                    gameControl.currPlayer.club.IsHide = false;
                    gameControl.currPlayer.club.HitPoint = gameControl.Balls[0].Position;

                }
                System.Console.Out.WriteLine("锁定：" + gameControl.setLock);

            }

            /*
            System.Console.Out.WriteLine("点击鼠标");
            Point point = e.GetPosition((Canvas)sender);
            System.Console.Out.WriteLine("点击坐标 X: " + point.X + " Y: " + point.Y);
            */
        }

        //移动、摆球、球杆旋转
        private void Desk_MouseMove(object sender, MouseEventArgs e)
        {
            if (gameControl.currPlayer.club != club.GetBody())
            {
                club.SetBody(gameControl.currPlayer.club);
            }

            if (gameControl.status == Status.SetBallandWaitHit&&!gameControl.setLock)
            {
                Point point = e.GetPosition((Canvas)sender);

                Vector2 logicPoint = UIConstant.PointTrans(point);

                //System.Console.Out.WriteLine("绝对坐标 X: " + point.X + " Y: " + point.Y);
                //System.Console.Out.WriteLine("逻辑坐标 X: " + logicPoint.X + " Y: " + logicPoint.Y);
                if (gameControl.TrySetBall(logicPoint))
                    gameControl.SetBall(logicPoint);

                
            }
            else if(gameControl.status == Status.WaitPlayerHit||(gameControl.status == Status.SetBallandWaitHit && gameControl.setLock))
            {//等待击球
                if (!gameControl.isXuLi)
                {
                    //球杆旋转
                    Point point = e.GetPosition((Canvas)sender);
                    Vector2 logicPoint = UIConstant.PointTrans(point);
                    club.RotateClub(logicPoint);
                }
            }


            //System.Console.Out.WriteLine(sender.GetType());

            //Point point = e.GetPosition((Canvas)sender);
            
            //System.Console.Out.WriteLine("当前坐标 X: "+point.X+" Y: "+point.Y);

        }

        //处理键盘事件
        private void Desk_KeyDown(object sender, KeyEventArgs e)
        {

            //System.Console.Out.WriteLine("按下"+e.Key);
            if ((gameControl.status == Status.WaitPlayerHit || (gameControl.status == Status.SetBallandWaitHit && gameControl.setLock))
                &&e.Key.Equals(Key.Space))
            {
                gameControl.isXuLi = true;
                club.UpdateStrange();
                System.Console.Out.WriteLine("按下空格,正在蓄力");
            }
        }

        //
        private void Desk_KeyUp(object sender, KeyEventArgs e)
        {
          
            //System.Console.Out.WriteLine("松开"+e.Key);
            //正在蓄力
           if (gameControl.isXuLi && e.Key.Equals(Key.Space))
            {
                System.Console.Out.WriteLine("松开空格，蓄力完毕");
                gameControl.isXuLi = false;
                gameControl.PlayerHit();
            }
        }
    }
}
