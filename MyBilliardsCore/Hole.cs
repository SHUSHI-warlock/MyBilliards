using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


namespace MyBilliardsCore
{
    public class Hole
    {//球洞
        public float Radius;
        public Vector2 Position;
        
        public float GetRadius()
        {
            return Radius;
        }

        

        //判断球是否掉进洞了
        public bool IsBallDrop(BaseBall ball)
        {
            if (!ball.IsInHole)
            {
                float dissqr = Vector2.DistanceSquared(Position, ball.Position);
                float radsqr = Radius * Radius;
                if (dissqr < radsqr)
                    return true;
            }
            return false;
        }

        //球掉进洞
        public void BallDrop(BaseBall ball)
        {
            ball.IsInHole = true;
            ball.IsStill = true;
            //ball.Velocity = new Vector2(0, 0);
        }
    }
    //角洞
    public class EdgeHole : Hole
    {
        public EdgeHole(Vector2 pos)
        {
            Radius = Constant.EdgeHole_Radius;
            Position = pos;
        }
    }
    //中洞
    public class MidHole : Hole
    {
        public MidHole(Vector2 pos)
        {
            Radius = Constant.MidHole_Radius;
            Position = pos;
        }
    }

}
