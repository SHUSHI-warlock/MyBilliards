using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    public class BaseBall : INotifyPropertyChanged
    {//球类
        public static float Mess = Constant.Ball_Mess;
        public static float Radius = Constant.Ball_Radius;
        //自动同步
        public event PropertyChangedEventHandler PropertyChanged;

        public int ID { get; private set; }
        private Vector2 postion;
        public Vector2 Position {
            get { return postion; }
            set{
                postion = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Position"));
            }

        }
        //运动前的位置
        public Vector2 LastPosition { set; get; }

        private Vector2 velocity;
        public Vector2 Velocity {
            get { return velocity; }
            set
            {
                velocity = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Velocity"));
            }
        } 
        //是否在洞中
        public bool IsInHole { set; get; }
        //是否停止了
        public bool IsStill { set; get; }


        public BaseBall(int Id)
        {
            Position = new Vector2(0,0);
            Velocity = new Vector2(0, 0);
            IsInHole = false;
            IsStill = true;
            ID = Id;
        }
        public BaseBall(Vector2 postion,Vector2 velocity,int Id)
        {
            Position = postion;
            Velocity = velocity;
            IsInHole = false;
            IsStill = true;
            ID = Id;
        }

        //是否碰撞
        public bool IsBallCollsion(BaseBall ball)
        {
            if(!IsInHole&&!ball.IsInHole)
            {
                float dissqr = Vector2.DistanceSquared(Position, ball.Position);
                float radsqr = 4*Radius*Radius;
                if(dissqr<radsqr)
                    return true;
            }
            return false;
        }

        //解决碰撞
        public void ResovleCollsion(BaseBall ball)
        {
            ///1.先要把双方从碰撞的重叠部分移开(不然可能出现，这次碰撞后还是处于碰撞状态)

            //两球分别向中心点连线方向反向移动((r+delta)/2-d/2)
            float d = 2 * Radius;

            //中心点连线向量
            Vector2 mtr = Position - ball.Position;
            //重叠部分移动向量
            Vector2 temp = mtr * ((d - mtr.Length()) / 2);
            Position += temp;
            ball.Position -= temp;

            ///2.计算速度
            //两球碰撞，可以看成一方相对不动
            //假设能量、动量守恒，则将速度分解到中心连线方向以及相切方向
            //因为质量相等，所以不动的小球碰撞后速度为中心连线方向速度，
            //发起碰撞的小球只剩切线方向的速度
            Vector2 SubVelocity = Velocity - ball.Velocity;
            Vector2 Vx = Vector2.Normalize(mtr) * Vector2.Dot(SubVelocity, Vector2.Normalize(mtr));
            Vector2 Vy = SubVelocity - Vx;

            ball.Velocity = Vx;
            Velocity = Vy;
        }

        
    }
}
