using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace DotaHelper.View
{
    public partial class MainWindow : Window
    {
        private DateTime globalTimerStart;
        private DispatcherTimer globalTimer;
        private DispatcherTimer firstTimer;
        private DispatcherTimer secondTimer;
        private DispatcherTimer thirdTimer;
        private DispatcherTimer fourthTimer;
        private DispatcherTimer fifthTimer;
        private MediaPlayer mediaPlayer;
        private bool isAnnouncementPlaying;
        private Queue<int> announcementQueue;
        private TimeSpan pauseDuration = TimeSpan.Zero;
        private TimeSpan pauseStart = TimeSpan.Zero;
        private int firstTimerInterval = 3 * 60; 
        private int secondTimerDelay = 3 * 60; 
        private int secondTimerInterval = 3 * 60; 
        private int thirdTimerInterval = 2 * 60; 
        private int thirdTimerRepeatCount = 2;
        private int fourthTimerInterval = 2 * 60; 
        private int fourthTimerDelay = 6 * 60; 
        private int fifthTimerInterval = 7 * 60; 
        private int fifthTimerDelay = 7 * 60; 
        private bool isPaused = false;
        private int TimerFlag;
        ConnectionDB database = new ConnectionDB();
        private int selectedRow;
        private string heroname = "";
        public MainWindow()
        {
            InitializeComponent();
            mediaPlayer = new MediaPlayer();
            isAnnouncementPlaying = false;
            announcementQueue = new Queue<int>();
            PopulateComboBoxWithHeroVariants();
            //InitializeComboBoxEventHandlers();
        }
        public string Login { get; set; }
        public void ShowLogin()
        {
            YouAreNotAuthorised.Text = "";
            YouAreNotAuthorised.Text = Login;

            NotAuthorised.Visibility = Visibility.Hidden;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void NotAuthorised_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginView view = new LoginView();
            this.Close();
            view.ShowDialog();
        }
        private void PopulateComboBoxWithHeroVariants()
        {
            
            List<int> variantHeroIDs = new List<int> { 4, 6, 20, 22, 29, 52, 88, 91, 98, 104 };
            List<ComboBox> comboBoxes = new List<ComboBox> { HeroVariants1, HeroVariants2, HeroVariants3, HeroVariants4, HeroVariants5, HeroVariants6, HeroVariants7, HeroVariants8, HeroVariants9, HeroVariants10 };

            foreach (ComboBox comboBox in comboBoxes)
            {
                foreach (int heroID in variantHeroIDs)
                {
                    string heroName = GetHeroNameByID(heroID);
                    comboBox.Items.Add(heroName);
                }
            }
        }
        private string GetHeroNameByID(int heroID)
        {
            string heroName = string.Empty;
            using (SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-4R5RIHS\SQLSERVER;Initial Catalog=DotaHelper;Integrated Security=True"))
            {
                sqlConnection.Open();
                string sql = $"SELECT hero_name FROM heroes WHERE id = {heroID}";
                SqlCommand command = new SqlCommand(sql, sqlConnection);
                heroName = command.ExecuteScalar()?.ToString();
            }
            return heroName;
        }
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (radioButton != null)
            {
                ComboBox comboBox = radioButton.Tag as ComboBox;

                if (comboBox != null)
                {
                    heroname = comboBox.SelectedItem?.ToString();
                    if (heroname == "Anti-Mage") heroname = "AntiMage";
                    if (heroname == "Storm Spirit") heroname = "StormSpirit";
                    if (heroname == "Shadow Fiend") heroname = "ShadowFiend";
                    if (heroname == "Dark Willow") heroname = "DarkWillow";
                    if (heroname == "Drow Ranger") heroname = "DrowRanger";
                    if (heroname == "Skywrath Mage") heroname = "SkywrathMage";
                    if (heroname == "Crystal Maiden") heroname = "CrystalMaiden";
                }
            }
        }
        private void btnResetDraft_Click(object sender, RoutedEventArgs e)
        {
            HeroesTable.ItemsSource = null;
            List<ComboBox> comboBoxes = new List<ComboBox> { HeroVariants1, HeroVariants2, HeroVariants3, HeroVariants4, HeroVariants5, HeroVariants6, HeroVariants7, HeroVariants8, HeroVariants9, HeroVariants10 };
            foreach (ComboBox comboBox in comboBoxes)
            {
                comboBox.SelectedItem = null;
            }
            MyHero1.IsChecked = false;
            MyHero2.IsChecked = false;
            MyHero3.IsChecked = false;
            MyHero4.IsChecked = false;
            MyHero5.IsChecked = false;
            btnResetDraft.IsEnabled = false;
        }
        private void btnConfirmDraft_Click(object sender, RoutedEventArgs e)
        {
            

            btnResetDraft.IsEnabled = true;
            if (heroname != "")
            {
                List<string> chosenVariants = new List<string>();

                // Iterate over the ComboBoxes and add the selected values to the list
                List<ComboBox> comboBoxes = new List<ComboBox> { HeroVariants1, HeroVariants2, HeroVariants3, HeroVariants4, HeroVariants5, HeroVariants6, HeroVariants7, HeroVariants8, HeroVariants9, HeroVariants10 };
                foreach (ComboBox comboBox in comboBoxes)
                {
                    string selectedVariant = comboBox.SelectedItem?.ToString();
                    if (!string.IsNullOrEmpty(selectedVariant) && selectedVariant != "-1")
                        chosenVariants.Add(selectedVariant);
                }
                // Check if any duplicates exist in the chosen variants
                if ((chosenVariants.Count != chosenVariants.Distinct().Count())/* || (chosenVariants.Count < 10)*/)
                {
                    MessageBox.Show("Некорректно составлен набор команд", "Ошибка");
                    return;
                }
                else DataDisplay(heroname);
            }
            else MessageBox.Show("Выберите своего героя!", "Ошибка!");

        }
        private void DataDisplay(string heroname)
        {

            
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable heroTable = new DataTable();
            string sql = $"SELECT BadAgainst_HeroName, BadAgainst_Reason, Advise, GoodAgainst_HeroName, GoodAgainst_Reason FROM {heroname}";
            SqlCommand sqlCommand = new SqlCommand(sql, database.GetConnection());
            adapter.SelectCommand = sqlCommand;
            adapter.Fill(heroTable);
            HeroesTable.ItemsSource = heroTable.DefaultView;


        }
        private async void btnStartTimer1_Click(object sender, RoutedEventArgs e)
        {
            TimerFlag = 1;
            btnStartTimer1.IsEnabled = false;
            await Task.Delay(4000);
            globalTimerStart = DateTime.Now;
            globalTimer = new DispatcherTimer();
            globalTimer.Tick += GlobalTimer_Tick;
            globalTimer.Interval = TimeSpan.FromMilliseconds(10);
            roshanTimerText.Text = "";
            setTimerRoshan.Text = "";
            btnResetTimer.IsEnabled = true;
            btnStopTimers.IsEnabled = true;
            globalTimer.Start();
            StartTimers();

        }
        private void GlobalTimer_Tick(object sender, EventArgs e)
        {
            if (!isPaused)
            {
                TimeSpan timeElapsed = DateTime.Now - globalTimerStart - pauseDuration;
                string formattedTime = $"{timeElapsed.Minutes:D2}:{timeElapsed.Seconds:D2}";
                UpdateRoshanStatus();
                TimeSpan targetTime = new TimeSpan(0, 20, 0);
                int comparisonResult = timeElapsed.CompareTo(targetTime);
            }
        }
        private void btnStartTimerRoshan_Click(object sender, RoutedEventArgs e)
        {

            UpdateRoshanStatus();
        }
        private void UpdateRoshanStatus()
        {

            if (DateTime.TryParseExact(setTimerRoshan.Text, "mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime roshanTime))
            {
                TimeSpan roshanDifference = DateTime.Now - globalTimerStart - roshanTime.TimeOfDay;

                if (roshanDifference.TotalSeconds > 0)
                {
                    if (roshanDifference.TotalSeconds <= 8 * 60)
                    {
                        RoshanStatus.Text = "Мертв";
                        RoshanStatus.Foreground = Brushes.Red;
                        RoshanStatus.FontSize = 25;
                    }
                    else if (roshanDifference.TotalSeconds >= 8 * 60 && roshanDifference.TotalSeconds <= 11 * 60)
                    {
                        RoshanStatus.Text = "Возможно жив";
                        RoshanStatus.Foreground = Brushes.Orange;
                        RoshanStatus.FontSize = 25;

                    }
                    else
                    {
                        RoshanStatus.Text = "Жив";
                        RoshanStatus.Foreground = Brushes.Green;
                        RoshanStatus.FontSize = 25;

                    }
                }
                else
                {
                    RoshanStatus.Text = "Неверное время";
                    RoshanStatus.FontSize = 20;
                }
            }
            else
            {
                RoshanStatus.Text = "";
                RoshanStatus.Foreground = Brushes.Black;
            }
        }
        private void setTimerRoshan_TextChanged(object sender, TextChangedEventArgs e)
        {
            string startTime = setTimerRoshan.Text;

            if (!string.IsNullOrEmpty(startTime) && startTime.Length == 5)
            {
                if (DateTime.TryParseExact(startTime, "mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTimeDateTime))
                {
                    TimeSpan startTimeSpan = startTimeDateTime.TimeOfDay;
                    TimeSpan endTimeSpan = startTimeSpan.Add(TimeSpan.FromMinutes(8));
                    TimeSpan possiblyEndTimeSpan = startTimeSpan.Add(TimeSpan.FromMinutes(11));

                    string endTimeFormatted = endTimeSpan.ToString(@"mm\:ss");
                    string possiblyEndTimeFormatted = possiblyEndTimeSpan.ToString(@"mm\:ss");

                    string timePeriod = $"{endTimeFormatted}-{possiblyEndTimeFormatted}";

                    roshanTimerText.Text = timePeriod;
                }
                else
                {
                    roshanTimerText.Text = "Неверные данные";
                }
            }
            else
            {
                roshanTimerText.Text = "";
            }
        }
        private void setTimerTormentor_TextChanged(object sender, TextChangedEventArgs e)
        {
            string startTime = setTimerTormentor.Text;

            if (!string.IsNullOrEmpty(startTime) && startTime.Length == 5)
            {
                if (DateTime.TryParseExact(startTime, "mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTimeDateTime))
                {
                    TimeSpan startTimeSpan = startTimeDateTime.TimeOfDay;
                    TimeSpan endTimeSpan = startTimeSpan.Add(TimeSpan.FromMinutes(10));

                    string endTimeFormatted = endTimeSpan.ToString(@"mm\:ss");

                    string timePeriod = $"{endTimeFormatted}";

                    tormentorTimerText.Text = timePeriod;
                }
                else
                {
                    tormentorTimerText.Text = "Неверные данные";
                }
            }
            else
            {
                tormentorTimerText.Text = "";
            }
        }
        private void StartTimers()
        {
            firstTimer = new DispatcherTimer();
            firstTimer.Interval = TimeSpan.FromSeconds(1);
            firstTimer.Tick += FirstTimer_Tick;
            firstTimer.Start();

            secondTimer = new DispatcherTimer();
            secondTimer.Interval = TimeSpan.FromSeconds(1);
            secondTimer.Tick += SecondTimer_Tick;
            secondTimer.Start();

            thirdTimer = new DispatcherTimer();
            thirdTimer.Interval = TimeSpan.FromSeconds(1);
            thirdTimer.Tick += ThirdTimer_Tick;
            thirdTimer.Start();

            fourthTimer = new DispatcherTimer();
            fourthTimer.Interval = TimeSpan.FromSeconds(1);
            fourthTimer.Tick += FourthTimer_Tick;
            fourthTimer.Start();

            fifthTimer = new DispatcherTimer();
            fifthTimer.Interval = TimeSpan.FromSeconds(1);
            fifthTimer.Tick += FifthTimer_Tick;
            fifthTimer.Start();
        }
        private void FirstTimer_Tick(object sender, EventArgs e)
        {
            firstTimerInterval--;
            FirstTimerText.Text = TimeSpan.FromSeconds(firstTimerInterval).ToString(@"mm\:ss");
            if (firstTimerInterval == 60)
            {
                AnnouncementPlay(1);
            }
            if (firstTimerInterval <= 0)
            {
                firstTimerInterval = 3 * 60;
            }
        }
        private void SecondTimer_Tick(object sender, EventArgs e)
        {
            secondTimerDelay--;
            if (secondTimerDelay <= 0)
            {
                SecondTimerText.Text = TimeSpan.FromSeconds(secondTimerInterval).ToString(@"mm\:ss");
                secondTimerInterval--;
                if (secondTimerInterval == 60)
                {
                    AnnouncementPlay(2);
                }
                if (secondTimerInterval <= 0)
                {
                    secondTimerInterval = 3 * 60;
                }
            }
        }
        private void ThirdTimer_Tick(object sender, EventArgs e)
        {
            if (thirdTimerRepeatCount > 0)
            {
                thirdTimerInterval--;
                ThirdTimerText.Text = TimeSpan.FromSeconds(thirdTimerInterval).ToString(@"mm\:ss");

                if (thirdTimerInterval == 60)
                {
                    AnnouncementPlay(3);
                }
                if (thirdTimerInterval <= 0)
                {
                    thirdTimerInterval = 2 * 60;
                    thirdTimerRepeatCount--;
                }
            }
            else
            {
                ThirdTimerText.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }
        private void FourthTimer_Tick(object sender, EventArgs e)
        {
            fourthTimerDelay--;
            if (fourthTimerDelay <= 0)
            {

                FourthTimerText.Text = TimeSpan.FromSeconds(fourthTimerInterval).ToString(@"mm\:ss");
                fourthTimerInterval--;
                if (fourthTimerInterval == 60)
                {
                    AnnouncementPlay(4);
                }
                if (fourthTimerInterval <= 0)
                {
                    fourthTimerInterval = 2 * 60;
                }
            }
        }
        private void FifthTimer_Tick(object sender, EventArgs e)
        {
            fifthTimerDelay--;
            if (fifthTimerDelay <= 0)
            {

                FifthTimerText.Text = TimeSpan.FromSeconds(fifthTimerInterval).ToString(@"mm\:ss");
                fifthTimerInterval--;
                if (fifthTimerInterval == 60)
                {
                    AnnouncementPlay(5);
                }
                if (fifthTimerInterval <= 0)
                {
                    fifthTimerInterval = 7 * 60;
                }
            }
        }
        private void AnnouncementPlay(int timernum)
        {
            announcementQueue.Enqueue(timernum);

            if (isAnnouncementPlaying)
            {
                return;
            }

            PlayNextAnnouncement();
        }
        private void PlayNextAnnouncement()
        {
            if (announcementQueue.Count == 0)
            {
                isAnnouncementPlaying = false;
                return;
            }

            int nextAnnouncement = announcementQueue.Dequeue();
            isAnnouncementPlaying = true;

            string filePath;
            switch (nextAnnouncement)
            {
                case 1:
                    filePath = "C:\\Users\\mkrch\\source\\repos\\DotaHelper\\Voices\\BountyRuneVoice.mp3";
                    break;
                case 2:
                    filePath = "C:\\Users\\mkrch\\source\\repos\\DotaHelper\\Voices\\LotosVoice.mp3";
                    break;
                case 3:
                    filePath = "C:\\Users\\mkrch\\source\\repos\\DotaHelper\\Voices\\WaterRune.mp3";
                    break;
                case 4:
                    filePath = "C:\\Users\\mkrch\\source\\repos\\DotaHelper\\Voices\\PowerRuneVoice.mp3";
                    break;
                case 5:
                    filePath = "C:\\Users\\mkrch\\source\\repos\\DotaHelper\\Voices\\WisdomRuneVoice.mp3";
                    break;
                default:
                    return;
            }

            try
            {
                mediaPlayer.Volume = 0.1;
                mediaPlayer.Open(new Uri(filePath));
                mediaPlayer.Play();
                mediaPlayer.MediaEnded += (sender, e) =>
                {
                    // Reset the flag when the announcement finishes playing
                    isAnnouncementPlaying = false;

                    // Play the next announcement
                    PlayNextAnnouncement();
                };
            }
            catch (Exception ex)
            {
                // Reset the flag in case of an error and move to the next announcement
                isAnnouncementPlaying = false;
                PlayNextAnnouncement();
            }
        }
        private void btnResetTimer_Click(object sender, RoutedEventArgs e)
        {
            TimerFlag = 0;
            ResetTimer(firstTimer, ref firstTimerInterval, "00:00", FirstTimerText);
            ResetTimer(secondTimer, ref secondTimerInterval, "00:00", SecondTimerText);
            ResetTimer(thirdTimer, ref thirdTimerInterval, "00:00", ThirdTimerText);
            ResetTimer(fourthTimer, ref fourthTimerInterval, "00:00", FourthTimerText);
            ResetTimer(fifthTimer, ref fifthTimerInterval, "00:00", FifthTimerText);
            thirdTimerRepeatCount = 2;
            globalTimer.Stop();
            btnStopTimers.IsEnabled = false;
            btnStartTimer1.IsEnabled = true;
            btnResetTimer.IsEnabled = false;
        }
        private void ResetTimer(DispatcherTimer timer, ref int interval, string displayText, TextBlock textBlock)
        {
            timer.Stop();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.IsEnabled = false;
            textBlock.Text = displayText;
            interval = GetInitialInterval(timer);
            roshanTimerText.Text = "";
            setTimerRoshan.Text = "";
        }
        private int GetInitialInterval(DispatcherTimer timer)
        {
            if (timer == firstTimer)
            {
                return 3 * 60;
            }
            else if (timer == secondTimer)
            {
                return 3 * 60; 
            }
            else if (timer == thirdTimer)
            {
                return 2 * 60; 
            }
            else if (timer == fourthTimer)
            {
                return 2 * 60;
            }
            else if (timer == fifthTimer)
            {
                return 7 * 60;
            }

            return 0;
        }
        private void btnStopTimers_Click(object sender, RoutedEventArgs e)
        {
            if (isPaused)
            {
                TimeSpan pauseEnd = DateTime.Now.TimeOfDay;
                pauseDuration += pauseEnd - pauseStart;
                isPaused = false;
                btnStopTimers.Content = "Пауза";
                globalTimer.Start();
                firstTimer.Start();
                secondTimer.Start();
                thirdTimer.Start();
                fourthTimer.Start();
                fifthTimer.Start();
            }
            else
            {
                pauseStart = DateTime.Now.TimeOfDay;
                isPaused = true;
                btnStopTimers.Content = "Продолжить";
                globalTimer.Stop();
                firstTimer.Stop();
                secondTimer.Stop();
                thirdTimer.Stop();
                fourthTimer.Stop();
                fifthTimer.Stop();
            }
        }
        private void More_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            AboutTimers aboutTimers = new AboutTimers();
            aboutTimers.ShowDialog();
            aboutTimers.Focus();
        }

    }
}