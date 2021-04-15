using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    public enum PlayerStatus
    {
        Preparing,  //未准备
        PrePared,   //已准备
        Waitting,   //等待对方击球
        Hitting,    //击球中
        Hitted,     //已击球
        foul,       //犯规
        Win,        //胜利
        Lose,       //输
    }

    public class Player
    {//玩家
        public Player() {
            club = new Club();

        }
        public Player(string name,string id) {
            this.name = name;
            this.id = id;
            club = new Club();

        }

        public string name; //姓名
        public string id;   //id
        public int score;   //得分
        public PlayerStatus status;//状态
        public int combo;   //连击次数
        public int foul;    //犯规次数
        public int suit;    //击打花色（0 单色,1 双色,2 黑8）

        List<BaseBall> unhitBalls;  //要打的球
        List<BaseBall> hitedBalls;  //已经打进的球

        //杆子
        public Club club;


    }
}
