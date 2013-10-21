using System.Collections.ObjectModel;
using System.Windows;
using DrumScore.Interpretation;
using DrumScore.ScoreSourcing;

namespace DrumScore.UI
{
    public partial class MainWindow
    {
        private readonly ScoreQueue scoreQueue;
        private readonly ObservableCollection<ScoreInfo> scores;

        public ObservableCollection<ScoreInfo> Scores
        {
            get { return scores; }
        }

        public MainWindow()
        {
            InitializeComponent();

            scores = new ObservableCollection<ScoreInfo>();
            scoreQueue = new ScoreQueue(new TwitterScoreFeed(), new Interpreter(new Tokeniser()), new Notifications());

            PlayQueue.ItemsSource = scores;
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
