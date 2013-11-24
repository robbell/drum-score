using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
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

            var channel1 = new Playback(new OscOutput(12013));
            var channel2 = new Playback(new OscOutput(12014));

            scoreQueue = new ScoreQueue(new TwitterScoreFeed(), new Interpreter(new Tokeniser()), new Notifications());

            var controlMessages = new ControlMessages(scoreQueue, channel1, channel2);
            controlMessages.Initialise();

            TweetListView.ItemsSource = tweets;
            PlaylistView.ItemsSource = playlist;
            scoreQueue.QueueChanged +=
                () => Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(BindToView));
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            UpdateButton.Content = "Updating...";
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

        private void RunInBackground(Action work, Action onComplete)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, a) => work();
            worker.RunWorkerCompleted += (s, a) => onComplete();
            worker.RunWorkerAsync();
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
