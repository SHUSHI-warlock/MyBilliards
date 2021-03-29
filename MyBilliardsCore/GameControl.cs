using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    class GameControl
    {
        public BaseBall[] Balls;
        public TableBorder[] Walls;

        private static GameControl instance;
        private GameControl()
        {
            //初始化
            Balls = new BaseBall[16];



        }
        //单例模式
        public GameControl getInstance()
        {
            if (instance == null)
                instance = new GameControl();
            return instance;
        }


    }
}
