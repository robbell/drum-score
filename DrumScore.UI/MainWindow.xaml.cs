using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using DrumScore.Interpretation;
using DrumScore.ScoreSourcing;

namespace DrumScore.UI
{
    public partial class MainWindow
    {
        private readonly ScoreQueue scoreQueue;
        private readonly PlaybackQueue playbackQueue;
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
            playbackQueue = new PlaybackQueue(scoreQueue, new Playback(new OscOutput()));

            TweetListView.ItemsSource = tweets;
            PlaylistView.ItemsSource = playlist;
            playbackQueue.Complete += PlaybackComplete;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            UpdateButton.Content = "Loading...";
            UpdateButton.IsEnabled = false;
            RunInBackground(scoreQueue.Update, UpdateComplete);
        }

        private void UpdateComplete()
        {
            BindToView();
            UpdateButton.Content = "Update";
            UpdateButton.IsEnabled = true;
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

        private void MoveUp(object sender, RoutedEventArgs e)
        {
            MoveItem(scoreQueue.MoveItemUp);
        }

        private void MoveDown(object sender, RoutedEventArgs e)
        {
            MoveItem(scoreQueue.MoveItemDown);
        }

        private void MoveItem(Action<ScoreInfo> moveAction)
        {
            var itemToMove = PlaylistView.SelectedItem;

            if (itemToMove == null) return;

            moveAction(itemToMove as ScoreInfo);
            BindToView();
            PlaylistView.SelectedItem = itemToMove;
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

        private void StartPlayback(object sender, RoutedEventArgs e)
        {
            playbackQueue.Play();
            PlayButton.IsEnabled = false;
        }

        private void PlaybackComplete()
        {
            PlayButton.IsEnabled = true;
        }

        private void RunInBackground(Action work, Action onComplete)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, a) => work();
            worker.RunWorkerCompleted += (s, a) => onComplete();
            worker.RunWorkerAsync();
        }
    }
}
