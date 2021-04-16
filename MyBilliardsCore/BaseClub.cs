using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyBilliardsCore
{
    //球杆
    public class BaseClub : INotifyPropertyChanged
    {
        private Vector2 angle { get; set; }  //角度
        private float strange { get; set; }  //力量
        private Vector2 hitPoint{ get;set;}  //击球点
        private bool isHide { get; set; }

        public Vector2 Angle
        {
            get { return angle; }
            set
            {
                angle = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Velocity"));
            }
        }

        public float Strange
        {
            get { return strange; }
            set
            {
                strange = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Velocity"));
            }
        }

        public Vector2 HitPoint
        {
            get { return hitPoint; }
            set
            {
                hitPoint = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("HitPoint"));
            }
        }

        public bool IsHide
        {
            get { return isHide; }
            set
            {
                isHide = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsHide"));
            }
        }

      

        public event PropertyChangedEventHandler PropertyChanged;

        //击球，给这个球附上一个什么样的状态
        public void Hit(BaseBall ball)
        {
            ball.Velocity = Vector2.Normalize(angle) * Strange;
            ball.IsStill = false;
        }
    }
}
