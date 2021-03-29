using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    //各种常量
    class Constant
    {
//物理常量/游戏常量
        
        //停止速度
        public const float Stop_velocity = 1e-3f;   
        
        //摩擦系数
        public const float Table_FrictionCoefficient = 0.1f;
        public const float g = 9.8f;

        //间隔时间，多少毫秒计算一次
        public const int Time_Interval = 20;

        public const float Ball_Mess = 1.0f;

        //检测碰撞形变的范围
        public const float Collsion_Delta = 1.0f;
        //真实半径和加上检测的半径
        private const float Ball_trueRadius = 8.0f;
        public const float Ball_Radius = Ball_trueRadius+ Collsion_Delta;
        public const float Ball_RadiusSquared = Ball_Radius * Ball_Radius;

        //时隙内速度衰减
        public const float Ball_VelocityAttenuation = Table_FrictionCoefficient * g * Time_Interval;

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
        //初始位置
        public static Vector2[] InitPostion = new Vector2[16] {
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
            new Vector2(0,0),
        };

        //构成边界的点
        public static Vector2[] BorderPoint = new Vector2[] { };

        
    }
}
