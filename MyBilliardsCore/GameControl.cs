//开启测试
#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MyBilliardsCore
{
    //对局状态
    public enum Status
    {
        WaitPlayerHit,      //等待击球，调整位置、击球点
                            //Player1Combo,   //连击
        Hitting,            //击球中，蓄力。。
        Moving,             //球还在运动
        Clear,              //清算本次击球
        Ending,             //对局结束
        Prepare,            //准备阶段
    }

    public class GameControl
    {
        private static GameControl instance;
        //单例模式
        private GameControl()
        {
            //初始化
            //Balls = new BaseBall[16];
            InitGameControl();


        }
        public static GameControl getInstance()
        {
            if (instance == null)
                instance = new GameControl();
            return instance;
        }

        public  BaseBall[] Balls=null;
        public  TableBorder[] Walls = null;
        public  Player[] Players = null;
        public  Hole[] Holes = null;

        //判断是否停止运动
        public bool IsStopMove;


        //对局状态
        public Status status;

        //运动计时器
        private static DispatcherTimer UpdateTimer = new DispatcherTimer();


        //初始化球桌
        public void InitGameControl()
        {
            //初始化球
            Balls = new BaseBall[16];
            for (int i = 0; i < 16; i++)
                Balls[i] = new BaseBall(i);

            //初始化边框
            Walls = new TableBorder[18];
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 3; j++)
                    Walls[3*i+j] = new TableBorder(Constant.BorderPoint[4*i+j],
                        Constant.BorderPoint[4*i + j + 1]);

            //初始化球洞
            Holes = new Hole[6];
            for(int i=0;i<6;i++)
                if(i==1||i==4)
                    Holes[i] = new MidHole(Constant.Hole_Postion[i]);
                else
                    Holes[i] = new EdgeHole(Constant.Hole_Postion[i]);

            //初始化玩家
            Players = new Player[2];
            Players[0] = new Player();
            Players[1] = new Player();


            //初始化状态、控制信息


            //启动日志记录

        }

        //初始化对局
        public void InitGame()
        {
            //初始化对局双方信息

            //清空记录

            //初始化球的位置
            InitBallPosition();
            //初始化状态、时间

        }

        //摆球
        public void InitBallPosition()
        {
            for (int i = 0; i < 16; i++)
            {
                Balls[i].IsStill = true;
                Balls[i].IsInHole = false;
                Balls[i].Position = Constant.InitPostion[i];
                Balls[i].Velocity = new Vector2(0,0);
            }
        }

        //开始游戏
        public void StartGame()
        {
            //初始化游戏
            InitGame();

            //进入开球

            
        }

        //玩家i击球，读取杆信息
        private void PlayerHit(int i)
        {
            //开始进入运动状态
            IsStopMove = false;

            UpdateTimer.Tick += new EventHandler(Moving);
            UpdateTimer.Interval = new TimeSpan(0,0,0,0,Constant.Time_Slot);
            UpdateTimer.Start();
        }

        //运动中
        public void Moving(object sender, EventArgs e)
        {
            if (IsStopMove)
            {//停止，进入结算

                UpdateTimer.Stop();
            }
            else
                Update();
        }
        
        void Update()
        {//每一个时隙的计算过程

            //球进行运动
            for (int i = 0; i < 16; i++)
                Balls[i].Move();

            //判断球球碰撞
            for (int i=0;i<15;i++)
                for (int j = i+1; j < 16; j++)
                    if(Balls[i].IsBallCollsion(Balls[j]))
                        Balls[i].ResovleCollsion(Balls[j]);

            //判断球墙
            for(int i=0;i<16;i++)
                for(int j=0;j<18;j++)
                    if(Walls[j].IsBallCollsion(Balls[i]))
                        Walls[j].ResovleCollsion(Balls[i]);

            //判断进洞
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 6; j++)
                    if (Holes[j].IsBallDrop(Balls[i]))
                        Holes[j].BallDrop(Balls[i]);

#if DEBUG
            for (int i = 0; i < 16; i++)
                if(Balls[i].IsInHole)
                {
                    //Balls[i].IsInHole = true;
                    Balls[i].Position = new Vector2(500, 300);
                }

#endif


            //判断运动结束
            IsStopMove = true;
            for (int i = 0; i < 16; i++)
                if (!Balls[i].IsInHole && !Balls[i].IsStill)
                    IsStopMove = false;
        }

        public void test()
        {
            /*for (int i = 2; i < 16; i++)
            {
                Balls[i].IsInHole = true;
                Balls[i].Position = new Vector2(-485,273);
            }*/


            //就假设白球有了一个初速度
            Balls[0].Velocity = new Vector2(500, -20);
            Balls[0].IsStill = false;

            PlayerHit(0);
        }
    }
}
