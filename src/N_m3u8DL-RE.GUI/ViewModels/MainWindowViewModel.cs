using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using N_m3u8DL_RE.CommandLine;
using N_m3u8DL_RE.Config;
using N_m3u8DL_RE.DownloadManager;
using N_m3u8DL_RE.Parser;
using N_m3u8DL_RE.Parser.Config;
using N_m3u8DL_RE.Common.Enum;
using N_m3u8DL_RE.Common.Util;
using N_m3u8DL_RE.Common.Log;
using N_m3u8DL_RE.Common.Resource;

namespace N_m3u8DL_RE.GUI.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private string _url = "";
    private string _saveDir = "";
    private string _saveName = "";
    private string _threadCount = Environment.ProcessorCount.ToString();
    private string _downloadRetryCount = "3";
    private string _httpRequestTimeout = "100";
    private string _maxSpeed = "";
    private string _headers = "";
    private bool _autoSelect;
    private bool _skipMerge;
    private bool _delAfterDone = true;
    private bool _subOnly;
    private bool _disableUpdateCheck;
    private bool _isDownloading;
    private double _progress;
    private string _statusText = "就绪";
    private string _speedText = "0 KB/s";
    private string _sizeText = "0 MB";
    private string _timeText = "--:--";
    private string _logText = "";
    private string _currentLang = "zh-CN";

    public string Url
    {
        get => _url;
        set => this.RaiseAndSetIfChanged(ref _url, value);
    }

    public string SaveDir
    {
        get => _saveDir;
        set => this.RaiseAndSetIfChanged(ref _saveDir, value);
    }

    public string SaveName
    {
        get => _saveName;
        set => this.RaiseAndSetIfChanged(ref _saveName, value);
    }

    public string ThreadCount
    {
        get => _threadCount;
        set => this.RaiseAndSetIfChanged(ref _threadCount, value);
    }

    public string DownloadRetryCount
    {
        get => _downloadRetryCount;
        set => this.RaiseAndSetIfChanged(ref _downloadRetryCount, value);
    }

    public string HttpRequestTimeout
    {
        get => _httpRequestTimeout;
        set => this.RaiseAndSetIfChanged(ref _httpRequestTimeout, value);
    }

    public string MaxSpeed
    {
        get => _maxSpeed;
        set => this.RaiseAndSetIfChanged(ref _maxSpeed, value);
    }

    public string Headers
    {
        get => _headers;
        set => this.RaiseAndSetIfChanged(ref _headers, value);
    }

    public bool AutoSelect
    {
        get => _autoSelect;
        set => this.RaiseAndSetIfChanged(ref _autoSelect, value);
    }

    public bool SkipMerge
    {
        get => _skipMerge;
        set => this.RaiseAndSetIfChanged(ref _skipMerge, value);
    }

    public bool DelAfterDone
    {
        get => _delAfterDone;
        set => this.RaiseAndSetIfChanged(ref _delAfterDone, value);
    }

    public bool SubOnly
    {
        get => _subOnly;
        set => this.RaiseAndSetIfChanged(ref _subOnly, value);
    }

    public bool DisableUpdateCheck
    {
        get => _disableUpdateCheck;
        set => this.RaiseAndSetIfChanged(ref _disableUpdateCheck, value);
    }

    public bool IsDownloading
    {
        get => _isDownloading;
        set => this.RaiseAndSetIfChanged(ref _isDownloading, value);
    }

    public bool IsNotDownloading => !IsDownloading;

    public double Progress
    {
        get => _progress;
        set => this.RaiseAndSetIfChanged(ref _progress, value);
    }

    public string StatusText
    {
        get => _statusText;
        set => this.RaiseAndSetIfChanged(ref _statusText, value);
    }

    public string SpeedText
    {
        get => _speedText;
        set => this.RaiseAndSetIfChanged(ref _speedText, value);
    }

    public string SizeText
    {
        get => _sizeText;
        set => this.RaiseAndSetIfChanged(ref _sizeText, value);
    }

    public string TimeText
    {
        get => _timeText;
        set => this.RaiseAndSetIfChanged(ref _timeText, value);
    }

    public string LogText
    {
        get => _logText;
        set => this.RaiseAndSetIfChanged(ref _logText, value);
    }

    public string CurrentLang
    {
        get => _currentLang;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentLang, value);
            ResString.CurrentLoc = value;
            CultureUtil.ChangeCurrentCultureName(value);
            RaisePropertyChanged(nameof(Title));
            RaisePropertyChanged(nameof(LUrl));
            RaisePropertyChanged(nameof(LUrlWatermark));
            RaisePropertyChanged(nameof(LStartDownload));
            RaisePropertyChanged(nameof(LSaveSettings));
            RaisePropertyChanged(nameof(LSaveDir));
            RaisePropertyChanged(nameof(LBrowse));
            RaisePropertyChanged(nameof(LSaveName));
            RaisePropertyChanged(nameof(LDownloadSettings));
            RaisePropertyChanged(nameof(LThreadCount));
            RaisePropertyChanged(nameof(LRetryCount));
            RaisePropertyChanged(nameof(LTimeout));
            RaisePropertyChanged(nameof(LOptions));
            RaisePropertyChanged(nameof(LAutoSelect));
            RaisePropertyChanged(nameof(LSkipMerge));
            RaisePropertyChanged(nameof(LDelAfterDone));
            RaisePropertyChanged(nameof(LSubOnly));
            RaisePropertyChanged(nameof(LDisableUpdateCheck));
            RaisePropertyChanged(nameof(LMaxSpeed));
            RaisePropertyChanged(nameof(LMaxSpeedWatermark));
            RaisePropertyChanged(nameof(LHeaders));
            RaisePropertyChanged(nameof(LHeadersWatermark));
            RaisePropertyChanged(nameof(StatusText));
        }
    }

    public string Title => GetLocalized("gui_title");
    public string LUrl => GetLocalized("gui_url");
    public string LUrlWatermark => GetLocalized("gui_url_watermark");
    public string LStartDownload => GetLocalized("gui_start_download");
    public string LSaveSettings => GetLocalized("gui_save_settings");
    public string LSaveDir => GetLocalized("gui_save_dir");
    public string LBrowse => GetLocalized("gui_browse");
    public string LSaveName => GetLocalized("gui_save_name");
    public string LDownloadSettings => GetLocalized("gui_download_settings");
    public string LThreadCount => GetLocalized("gui_thread_count");
    public string LRetryCount => GetLocalized("gui_retry_count");
    public string LTimeout => GetLocalized("gui_timeout");
    public string LOptions => GetLocalized("gui_options");
    public string LAutoSelect => GetLocalized("gui_auto_select");
    public string LSkipMerge => GetLocalized("gui_skip_merge");
    public string LDelAfterDone => GetLocalized("gui_del_after_done");
    public string LSubOnly => GetLocalized("gui_sub_only");
    public string LDisableUpdateCheck => GetLocalized("gui_disable_update_check");
    public string LMaxSpeed => GetLocalized("gui_max_speed");
    public string LMaxSpeedWatermark => GetLocalized("gui_max_speed_watermark");
    public string LHeaders => GetLocalized("gui_headers");
    public string LHeadersWatermark => GetLocalized("gui_headers_watermark");

    public ReactiveCommand<Unit, Unit> StartDownloadCommand { get; }
    public ReactiveCommand<Unit, Unit> BrowseSaveDirCommand { get; }
    public ReactiveCommand<string, Unit> ChangeLanguageCommand { get; }

    public MainWindowViewModel()
    {
        StartDownloadCommand = ReactiveCommand.CreateFromTask(StartDownloadAsync);
        BrowseSaveDirCommand = ReactiveCommand.Create(BrowseSaveDir);
        ChangeLanguageCommand = ReactiveCommand.Create<string>(ChangeLanguage);
        
        Logger.OnLog += OnLog;

        ResString.CurrentLoc = CurrentLang;
        CultureUtil.ChangeCurrentCultureName(CurrentLang);
        StatusText = GetLocalized("gui_ready");
    }

    private void ChangeLanguage(string lang)
    {
        CurrentLang = lang;
    }

    private string GetLocalized(string key, params object[] args)
    {
        var text = GuiResString.GetText(key, CurrentLang);
        return args.Length > 0 ? string.Format(text, args) : text;
    }

    private void OnLog(string message)
    {
        LogText += message + "\n";
    }

    private void BrowseSaveDir()
    {
        var dialog = new OpenFolderDialog();
        var result = dialog.ShowAsync(new Window()).Result;
        if (!string.IsNullOrEmpty(result))
        {
            SaveDir = result;
        }
    }

    private async Task StartDownloadAsync()
    {
        if (string.IsNullOrWhiteSpace(Url))
        {
            AddLog(GetLocalized("gui_empty_url"));
            return;
        }

        IsDownloading = true;
        Progress = 0;
        StatusText = GetLocalized("gui_preparing");
        LogText = "";

        try
        {
            var option = BuildOption();
            await DoWorkAsync(option);
        }
        catch (Exception ex)
        {
            AddLog($"{GetLocalized("gui_error")}: {ex.Message}");
            StatusText = GetLocalized("gui_failed");
        }
        finally
        {
            IsDownloading = false;
        }
    }

    private MyOption BuildOption()
    {
        var option = new MyOption
        {
            Input = Url,
            SaveDir = string.IsNullOrEmpty(SaveDir) ? null : SaveDir,
            SaveName = string.IsNullOrEmpty(SaveName) ? null : SaveName,
            ThreadCount = int.TryParse(ThreadCount, out var tc) ? tc : Environment.ProcessorCount,
            DownloadRetryCount = int.TryParse(DownloadRetryCount, out var rc) ? rc : 3,
            HttpRequestTimeout = double.TryParse(HttpRequestTimeout, out var to) ? to : 100,
            AutoSelect = AutoSelect,
            SkipMerge = SkipMerge,
            DelAfterDone = DelAfterDone,
            SubOnly = SubOnly,
            DisableUpdateCheck = DisableUpdateCheck,
            Headers = ParseHeaders(),
        };

        if (!string.IsNullOrEmpty(MaxSpeed))
        {
            option.MaxSpeed = ParseSpeed(MaxSpeed);
        }

        return option;
    }

    private Dictionary<string, string> ParseHeaders()
    {
        var headers = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(Headers))
        {
            foreach (var line in Headers.Split('\n'))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                var parts = trimmed.Split(':', 2);
                if (parts.Length == 2)
                {
                    headers[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }
        return headers;
    }

    private long? ParseSpeed(string speed)
    {
        speed = speed.Trim().ToUpper();
        if (speed.EndsWith("M"))
        {
            if (long.TryParse(speed.Substring(0, speed.Length - 1), out var val))
            {
                return val * 1024 * 1024 / 8;
            }
        }
        else if (speed.EndsWith("K"))
        {
            if (long.TryParse(speed.Substring(0, speed.Length - 1), out var val))
            {
                return val * 1024 / 8;
            }
        }
        return null;
    }

    private void AddLog(string message)
    {
        LogText += $"[{DateTime.Now:HH:mm:ss}] {message}\n";
    }

    private async Task DoWorkAsync(MyOption option)
    {
        HTTPUtil.AppHttpClient.Timeout = TimeSpan.FromSeconds(option.HttpRequestTimeout);
        
        CustomAnsiConsole.InitConsole(true, true);
        
        if (!option.DisableUpdateCheck)
            _ = CheckUpdateAsync();

        Logger.IsWriteFile = true;
        Logger.InitLogFile();
        Logger.LogLevel = LogLevel.INFO;

        if (option.UseSystemProxy == false)
        {
            HTTPUtil.HttpClientHandler.UseProxy = false;
        }

        option.FFmpegBinaryPath ??= GlobalUtil.FindExecutable("ffmpeg");

        if (string.IsNullOrEmpty(option.FFmpegBinaryPath) || !File.Exists(option.FFmpegBinaryPath))
        {
            throw new FileNotFoundException(ResString.ffmpegNotFound);
        }

        AddLog($"ffmpeg => {option.FFmpegBinaryPath}");

        var headers = new Dictionary<string, string>()
        {
            ["user-agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36"
        };
        foreach (var item in option.Headers)
        {
            headers[item.Key] = item.Value;
            AddLog($"Header => {item.Key}: {item.Value}");
        }

        var parserConfig = new ParserConfig()
        {
            AppendUrlParams = option.AppendUrlParams,
            UrlProcessorArgs = option.UrlProcessorArgs,
            BaseUrl = option.BaseUrl!,
            Headers = headers,
            CustomMethod = option.CustomHLSMethod,
            CustomeKey = option.CustomHLSKey,
            CustomeIV = option.CustomHLSIv,
        };

        if (option.AllowHlsMultiExtMap)
        {
            parserConfig.CustomParserArgs.Add("AllowHlsMultiExtMap", "true");
        }

        StatusText = GetLocalized("gui_parsing");
        AddLog(GetLocalized("gui_parsing_log"));

        var url = option.Input;
        var extractor = new StreamExtractor(parserConfig);
        
        await RetryUtil.WebRequestRetryAsync(async () =>
        {
            await extractor.LoadSourceFromUrlAsync(url);
            return true;
        });

        var streams = await extractor.ExtractStreamsAsync();

        var lists = streams.OrderBy(p => p.MediaType).ThenByDescending(p => p.Bandwidth).ToList();
        var basicStreams = lists.Where(x => x.MediaType is null or MediaType.VIDEO).ToList();
        var audios = lists.Where(x => x.MediaType == MediaType.AUDIO).ToList();
        var subs = lists.Where(x => x.MediaType == MediaType.SUBTITLES).ToList();

        AddLog(GetLocalized("gui_streams_found", lists.Count, basicStreams.Count, audios.Count, subs.Count));

        foreach (var item in lists)
        {
            AddLog(item.ToString());
        }

        if (string.IsNullOrEmpty(option.SaveName))
        {
            option.SaveName = N_m3u8DL_RE.Util.OtherUtil.GetFileNameFromInput(option.Input);
        }

        var tmpDir = Path.Combine(option.TmpDir ?? Environment.CurrentDirectory, $"{option.SaveName ?? DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}");

        var selectedStreams = new List<StreamSpec>();

        if (option.AutoSelect)
        {
            if (basicStreams.Count != 0)
                selectedStreams.Add(basicStreams.First());
            var langs = audios.DistinctBy(a => a.Language).Select(a => a.Language);
            foreach (var lang in langs)
            {
                selectedStreams.Add(audios.Where(a => a.Language == lang).OrderByDescending(a => a.Bandwidth).First());
            }
            selectedStreams.AddRange(subs);
        }
        else if (option.SubOnly)
        {
            selectedStreams.AddRange(subs);
        }
        else
        {
            selectedStreams = basicStreams.Concat(audios).Concat(subs).ToList();
        }

        if (selectedStreams.Count == 0)
            throw new Exception(GetLocalized("gui_no_streams"));

        if (selectedStreams.Any(s => s.Playlist == null) || extractor.ExtractorType == ExtractorType.MPEG_DASH || extractor.ExtractorType == ExtractorType.MSS)
            await extractor.FetchPlayListAsync(selectedStreams);

        var livingFlag = selectedStreams.Any(s => s.Playlist?.IsLive == true) && !option.LivePerformAsVod;
        if (livingFlag)
        {
            AddLog(GetLocalized("gui_live_found"));
        }

        if (selectedStreams.Any(s => s.Playlist!.MediaParts.Any(p => p.MediaSegments.Any(m => m.EncryptInfo.Method == EncryptMethod.UNKNOWN))))
        {
            option.BinaryMerge = true;
        }

        AddLog($"{GetLocalized("gui_starting_download")}: {option.SaveName}");
        StatusText = GetLocalized("gui_downloading");

        var downloadConfig = new DownloaderConfig()
        {
            MyOptions = option,
            DirPrefix = tmpDir,
            Headers = parserConfig.Headers,
        };

        bool result;

        if (extractor.ExtractorType == ExtractorType.HTTP_LIVE)
        {
            var sldm = new HTTPLiveRecordManager(downloadConfig, selectedStreams, extractor);
            result = await sldm.StartRecordAsync();
        }
        else if (!livingFlag)
        {
            var sdm = new SimpleDownloadManager(downloadConfig, selectedStreams, extractor);
            result = await sdm.StartDownloadAsync();
        }
        else
        {
            var sldm = new SimpleLiveRecordManager2(downloadConfig, selectedStreams, extractor);
            result = await sldm.StartRecordAsync();
        }

        if (result)
        {
            StatusText = GetLocalized("gui_completed");
            Progress = 100;
            AddLog(GetLocalized("gui_completed_log"));
        }
        else
        {
            StatusText = GetLocalized("gui_failed");
            AddLog(GetLocalized("gui_failed_log"));
        }
    }

    private static async Task CheckUpdateAsync()
    {
        try
        {
            var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;
            string nowVer = $"v{ver.Major}.{ver.Minor}.{ver.Build}";
            string redirctUrl = await Get302Async("https://github.com/nilaoda/N_m3u8DL-RE/releases/latest");
            string latestVer = redirctUrl.Replace("https://github.com/nilaoda/N_m3u8DL-RE/releases/tag/", "");
            if (!latestVer.StartsWith(nowVer) && !latestVer.StartsWith("https"))
            {
                Logger.InfoMarkUp($"[cyan]{ResString.newVersionFound}[/] [red]{latestVer}[/]");
            }
        }
        catch
        {
        }
    }

    private static async Task<string> Get302Async(string url)
    {
        var handler = new HttpClientHandler { AllowAutoRedirect = false };
        var redirectedUrl = "";
        using var client = new HttpClient(handler);
        using var response = await client.GetAsync(url);
        if (response.StatusCode == System.Net.HttpStatusCode.Found && response.Headers.Location != null)
        {
            redirectedUrl = response.Headers.Location.AbsoluteUri;
        }
        return redirectedUrl;
    }
}

public static class GuiResString
{
    private static readonly Dictionary<string, (string zhCN, string zhTW, string enUS)> LangDict = new()
    {
        ["gui_title"] = ("N_m3u8DL-RE", "N_m3u8DL-RE", "N_m3u8DL-RE"),
        ["gui_url"] = ("URL / 文件路径", "URL / 文件路徑", "URL / File Path"),
        ["gui_url_watermark"] = ("输入m3u8链接或文件路径", "輸入m3u8連結或文件路徑", "Enter m3u8 URL or file path"),
        ["gui_start_download"] = ("开始下载", "開始下載", "Start Download"),
        ["gui_save_settings"] = ("保存设置", "保存設定", "Save Settings"),
        ["gui_save_dir"] = ("保存目录", "保存目錄", "Save Directory"),
        ["gui_browse"] = ("浏览...", "瀏覽...", "Browse..."),
        ["gui_save_name"] = ("保存文件名", "保存檔案名", "Save File Name"),
        ["gui_download_settings"] = ("下载设置", "下載設定", "Download Settings"),
        ["gui_thread_count"] = ("线程数: ", "執行緒數: ", "Threads: "),
        ["gui_retry_count"] = ("重试次数: ", "重試次數: ", "Retries: "),
        ["gui_timeout"] = ("超时(秒): ", "超時(秒): ", "Timeout(s): "),
        ["gui_options"] = ("选项", "選項", "Options"),
        ["gui_auto_select"] = ("自动选择最佳轨道", "自動選擇最佳軌道", "Auto Select Best Tracks"),
        ["gui_skip_merge"] = ("跳过合并", "跳過合併", "Skip Merge"),
        ["gui_del_after_done"] = ("删除临时文件", "刪除臨時文件", "Delete Temp Files"),
        ["gui_sub_only"] = ("只下载字幕", "只下載字幕", "Subtitle Only"),
        ["gui_disable_update_check"] = ("禁用更新检测", "禁用更新檢測", "Disable Update Check"),
        ["gui_max_speed"] = ("限速", "限速", "Speed Limit"),
        ["gui_max_speed_watermark"] = ("例如: 15M 或 100K", "例如: 15M 或 100K", "e.g.: 15M or 100K"),
        ["gui_headers"] = ("Headers", "Headers", "Headers"),
        ["gui_headers_watermark"] = ("格式: Cookie:xxx\\nUser-Agent:xxx", "格式: Cookie:xxx\\nUser-Agent:xxx", "Format: Cookie:xxx\\nUser-Agent:xxx"),
        ["gui_ready"] = ("就绪", "就緒", "Ready"),
        ["gui_empty_url"] = ("请输入URL或文件路径", "請輸入URL或文件路徑", "Please enter URL or file path"),
        ["gui_preparing"] = ("准备下载...", "準備下載...", "Preparing..."),
        ["gui_error"] = ("错误", "錯誤", "Error"),
        ["gui_failed"] = ("下载失败", "下載失敗", "Download Failed"),
        ["gui_parsing"] = ("解析流信息...", "解析串流訊息...", "Parsing Stream Info..."),
        ["gui_parsing_log"] = ("正在解析流信息...", "正在解析串流訊息...", "Parsing stream info..."),
        ["gui_streams_found"] = ("发现 {0} 个流: {1} 视频, {2} 音频, {3} 字幕", "發現 {0} 個串流: {1} 影片, {2} 音訊, {3} 字幕", "Found {0} streams: {1} video, {2} audio, {3} subtitle"),
        ["gui_no_streams"] = ("没有选择任何流", "沒有選擇任何串流", "No streams selected"),
        ["gui_live_found"] = ("检测到直播流", "檢測到直播串流", "Live stream detected"),
        ["gui_starting_download"] = ("开始下载", "開始下載", "Starting download"),
        ["gui_downloading"] = ("正在下载...", "正在下載...", "Downloading..."),
        ["gui_completed"] = ("下载完成", "下載完成", "Download Completed"),
        ["gui_completed_log"] = ("下载完成!", "下載完成!", "Download completed!"),
        ["gui_failed_log"] = ("下载失败!", "下載失敗!", "Download failed!"),
    };

    public static string GetText(string key, string lang)
    {
        if (!LangDict.TryGetValue(key, out var texts))
            return key;

        if (lang is "zh-CN" or "zh-SG" or "zh-Hans")
            return texts.zhCN;
        if (lang.StartsWith("zh-"))
            return texts.zhTW;
        return texts.enUS;
    }
}