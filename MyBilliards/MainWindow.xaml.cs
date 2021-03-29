﻿using MyBilliards.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyBilliards
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitGame();


        }
        public void InitGame()
        {
           
            
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider sd = (Slider)sender;
            Vector2 nnow = wb.Postion;
            if (sd.Name == "x_slider")
            {
                wb.Postion = new Vector2((float)sd.Value, nnow.Y);
            }
            else
            {
                wb.Postion = new Vector2(nnow.X,(float)sd.Value);
            }
        }
    }
}
