using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.ApplicationLifetimes;
using CodeHollow.FeedReader;
using NewsReader.Interfaces;
using NewsReader.Views;
using OPML;
using ReactiveUI;

namespace NewsReader.ViewModels;

public class MainViewModel : ViewModelBase
{
    private AvaloniaList<string> _feeds;
    private List<string> _sources = new List<string>();

    public MainViewModel()
    {
        _feeds = new AvaloniaList<string>();
        LoadFeed();
    }

    public void EditSourceFeeds()
    {
        //todo
    }


    public async void OnDeleteClicked(object feed)  
    {
        try
        {
            // todo deleting with a dialog
            var lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime;
            var window = new ConfirmWindow();
            window.CloseRequested += (s, e) =>
            {
                ;
            };
            window.ShowDialog(lifetime.MainWindow);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    public void OnRefreshedClicked()
    {
        try
        {
            LoadFeed();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    private async void LoadFeed()
    {
        try
        {
            string file2load = Path.Combine(Directory.GetCurrentDirectory(), "subscriptions.opml");

            IOPML opml = new OpmlDataProvider();
            var data = opml.LoadOpml(file2load);

            foreach (var outline in data.Body.Outlines)
            {
                if (!string.IsNullOrWhiteSpace(outline.XMLUrl))
                    _sources.Add(outline.XMLUrl);

                foreach (var innerOutline in outline.Outlines)
                {
                    if (!string.IsNullOrWhiteSpace(innerOutline.XMLUrl))
                        _sources.Add(innerOutline.XMLUrl);
                }
            }

            int articles = 0;
            int feeds =0;
            foreach (string source in _sources)
            {
                Feed? feed = null;
                try
                {

                    feed = await FeedReader.ReadAsync(source);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    
                }
                feeds ++;
                if (feed == null)
                    continue;

                foreach (FeedItem? article in feed.Items)
                {
                    articles ++;
                    if(article == null)
                        continue;
                    try
                    {
                        // titles seems to have white space and return lines
                        Feeds.Add(article.Title.TrimStart().TrimEnd());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }


            FeedSources = $"{_sources.Count} feeds";
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    public string? FeedSources { get; set; }

    public AvaloniaList<string> Feeds
    {
        get => _feeds;
        set => this.RaiseAndSetIfChanged(ref _feeds, value);
    }
}