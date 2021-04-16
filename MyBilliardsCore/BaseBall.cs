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

        public int ID { get;  set; }
        private Vector2 postion;
        public Vector2 Position {
            get { return postion; }
            set {
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

        public float Velocity_X
        {
            get { return velocity.X; }
            set { velocity.X = value; }
        }
        public float Velocity_Y
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }



        //是否在洞中
        private bool isInHole;
        public bool IsInHole
        {
            get { return isInHole; }
            set
            {
                isInHole = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsInHole"));
            }
        }

        //是否停止了
        public bool IsStill { set; get; }
        //是否碰到的边
        public bool IsTouchWall { set; get; }


        public BaseBall()
        {
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            IsInHole = false;
            IsStill = true;
        }
        public BaseBall(int Id)
        {
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            IsInHole = false;
            IsStill = true;
            ID = Id;
        }
        public BaseBall(Vector2 postion, Vector2 velocity, int Id)
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
            if (!IsInHole && !ball.IsInHole)
            {
                float dissqr = Vector2.DistanceSquared(Position, ball.Position);
                float radsqr = 4 * Radius * Radius;
                if (dissqr < radsqr)
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
            Vector2 mtr = ball.Position - Position;
            Vector2 mtrN = Vector2.Normalize(mtr);
            //右侧垂直向量
            Vector2 mtrE = new Vector2(mtrN.Y, -mtrN.X);

            //重叠部分移动向量
            Vector2 temp = mtrN * ((d - mtr.Length()) / 2);
            Position -= temp;
            ball.Position += temp;

            ///2.计算速度
            //两球碰撞，可以看成一方相对不动
            //假设能量、动量守恒，则将速度分解到中心连线方向以及相切方向
            //因为质量相等，所以不动的小球碰撞后速度为中心连线方向速度，
            //发起碰撞的小球只剩切线方向的速度

            //需要判断两球中谁是碰撞的发起者，谁是静止不动被碰撞的那个！！！！

            //对方奔向自己的速度 SubVelocity
            //对方朝向自己的方向 mtrN


            //算出双方在连线方向的分量和垂直连线方向的分量
            float thisVx = Vector2.Dot(velocity, mtrN);
            float thisVy = Vector2.Dot(velocity,mtrE);

            float ballVx = Vector2.Dot(ball.velocity, mtrN);
            float ballVy = Vector2.Dot(ball.velocity, mtrE);

            //碰撞后x方向速度交换

            //this碰撞ball
            
                velocity = mtrN * (ballVx);
                ball.velocity = mtrN * (thisVx);
                ball.IsStill = false;
                IsStill = false;
            

            velocity = Vector2.Add(velocity, mtrE* thisVy);
            ball.velocity = Vector2.Add(ball.velocity, mtrE * ballVy);

        }

        //运动
        public void Move()
        {
            if (!IsStill && !IsInHole) {
                Position += Velocity * Constant.Time_Interval;

                //这里需要先判断是否停止再左移动，不然完全碰撞后等于0,0的话做Normalize就会变为（NaN,NaN）
                if (Math.Abs(velocity.X) < Constant.Stop_velocity &&Math.Abs(velocity.Y) < Constant.Stop_velocity)
                {
                    IsStill = true;
                    velocity.X = velocity.Y = 0;
                    return;
                }

                Vector2 nomal = Vector2.Normalize(velocity);
                velocity -= nomal * Constant.Ball_VelocityAttenuation;

            }
        }
    }
}
