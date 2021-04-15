using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    //球杆
    public class Club
    {
        public Vector2 angle { get; set; }  //角度
        public float Strange { get; set; }  //力量
        public Vector2 HitPoint{ get;set;}  //击球点

        //击球，给这个球附上一个什么样的状态
        public void Hit(BaseBall ball)
        {
            ball.Velocity = angle * Strange;
        }
    }
}
