﻿using iNKORE.UI.WPF.Modern.Controls;
using OplWpf.Pages;
using System.Windows;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace OplWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Tunnel TunnelPage { get; init; } = new();
        public Log LogPage { get; init; } = new();
        public About AboutPage { get; init; } = new();

        public MainWindow()
        {
            InitializeComponent();
            NavigationView_Root.SelectedItem = Navigation_Tunnel;
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = sender.SelectedItem;
            Page? page = null;

            if (item == Navigation_Tunnel)
            {
                page = TunnelPage;
            }
            else if (item == Navigation_Log)
            {
                page = LogPage;
            }
            //else if (item == NavigationViewItem_Apps)
            //{
            //    page = Page_Apps;
            //}
            else if (item == Navigation_About)
            {
                page = AboutPage;
            }

            if (page != null)
            {
                Frame_Main.Navigate(page);
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            ConfigManager.Instance.Openp2p.Stop();
            Serilog.Log.CloseAndFlush();
            base.OnClosed(e);
        }
    }
}