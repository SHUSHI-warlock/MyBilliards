using MyBilliardsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyBilliards.Body
{
    class WhiteBall : Image
    {
        //球对象
        private BaseBall body;
        
        public Vector2 Postion {
            get { return body.Position; }
            set { body.Position = value; }
        }

        public WhiteBall()
        {
            body = new BaseBall(0);
            this.DataContext = body;

            //Width = 20;
            Image img = new Image();
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();//初始化
            bmp.UriSource = new Uri(@"Image\eightBall\ball_0.png", UriKind.Relative);//设置图片路径
            bmp.EndInit();//结束初始化
            Source = bmp;//设置显示图片
        }
    }
}
