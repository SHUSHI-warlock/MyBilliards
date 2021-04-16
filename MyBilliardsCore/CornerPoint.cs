using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    public class CornerPoint
    {//边框中的顶点
        
        Vector2 Point { get; set; }
        
        public CornerPoint(Vector2 point)
        {
            Point = point;
        }

        public bool IsBallCollsion(BaseBall ball)
        {
            if (!ball.IsInHole && !ball.IsStill)
            {//计算球到角的距离
                Vector2 L = ball.Position - Point;

                if (L.LengthSquared() > Constant.Ball_RadiusSquared)
                    return false;

                ball.IsTouchWall = true;
                return true;
            }
            return false;
        }

        public void ResovleCollsion(BaseBall ball)
        {//根据角度反射出去

            ///1.先要把双方从碰撞的重叠部分移开(不然可能出现，这次碰撞后还是处于碰撞状态)

            //两球分别向中心点连线方向反向移动((r+delta)/2-d/2)
            

            //中心点连线向量
            Vector2 mtr = ball.Position - Point;
            Vector2 mtrN = Vector2.Normalize(mtr);
            //右侧垂直向量
            //Vector2 mtrE = new Vector2(mtrN.Y, -mtrN.X);

            //重叠部分移动向量
            Vector2 temp = mtrN * ((Constant.Ball_Radius - mtr.Length()));
            ball.Position += temp;

            //算出连线方向的分量和垂直连线方向的分量

            float ballVx = Vector2.Dot(ball.Velocity, mtrN);
            //float ballVy = Vector2.Dot(ball.Velocity, mtrE);

            //碰撞后x方向速度交换

            //this碰撞ball
            ball.Velocity = Vector2.Add(ball.Velocity, mtrN * -2* ballVx);

        }

        public float X { get { return Point.X; } }
        public float Y { get { return Point.Y; } }


    }
}
