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
        Hitting,            //击球中。。
        Moving,             //球还在运动
        Clear,              //清算本次击球
        Ending,             //对局结束
        Prepare,            //准备阶段
        Foul,               //击球犯规
        SetBallandWaitHit,   //既等待击球，又可放
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

        

        //全局
        public  BaseBall[] Balls=null;
        public  TableBorder[] Walls = null;
        public CornerPoint[] Corners = null;
        public Player[] Players = null;
        public  Hole[] Holes = null;

        private BaseBall UnVisBall;         //不可视之球，用于做检测判断

        public Player currPlayer;           //当前击球方
        public int firstTouch;              //碰到的第一个球
        public List<BaseBall> dropBalls;    //本次击球的进球


        //对局状态
        
        //判断是否停止运动
        public bool IsStopMove;

        public bool isFirstHit;     //开球
        public bool isXuLi;         //蓄力
        public Status status;       //对局状态
        public int winner = -1;
        public bool setLock = false;

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

            //初始化转角
            Corners = new CornerPoint[12];
            for (int i = 0; i < 12; i++)
                Corners[i] = new CornerPoint(Constant.CornerPoint[i]);

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
            UnVisBall = new BaseBall();
            UnVisBall.IsStill = false;
            dropBalls = new List<BaseBall>();
            //启动日志记录

        }

        //初始化对局
        public void InitGame()
        {
            //初始化对局双方信息
            Players[0].Init();
            Players[1].Init();

#if DEBUG
            Players[0].name = "lzh";
            Players[1].name = "zal";
#endif


            Players[0].SetAimBall(Balls[0]);
            Players[1].SetAimBall(Balls[0]);



            //清空记录

            //初始化球的位置
            InitBallPosition();
            //初始化状态、时间
            winner = -1;
            isFirstHit = true;

            status = Status.Prepare;    //准备阶段

        }

        //初始摆球
        public void InitBallPosition()
        {
            //设置其他属性
            for(int i=0;i<16;i++)
            {
                Balls[i].IsStill = true;
                Balls[i].IsInHole = false;
                Balls[i].Velocity = new Vector2(0, 0);
            }

            //设置位置
            int[] ab = { 0, 1 };
            ab = Constant.GetDisruptedItems(ab);
            int[] a_ball = Constant.GetDisruptedItems(Constant.A_Ball);
            int[] b_ball = Constant.GetDisruptedItems(Constant.B_Ball);

            for (int i = 0; i < 7; i++)
                Balls[ab[0] * 8 + 1+i].Position = Constant.InitPostion[a_ball[i]];
            for (int i = 0; i < 7; i++)
                Balls[ab[1] * 8 + 1+i].Position = Constant.InitPostion[b_ball[i]];
            //黑白
            Balls[0].Position = Constant.InitPostion[0];
            Balls[8].Position = Constant.InitPostion[5];
        }

        //开始游戏
        public void StartGame()
        {
            //初始化游戏
            InitGame();

            //进入开球
            currPlayer = Players[0];
            NextStatus(Status.SetBallandWaitHit);
            /*
            currPlayer.club.IsHide = true;
            setLock = false;
            status = Status.SetBallandWaitHit;
            */

        }

        //尝试摆球
        public bool TrySetBall(Vector2 point)
        {
            if (point.Y > Constant.UpBorder || point.Y < -Constant.UpBorder || point.X > Constant.LeftBorder || point.X < -Constant.LeftBorder)
                return false;

            UnVisBall.Position = point;
            for (int j = 1; j < 16; j++)
                if (UnVisBall.IsBallCollsion(Balls[j]))
                    return false;
            for (int j = 0; j < 18; j++)
                if (Walls[j].IsBallCollsion(UnVisBall))
                    return false;
            //判断转角
            for (int j = 0; j < 12; j++)
                if (Corners[j].IsBallCollsion(UnVisBall))
                    return false;

            //开球有要求
            if(isFirstHit&& point.X > Constant.OpenBallLine)
                return false;
            

            return true;
        }

        //确定摆球
        public void SetBall(Vector2 point)
        {
            Balls[0].Position = point;
        }

        //更新状态
        void NextStatus(Status next_status)
        {
            //旧状态复原
            switch (status)
            {
                case Status.SetBallandWaitHit:
                    isXuLi = false;
                    setLock = false;
                    currPlayer.club.IsHide = true;
                    break;
                case Status.WaitPlayerHit:
                    isXuLi = false;
                    setLock = true;
                    currPlayer.club.IsHide = true;
                    break;
                case Status.Moving:
                    IsStopMove = true;
                    Account();  //清除
                    break;
                default:
                    break;
            }

            //新状态赋值
            switch (next_status)
            {
                case Status.SetBallandWaitHit:
                    currPlayer.club.HitPoint = Balls[0].Position;
                    isXuLi = false;
                    setLock = false;
                    currPlayer.club.IsHide = true;
                    break;
                case Status.WaitPlayerHit:
                    currPlayer.club.HitPoint = Balls[0].Position;
                    isXuLi = false;
                    setLock = true;
                    currPlayer.club.IsHide = false;
                    break;

                default:
                    break;
            }
            status = next_status;
        }

        //玩家i击球，读取杆信息
        public void PlayerHit()
        {
#if DEBUG
            System.Console.Out.WriteLine("玩家"+currPlayer.name+"击球！");
#endif


            currPlayer.HitBall();

            //开始进入运动状态
            IsStopMove = false;

            NextStatus(Status.Moving);

            for (int i=0;i<16;i++)
                Balls[i].IsTouchWall = false;
            
            firstTouch = -1;
            dropBalls.Clear();

            UpdateTimer.Tick += new EventHandler(Moving);
            UpdateTimer.Interval = new TimeSpan(0,0,0,0,Constant.Time_Slot);
            UpdateTimer.Start();

        }

        //运动中
        public void Moving(object sender, EventArgs e)
        {
            if (!IsStopMove)
                Update();
            else
            {
                //停止，进入结算
                UpdateTimer.Stop();
                NextStatus(Status.Clear);
                return;
            }
        }

        //更新位置
        void Update()
        {//每一个时隙的计算过程

            //球进行运动
            for (int i = 0; i < 16; i++)
                Balls[i].Move();

            //判断球球碰撞

            for (int i = 0; i < 15; i++)
                for (int j = i + 1; j < 16; j++)
                    if (Balls[i].IsBallCollsion(Balls[j])){
                        if (firstTouch == -1)
                            firstTouch = j;
                        Balls[i].ResovleCollsion(Balls[j]);
                    }

            //判断球墙
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 18; j++)
                    if (Walls[j].IsBallCollsion(Balls[i]))
                        Walls[j].ResovleCollsion(Balls[i]);

            //判断转角
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 12; j++)
                    if (Corners[j].IsBallCollsion(Balls[i]))
                        Corners[j].ResovleCollsion(Balls[i]);

            //判断进洞
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 6; j++)
                    if (Holes[j].IsBallDrop(Balls[i]))
                    {
                        Holes[j].BallDrop(Balls[i]);
#if DEBUG
                        if(i==0)
                            System.Console.Out.WriteLine("白球进了"+j+"号洞！");
                        else
                            System.Console.Out.WriteLine(i+"号球进了" + j + "号洞！");

#endif
                        dropBalls.Add(Balls[i]);
                    }


            //判断运动结束
            IsStopMove = true;
            for (int i = 0; i < 16; i++)
                if (!Balls[i].IsInHole && !Balls[i].IsStill)
                    IsStopMove = false;
        }

        //结算击球结果
        void Account()
        {
            

            bool WhiteBallDrop = Balls[0].IsInHole;
            bool BlackBallDrop = Balls[8].IsInHole;
            bool IsHitSelfBall = false;
            //判断是否击中己方球
            if (firstTouch != -1) {
                IsHitSelfBall = currPlayer.IsMySuit(firstTouch);
            }

            int BallTouchWall = 0;
            for (int i = 0; i < 16; i++)
                if (Balls[i].IsTouchWall) BallTouchWall++;
            

            if (isFirstHit)
            {//开球杆
                if (BlackBallDrop)
                {//直接死，不用想
#if DEBUG
                    System.Console.Out.WriteLine("玩家" + currPlayer.name + "打进黑8，游戏结束！");
#endif
                    NextStatus(Status.Ending);
                    GameOver();
                }
                else if (WhiteBallDrop|| BallTouchWall<4)
                {//开杆白球落袋，少于四颗球吃库，交换击球方
                    if (WhiteBallDrop)  //进白球要拿出来
                        Balls[0].IsInHole = false;
                    NextStatus(Status.Foul);
                    PlayerFoul();

                }
                else if(dropBalls.Count!=0)
                {//连杆
                    isFirstHit = false;
                    NextStatus(Status.WaitPlayerHit);
                }
                else
                {//交换击球
                    ChangePlayer();
                    isFirstHit = false;
                    NextStatus(Status.WaitPlayerHit);

                }
            }
            else if(currPlayer.suit==8)
            {//绝杀杆

            }
            else
            {//正常杆
                if (BlackBallDrop)
                {//直接死，不用想
                    GameOver();
                    NextStatus(Status.Ending);
#if DEBUG
                    System.Console.Out.WriteLine("玩家" + currPlayer.name + "打进黑8，游戏结束！");
#endif
                }
                else if ((dropBalls.Count != 0&&BallTouchWall ==0)  //没有球进且没有碰边
                ||(WhiteBallDrop)                                   //进白球
                ||(!IsHitSelfBall)                                   //打的不是自己的球
                ){
                    if (WhiteBallDrop)  //进白球要拿出来
                        Balls[0].IsInHole = false;

                    NextStatus(Status.Foul);
                    PlayerFoul();
                }
                else{
                    //判断是否可以连击
                    bool Com = false;

                    //如果还没确定击打
                    if (dropBalls.Count != 0&&currPlayer.suit == -1)
                    {
                        int big = 0;        //记录大小球当前分别还剩多少，给他最少的那个，不能亏了
                        int small = 0;
                        foreach (BaseBall ball in Balls)
                        {
                            if (ball.IsInHole && ball.ID < 8)
                                small++;
                            if (ball.IsInHole && ball.ID > 8)
                                big++;
                        }
                        if (big > small)
                        {
                            SetPlayerTask(1);
#if DEBUG
                            System.Console.Out.WriteLine("玩家" + currPlayer.name + "打大球");
#endif
                        }
                        else
                        {
                            SetPlayerTask(0);
#if DEBUG
                            System.Console.Out.WriteLine("玩家" + currPlayer.name + "打小球");
#endif
                        }

                        Com = true;
                    }
                    else
                    {
                        foreach (BaseBall ball in dropBalls)
                            if (currPlayer.IsMySuit(ball.ID))
                            {
                                Com = true;
                                break;
                            }
                    }

                    if(Com)
                    {//连杆
                        ContinueHit();
                        NextStatus(Status.WaitPlayerHit);
                    }
                    else
                    {//交换击球
                        ChangePlayer();
                        NextStatus(Status.WaitPlayerHit);
                    }

                }
            }
        }

        //分配击球任务,0 小 1 大
        void SetPlayerTask(int flag)
        {
            if (currPlayer == Players[0])
            {
                Players[0].suit = flag;
                Players[1].suit = 1 - flag;
            }
            else
            {
                Players[0].suit = 1-flag;
                Players[1].suit = flag;
            }

            //通知界面

        }

        //交换击球方
        void ChangePlayer()
        {
            if (currPlayer == Players[0])
                currPlayer = Players[1];
            else
                currPlayer = Players[0];
#if DEBUG
                System.Console.Out.WriteLine("接下来轮到玩家"+currPlayer.name+"击球了！");
#endif
        }
        //连击
        void ContinueHit()
        {
#if DEBUG
            System.Console.Out.WriteLine("玩家" + currPlayer.name + "连击！");
#endif
            currPlayer.combo++;
        }

        //犯规
        void PlayerFoul()
        {
            //更新，通知
#if DEBUG
            System.Console.Out.WriteLine("玩家" + currPlayer.name + "犯规！");
#endif
            //交换
            ChangePlayer();
            NextStatus(Status.SetBallandWaitHit);
            
        }

        //游戏结束
        void GameOver()
        {
            System.Console.Out.WriteLine("游戏结束"); 
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

            PlayerHit();
        }
    }
}
