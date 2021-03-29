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
        Vector2 Line { get; set; }

        TableBorder(Vector2 P1, Vector2 P2)
        {
            Point1 = P1;
            Point2 = P2;
            Line = Vector2.Normalize(Point2 - Point1);
        }

        public bool IsBallCollsion(BaseBall ball)
        {
            if (!ball.IsInHole && !ball.IsStill)
            {//计算球到边的距离
                Vector2 L1 = ball.Position - Point1;
                float dot = Vector2.Dot(L1, Line);
                if (dot < 0)  //不在线内
                    return false;
                if (L1.LengthSquared() < Constant.Ball_RadiusSquared + dot * dot)
                    return true;
            }
            return false;
        }

        public void ResovleCollsion(BaseBall ball)
        {//根据角度反射出去

            Vector2 L1 = ball.Position - Point1;
            float dot = Vector2.Dot(L1, Line);
            Vector2 L2 = L1 - Line * dot;   //垂直方向向量
            //挤出去
            ball.Position *= (Constant.Ball_Radius / L2.Length());

            //反弹
            Vector2 Vx = Vector2.Normalize(L2) * Vector2.Dot(ball.Velocity, Vector2.Normalize(L2));
            ball.Velocity -= 2 * Vx;

        }
    }
}
