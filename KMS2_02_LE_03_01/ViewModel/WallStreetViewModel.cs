using KMS2_02_LE_03_01.Manager.ApiManager;
using KMS2_02_LE_03_01.Model.WallStreetModel;
using KMS2_02_LE_03_01.MVVM;
using System.Collections.ObjectModel;

namespace KMS2_02_LE_03_01.ViewModel
{
    /// <summary>
    /// ViewModel for handling and displaying Wall Street Journal news articles.
    /// </summary>
    public class WallStreetViewModel : ViewModelBase
    {
        private readonly string url = "https://newsapi.org/v2/everything?domains=wsj.com&apiKey=here_is_your_API";
        private string title;
        private Article selectedArticle;
        private string author;
        private string description;
        private string publishedAt;
        private string content;

        private ObservableCollection<Article> articles;

        /// <summary>
        /// Gets or sets the collection of news articles.
        /// </summary>
        public ObservableCollection<Article> Articles
        {
            get { return articles; }
            set { articles = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the currently selected article.
        /// </summary>
        public Article SelectedArticle
        {
            get { return selectedArticle; }
            set { selectedArticle = value; OnPropertyChanged(); UpdateDetails(); }
        }

        /// <summary>
        /// Gets or sets the title of the selected article.
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the author of the selected article.
        /// </summary>
        public string Author
        {
            get { return author; }
            set { author = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the description of the selected article.
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the publication date of the selected article.
        /// </summary>
        public string PublishedAt
        {
            get { return publishedAt; }
            set { publishedAt = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the content of the selected article.
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Initializes a new instance of the WallStreetViewModel class and loads the data.
        /// </summary>
        public WallStreetViewModel()
        {
            LoadData();
        }

        /// <summary>
        /// Loads the news articles asynchronously from the API.
        /// </summary>
        public async void LoadData()
        {
            var data = await ApiClient.GetDataFromApi<NewsApiResponse>(url);
            if (data != null && data.Articles != null)
            {
                Articles = new ObservableCollection<Article>(data.Articles);
            }
        }

        /// <summary>
        /// Updates the details properties with the information from the selected article.
        /// </summary>
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
