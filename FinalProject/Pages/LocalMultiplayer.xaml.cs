﻿using FinalProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace FinalProject.Pages
{
    /// <summary>
    /// Interaction logic for LocalMultiplayer.xaml
    /// </summary>
    public partial class LocalMultiplayer : Page
    {
        private Settings setting;
        public LocalMultiplayer(Settings setting)
        {
            InitializeComponent();
            this.setting = setting;
        }
    }
}
