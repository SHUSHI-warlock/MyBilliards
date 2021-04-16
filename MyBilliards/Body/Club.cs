using MyBilliardsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyBilliards.Body
{
    class Club : Image
    {
        //杆对象
        private BaseClub body;
        private TransformGroup transformGroup;
        private bool StrangeUp;     //当前力量是在增大还是减小
        private float Strange;

        public Club()
        {
            body = new BaseClub();
            //挂载
            this.DataContext = body;
            //Width = 2 * Constant.Ball_Radius;

            Image img = new Image();
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();//初始化
            bmp.UriSource = new Uri(UIConstant.ClubImage, UriKind.Relative);//设置图片路径
            bmp.EndInit();//结束初始化
            Source = bmp;//设置显示图片

            //设置变形
            transformGroup = new TransformGroup();
            transformGroup.Children.Add(new RotateTransform(0, 662, 15));
            transformGroup.Children.Add(new TranslateTransform(-646, 0));

        }

        public void RotateClub(Vector2 point)
        {
            body.Angle = point - body.HitPoint;

            RotateTransform temp = (RotateTransform)transformGroup.Children[0]; //new RotateTransform(0, 285, 15);
            //Math.Acos((double)body.Angle.)
            if (body.Angle.Y==0)
            {
                temp.Angle = body.Angle.X < 0 ? 180 : 0;
            }
            else
            {
                double ac = Math.Atan2(body.Angle.Y, body.Angle.X);
                
                temp.Angle = -180*ac/Math.PI;
            }
            this.RenderTransform = transformGroup;
        }

        //更新力量
        public void UpdateStrange()
        {
            if(StrangeUp)
            {
                body.Strange += Constant.Detla_Strange;
                if (Strange >= Constant.MAX_Strange)
                    StrangeUp = false;
            }
            else
            {
                body.Strange -= Constant.Detla_Strange;
                if (Strange <= 0)
                {
                    body.Strange = 0;
                    StrangeUp = false;
                }
            }
        }

        public BaseClub GetBody()
        {
            return body;
        }

        public void SetBody(BaseClub club)
        {
            body = club;
            //挂载
            this.DataContext = body;
            StrangeUp = true;
            Strange = 0;
        }
    }
}
