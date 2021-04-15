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

        public MainWindow()
        {
            InitializeComponent();

            gameControl = GameControl.getInstance();

            Console console = new Console();
            console.Show();

            gameControl.InitGame();

            InitUI();

            ShowLogicBorder();

            test();
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
                //
            }
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

        }

        public void test()
        {
            
            gameControl.test();

        }
    }
}
