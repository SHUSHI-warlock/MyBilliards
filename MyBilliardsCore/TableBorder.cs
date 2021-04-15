using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    public class TableBorder
    {//边框的一条边
        //一条边的两点
        Vector2 Point1 { get; set; }
        Vector2 Point2 { get; set; }
        float Linesqr;
        //单位向量
        Vector2 Line { get; set; }



        public TableBorder(Vector2 P1, Vector2 P2)
        {
            Point1 = P1;
            Point2 = P2;
            Linesqr = (P1 - P2).LengthSquared();
            Line = Vector2.Normalize(Point2 - Point1);
        }

        public bool IsBallCollsion(BaseBall ball)
        {
            if (!ball.IsInHole && !ball.IsStill)
            {//计算球到边的距离
                Vector2 L1 = ball.Position - Point1;
                float L1sqr = L1.LengthSquared();
                float L2sqr = (ball.Position - Point2).LengthSquared();

                //最大边平方大于两边平方和，钝角
                if (Math.Max(L1sqr,L2sqr)-Math.Min(L1sqr, L2sqr)> Linesqr)
                    return false;
                
                float dot = Vector2.Dot(L1, Line);
                if (L1.LengthSquared() < Constant.Ball_RadiusSquared + dot * dot)
                    return true;
            }
            return false;
        }

        public void ResovleCollsion(BaseBall ball)
        {//根据角度反射出去

            Vector2 L1 = ball.Position - Point1;
            float dot = Vector2.Dot(L1, Line);

            Vector2 L2 = L1 - Line * dot;
            Vector2 L2N = Vector2.Normalize(L1 - Line * dot);   //垂直方向向量
            //挤出去
            ball.Position += L2N * (Constant.Ball_Radius / L2.Length());

            //反弹
            Vector2 Vx = L2N * Vector2.Dot(ball.Velocity, L2N);
            ball.Velocity -= 2 * Vx;
           
        }

        public float X1 { get { return Point1.X; } }
        public float Y1 { get { return Point1.Y; } }
        public float X2 { get { return Point2.X; } }
        public float Y2 { get { return Point2.Y; } }

    }
}
