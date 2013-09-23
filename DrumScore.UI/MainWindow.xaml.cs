using System.Collections.ObjectModel;
using System.Windows;

namespace DrumScore.UI
{
    public partial class MainWindow
    {
        private readonly ScoreQueue scoreQueue;
        private readonly TwitterScoreFeed feed = new TwitterScoreFeed();
        private readonly Tokeniser tokeniser = new Tokeniser();
        private readonly Interpreter interpreter;
        private readonly ObservableCollection<ScoreInfo> scores;

        public ObservableCollection<ScoreInfo> Scores
        {
            get { return scores; }
        }

        public MainWindow()
        {
            InitializeComponent();

            scores = new ObservableCollection<ScoreInfo>();
            interpreter = new Interpreter(tokeniser);
            scoreQueue = new ScoreQueue(feed, interpreter, null);

            QueuedScores.ItemsSource = scores;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            scoreQueue.Update();
            scores.Clear();

            foreach (var info in scoreQueue.Scores)
            {
                scores.Add(info);
            }
        }
    }
}
