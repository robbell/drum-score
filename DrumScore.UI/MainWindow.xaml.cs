using System.Collections.ObjectModel;
using System.Windows;
using DrumScore.Interpretation;
using DrumScore.ScoreSourcing;

namespace DrumScore.UI
{
    public partial class MainWindow
    {
        private readonly ScoreQueue scoreQueue;
        private readonly ObservableCollection<ScoreInfo> tweets;
        private readonly ObservableCollection<ScoreInfo> playlist;

        public ObservableCollection<ScoreInfo> Tweets
        {
            get { return tweets; }
        }

        public ObservableCollection<ScoreInfo> Playlist
        {
            get { return playlist; }
        }

        public MainWindow()
        {
            InitializeComponent();

            tweets = new ObservableCollection<ScoreInfo>();
            playlist = new ObservableCollection<ScoreInfo>();
            scoreQueue = new ScoreQueue(new TwitterScoreFeed(), new Interpreter(new Tokeniser()), new Notifications());

            TweetListView.ItemsSource = tweets;
            PlaylistView.ItemsSource = playlist;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            scoreQueue.Update();
            BindToView();
        }

        private void MoveToPlaylist(object sender, RoutedEventArgs e)
        {
            var scoresToMove = TweetListView.SelectedItems;

            if (scoresToMove == null) return;

            foreach (var score in scoresToMove)
            {
                scoreQueue.MoveToPlaylist(score as ScoreInfo);
            }

            BindToView();
        }

        private void BindToView()
        {
            tweets.Clear();
            playlist.Clear();

            foreach (var info in scoreQueue.Tweets)
            {
                tweets.Add(info);
            }

            foreach (var info in scoreQueue.Playlist)
            {
                playlist.Add(info);
            }
        }
    }
}
