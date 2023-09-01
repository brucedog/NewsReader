using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.ApplicationLifetimes;
using CodeHollow.FeedReader;
using NewsReader.DataStorage.Interfaces.UnitOfWork;
using NewsReader.Interfaces;
using NewsReader.Model;
using NewsReader.Models;
using OPML;
using ReactiveUI;

namespace NewsReader.ViewModels;

public class MainViewModel : ViewModelBase
{
    private AvaloniaList<DisplayArticle> _feeds;
    private List<string> _sources = new List<string>();
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public MainViewModel(IUnitOfWorkFactory unitOfWorkFactory)
    {
        _unitOfWorkFactory = unitOfWorkFactory;
        _feeds = new AvaloniaList<DisplayArticle>();
        LoadFeed();
    }

    public void EditSourceFeeds()
    {
        //todo
    }

    public void OnBookMarkedClicked(object content)
    {
        try
        {
            string item2remove = (string)content;
            var article = _feeds.FirstOrDefault(f => f.Title == item2remove);
            if (article != null)
            {
                var uow = _unitOfWorkFactory.Create();
                var repo = uow.GetRepository<Article>();
                Article updatedArticle = new Article
                {
                    IsBookMarkedForLater = true
                };
                repo.Update(updatedArticle.Id, updatedArticle);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    public void OnPlayContent(object content)
    {
        try
        {
            string item2remove = (string)content;
            var article = _feeds.FirstOrDefault(f => f.Title == item2remove);
            if (article != null)
            {
                HttpClient client = new HttpClient();
                var data = client.GetStringAsync(article.Link);
                var result = data.Result;
            }

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }

    public async Task OnDeleteClicked(object feed)  
    {
        try
        {
            string item2remove = (string)feed;
            var id = _feeds.FirstOrDefault(f => f.Title == item2remove);
            if (id != null)
            {
                Feeds.Remove(id);
            }
            //var deleteWindow = new DeleteConfirmationWindow
            //{
            //    DataContext = new DeleteConfirmationWindowModel(feed)
            //};
            //var lifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime;

            //deleteWindow.ShowDialog(lifetime.MainWindow);
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
        var uow = _unitOfWorkFactory.Create();
        var repo = uow.GetRepository<Article>();
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
            int feeds = 0;
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

                feeds++;
                if (feed == null)
                    continue;

                foreach (FeedItem? article in feed.Items)
                {
                    articles++;
                    if (article == null)
                        continue;
                    try
                    {
                        string id = Guid.NewGuid().ToString();
                        var art = new Article
                        {
                            Id = Guid.NewGuid().ToString(),
                            Title = article.Title,
                            Author = article.Author,
                            Description = article.Description,
                            Categories = article.Categories.ToList(),
                            PublishedOn = article.PublishingDate ?? DateTime.Now,
                            Content = article.Content,
                            Link = article.Link,
                        };
                        repo.Add(id, art);

                        DisplayArticle displayArticle = new DisplayArticle
                        {
                            Id = Guid.NewGuid().ToString(),
                            Title = article.Title,
                            Description = article.Description,
                            Categories = article.Categories.ToList(),
                            PublishedOn = article.PublishingDate ?? DateTime.Now,
                            Content = article.Content,
                            Link = article.Link,
                        };

                        // titles seems to have white space and return lines
                        Feeds.Add(displayArticle);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        Feeds.OrderBy(o => o.PublishedOn);
                    }
                }
            }

            FeedSources = $"{_sources.Count} feeds";
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
        finally
        {
            uow.SaveChanges();
        }
    }

    public string? FeedSources { get; set; }

    public AvaloniaList<DisplayArticle> Feeds
    {
        get => _feeds;
        set => this.RaiseAndSetIfChanged(ref _feeds, value);
    }
}