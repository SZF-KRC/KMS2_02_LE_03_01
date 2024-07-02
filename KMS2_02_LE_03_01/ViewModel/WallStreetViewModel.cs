using KMS2_02_LE_03_01.Manager.ApiManager;
using KMS2_02_LE_03_01.Model.WallStreetModel;
using KMS2_02_LE_03_01.MVVM;
using System.Collections.ObjectModel;

namespace KMS2_02_LE_03_01.ViewModel
{
    public class WallStreetViewModel : ViewModelBase
    {
        private readonly string url = "https://newsapi.org/v2/everything?domains=wsj.com&apiKey=ae4bf5d8949f4f6db9b1fdf2dfdede89";

        private ObservableCollection<Article> articles;
        public ObservableCollection<Article> Articles
        {
            get { return articles; }
            set { articles = value; OnPropertyChanged(); }
        }

        private Article selectedArticle;
        public Article SelectedArticle
        {
            get { return selectedArticle; }
            set { selectedArticle = value; OnPropertyChanged(); UpdateDetails(); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        private string author;
        public string Author
        {
            get { return author; }
            set { author = value; OnPropertyChanged(); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        private string publishedAt;
        public string PublishedAt
        {
            get { return publishedAt; }
            set { publishedAt = value; OnPropertyChanged(); }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }

        public WallStreetViewModel()
        {
            LoadData();
        }

        public async void LoadData()
        {
            var data = await ApiClient.GetDataFromApi<NewsApiResponse>(url);
            if (data != null && data.Articles != null)
            {
                Articles = new ObservableCollection<Article>(data.Articles);
            }
        }

        private void UpdateDetails()
        {
            if (SelectedArticle != null)
            {
                Title = SelectedArticle.title;
                Author = SelectedArticle.author;
                Description = SelectedArticle.description;
                PublishedAt = SelectedArticle.publishedAt;
                Content = SelectedArticle.content;
            }
        }
 
        
    }
}
