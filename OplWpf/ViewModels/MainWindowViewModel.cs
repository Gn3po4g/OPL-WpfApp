﻿using CommunityToolkit.Mvvm.Input;
using OplWpf.Models;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CommunityToolkit.Mvvm.Messaging;

namespace OplWpf.ViewModels;

[Injection(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient)]
public partial class MainWindowViewModel : ObservableObject
{
    public StateManager StateManager { get; }
    private readonly Openp2p openp2p;

    public MainWindowViewModel(ILogger<MainWindowViewModel> logger, StateManager stateManager, Openp2p openp2p)
    {
        StateManager = stateManager;
        this.openp2p = openp2p;
        var osVersion = Environment.OSVersion.Version;
        var fileName = Process.GetCurrentProcess().MainModule?.FileName;
        if (fileName != null)
        {
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
            Version = fileVersionInfo.FileVersion ?? "";
        }
        logger.LogInformation("----- OPENP2P Launcher by Guailoudou -----");
        logger.LogInformation("程序启动，当前版本：{Version}，更新包号：{Pvn}，系统版本：{Os}", Version, Net.Pvn, osVersion);
    }


    private string Version { get; } = "";

    public string DisplayVersion => Version + " - " + Net.Pvn;

    [ObservableProperty]
    public partial string ButtonText { get; set; } = "启动";

    [RelayCommand]
    private void DisableAll()
    {
        var config = App.GetService<IOptions<Config>>().Value;
        foreach (var app in config.Apps)
        {
            app.Enabled = 0;
        }
        config.Save();
        App.GetService<IMessenger>().Send(DisableAllMessage.Default);
    }

    [RelayCommand]
    private async Task Start()
    {
        if (StateManager.MainState == State.Stop)
        {
            await openp2p.Start();
            ButtonText = "停止";
        }
        else
        {
            openp2p.Stop();
            ButtonText = "启动";
        }
    }
}

public class DisableAllMessage { public static DisableAllMessage Default { get; } = new(); };