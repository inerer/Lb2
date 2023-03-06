using Lb2.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lb2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString;
        private readonly SqlConnection _connection;
        readonly PlayerTeam _playerTeam;
        private Team _team;
        private readonly List<Team> _teams;
        private Player _player;
        private List<Player> _players;
        
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["FootballTeam"].ConnectionString;
            _connection = new SqlConnection(connectionString);
            _playerTeam= new PlayerTeam();
            _teams = new List<Team>();
            ComboBoxRendered();
        }
        
        private void ComboBoxRendered()
        {
            _connection.Open();
            string query = "select* from Teams";
            SqlCommand command = new SqlCommand(query, _connection);
            using (command)
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _team = new Team
                        {
                            TeamId = Convert.ToInt32(reader["Teamid"]),
                            Name = reader["Name"].ToString()
                        };
                        _teams.Add(_team);
                    }
                }
            }
            _connection.Close();
            FilterComboBox.ItemsSource = _teams;
        }

        private void RequestListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _players = new List<Player>();
            RequestListView.ItemsSource = null;
            var selectedItem = (Team)FilterComboBox.SelectedItem;
            _connection.Open();
            string query =
                $"select* from TeamsPlayers join Players on TeamsPlayers.PlayerId = Players.PlayerId join Teams on TeamsPlayers.Teamid = Teams.Teamid where TeamsPlayers.Teamid = {selectedItem.TeamId}";
            SqlCommand command = new SqlCommand(query, _connection);
            using (command)
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _player = new Player
                        {
                            Name = reader["PlayerName"].ToString(),
                            Age = Convert.ToInt32(reader["Age"])
                        };
                        _players.Add(_player);
                    }
                }
            }
            _connection.Close();
            RequestListView.ItemsSource = _players;
        }
    }
}
