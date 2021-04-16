using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    //各种常量
    public class Constant
    {
//物理常量/游戏常量
        
        //停止速度
        public const float Stop_velocity = 1f;   
        
        //摩擦系数
        public const float Table_FrictionCoefficient = 1.5f;
        public const float g = 9.8f;

        //间隔时间，多少毫秒计算一次
        public const int Time_Slot = 20;     //20毫秒
        public const float Time_Interval = (float)Time_Slot/1000;


        public const float Ball_Mess = 1.0f;

        
        //时隙内速度衰减
        public const float Ball_VelocityAttenuation = Table_FrictionCoefficient * g * Time_Interval;

        
       

        //距离位置参数（中心点为（0,0））

        /// <summary>
        /// 边线位置
        /// </summary>
        //距离上边的内侧线距离
        public const float UpBorder = 216.0f;
        //距离左边的内侧线距离
        public const float LeftBorder = 428.0f;
        //内边宽度
        public const float BorderWidth = 14.0f;
        //左上边起始点X
        public const float StartPointX = -414.0f;
        //左上边终点X
        public const float EndPointX = -22.0f;
        //左上边Y
        public const float StartEndPointY = 202.0f;

        //角洞斜率
        public const float EdgeSlope = 16.0f;
        //中洞斜率（上下相差多少像素）
        public const float MidSlope = 5.0f;

        //构成边界的点，从左上开始，顺时针记录，每四个点构成一个边
        public static readonly Vector2[] BorderPoint = new Vector2[24]
        {
        //左上边
            new Vector2(StartPointX             ,   UpBorder + BorderWidth      ),
            new Vector2(StartPointX + EdgeSlope ,   UpBorder                    ),
            new Vector2(EndPointX   - MidSlope  ,   UpBorder                    ),
            new Vector2(EndPointX               ,   UpBorder + BorderWidth      ),
        //右上边
            new Vector2(-EndPointX              ,   UpBorder + BorderWidth      ),
            new Vector2(-EndPointX   + MidSlope ,   UpBorder                    ),
            new Vector2(-StartPointX - EdgeSlope,   UpBorder                    ),
            new Vector2(-StartPointX            ,   UpBorder + BorderWidth      ),
        //右边
            new Vector2(LeftBorder + BorderWidth,   StartEndPointY              ),
            new Vector2(LeftBorder              ,   StartEndPointY - EdgeSlope  ),
            new Vector2(LeftBorder              , - StartEndPointY + EdgeSlope  ),
            new Vector2(LeftBorder + BorderWidth, - StartEndPointY              ),
        //右下边
            new Vector2(-StartPointX,- UpBorder - BorderWidth),
            new Vector2(-StartPointX-EdgeSlope,-UpBorder),
            new Vector2(-EndPointX+MidSlope,-UpBorder),
            new Vector2(-EndPointX,-UpBorder-BorderWidth),
        //左下边
            new Vector2(EndPointX, - UpBorder-BorderWidth),
            new Vector2(EndPointX-MidSlope, -UpBorder),
            new Vector2(StartPointX+EdgeSlope, -UpBorder),
            new Vector2(StartPointX, - UpBorder - BorderWidth),
        //左边
            new Vector2(-(LeftBorder+BorderWidth), -StartEndPointY),
            new Vector2(-LeftBorder, -StartEndPointY+EdgeSlope),
            new Vector2(-LeftBorder, StartEndPointY-EdgeSlope),
            new Vector2(-(LeftBorder+BorderWidth), StartEndPointY),
        };
        //转角点
        public static readonly Vector2[] CornerPoint = new Vector2[12]{
        //左上边
            new Vector2(StartPointX + EdgeSlope ,   UpBorder                    ),
            new Vector2(EndPointX   - MidSlope  ,   UpBorder                    ),
        //右上边
            new Vector2(-EndPointX   + MidSlope ,   UpBorder                    ),
            new Vector2(-StartPointX - EdgeSlope,   UpBorder                    ),
        //右边
            new Vector2(LeftBorder              ,   StartEndPointY - EdgeSlope  ),
            new Vector2(LeftBorder              , - StartEndPointY + EdgeSlope  ),
        //右下边
            new Vector2(-StartPointX-EdgeSlope,-UpBorder),
            new Vector2(-EndPointX+MidSlope,-UpBorder),
        //左下边
            new Vector2(EndPointX-MidSlope, -UpBorder),
            new Vector2(StartPointX+EdgeSlope, -UpBorder),
        //左边
            new Vector2(-LeftBorder, -StartEndPointY+EdgeSlope),
            new Vector2(-LeftBorder, StartEndPointY-EdgeSlope),
        };

        /// <summary>
        /// 洞口的位置
        /// </summary>
        //距离左上角洞口XY距离
        public const float EdgeHoleX = 440.0f;
        public const float EdgeHoleY = 228.0f;
        //距离上边中洞Y距离
        public const float MidHoleY = 240.0f;
        public const float MidHoleX = 0.0f;
        //洞口半径
        public const float EdgeHole_Radius = 24.0f;
        public const float MidHole_Radius = 21.0f;
        //上面 0 1 2 下面 3 4 5
        public static readonly Vector2[] Hole_Postion = new Vector2[6]{
            new Vector2(-EdgeHoleX,EdgeHoleY),
            new Vector2(MidHoleX,MidHoleY),
            new Vector2(EdgeHoleX,EdgeHoleY),
            new Vector2(-EdgeHoleX,-EdgeHoleY),
            new Vector2(MidHoleX,-MidHoleY),
            new Vector2(EdgeHoleX,-EdgeHoleY),
        };

        /// <summary>
        /// 球的大小、位置
        /// </summary>

        //发球线
        public const float OpenBallLine = -214.0f;
        //第一个球的 X 坐标
        public const float FirstBallX = 200 + Ball_Radius;
        public static readonly float RadiusSqrt3 =2 * Ball_Radius * (float)Math.Cos(Math.PI / 6);

        //检测碰撞形变的范围
        public const float Collsion_Delta = 1.0f;
        //真实半径和加上检测的半径
        private const float Ball_trueRadius = 15.0f;
        public const float Ball_Radius = Ball_trueRadius + Collsion_Delta;
        public const float Ball_RadiusSquared = Ball_Radius * Ball_Radius;

        //所有球的初始位置
        public static readonly int[] A_Ball = { 1, 3, 4, 8, 10, 11,14 };
        public static readonly int[] B_Ball = { 2, 6, 7, 9, 12, 13,15 };

        public static readonly Vector2[] InitPostion = new Vector2[16] {
            new Vector2(OpenBallLine,0),                            //白球

            new Vector2(FirstBallX,0),                              //A类球

            new Vector2(FirstBallX+RadiusSqrt3,Ball_Radius),        //B类球
            new Vector2(FirstBallX+RadiusSqrt3,-Ball_Radius),       //A类球

            new Vector2(FirstBallX+2*RadiusSqrt3,2*Ball_Radius),    //A类球
            new Vector2(FirstBallX+2*RadiusSqrt3,0),                //8号球
            new Vector2(FirstBallX+2*RadiusSqrt3,-2*Ball_Radius),   //B类球

            new Vector2(FirstBallX+3*RadiusSqrt3,3*Ball_Radius),    //B类球
            new Vector2(FirstBallX+3*RadiusSqrt3,Ball_Radius),      //A类球
            new Vector2(FirstBallX+3*RadiusSqrt3,-Ball_Radius),     //B类球
            new Vector2(FirstBallX+3*RadiusSqrt3,-3*Ball_Radius),   //A类球

            new Vector2(FirstBallX+4*RadiusSqrt3,4*Ball_Radius),    //A类球
            new Vector2(FirstBallX+4*RadiusSqrt3,2*Ball_Radius),    //B类球
            new Vector2(FirstBallX+4*RadiusSqrt3,0),                //B类球
            new Vector2(FirstBallX+4*RadiusSqrt3,-2*Ball_Radius),   //A类球
            new Vector2(FirstBallX+4*RadiusSqrt3,-4*Ball_Radius),   //B类球
        };

        //打乱数组返回
        public static int[] GetDisruptedItems(int[] a)
        {
            //生成一个新数组：用于在之上计算和返回
            int[] temp;
            temp = new int[a.Length];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = a[i];
            }

            //打乱数组中元素顺序
            Random rand = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < temp.Length; i++)
            {
                int x, y; int t;
                x = rand.Next(0, temp.Length);
                do
                {
                    y = rand.Next(0, temp.Length);
                } while (y == x);

                t = temp[x];
                temp[x] = temp[y];
                temp[y] = t;
            }
            return temp;
        }


        //最大力量
        public static float MAX_Strange = 500;
        public static float Detla_Strange = 10;


    }
}
