using MyBilliardsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyBilliards
{
    class UIConstant
    {
        //桌面背景
        public const string backgroundImage = "Image/eightBall/eightBall_DeskImage.png";
        //球的背景
        public static string[] BallsImage = {
           "Image/eightBall/ball_0.png",
           "Image/eightBall/ball_1.png",
           "Image/eightBall/ball_2.png",
           "Image/eightBall/ball_3.png",
           "Image/eightBall/ball_4.png",
           "Image/eightBall/ball_5.png",
           "Image/eightBall/ball_6.png",
           "Image/eightBall/ball_7.png",
           "Image/eightBall/ball_8.png",
           "Image/eightBall/ball_9.png",
           "Image/eightBall/ball_10.png",
           "Image/eightBall/ball_11.png",
           "Image/eightBall/ball_12.png",
           "Image/eightBall/ball_13.png",
           "Image/eightBall/ball_14.png",
           "Image/eightBall/ball_15.png",
        };

        //杆
        public static String ClubImage = "Image/eightBall/eightBall_Cue.png";
        //public static String ClubImage = "Image/eightBall/eightBall_Cue_bar.png";


        

        //缩放
        public const float ScaleXY = 1;

        public const float backgroundWidth = 0;
        public const float backgroundHeight = 0;

        public const float BallWidth = 0;
        public const float BallHeight = 0;

        //坐标转换
        public static float Logic2AbsY(float y)
        {
            return -y + 273 - Constant.Ball_Radius;
        }
        public static float Logic2AbsX(float x)
        {
            return x + 485 - Constant.Ball_Radius;
        }

        public static Vector2 PointTrans(Point point)
        {
            return new Vector2((float)point.X - 485, -(float)point.Y + 273);
        }

    }
}
