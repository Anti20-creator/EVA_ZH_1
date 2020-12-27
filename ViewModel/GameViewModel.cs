using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace ZH.ViewModel
{
    class GameViewModel : ViewModelBase
    {
        public ObservableCollection<Field> Fields { get; set; }
        Model _model;
        public DelegateCommand StartGameSmall { get; private set; }
        public DelegateCommand StartGameMedium { get; private set; }
        public DelegateCommand StartGameLarge { get; private set; }
        public DelegateCommand Pass { get; private set; }
        public int BoardSize { get { return _model.Size; } }

        public EventHandler<int> StartGame;

        public string OnTurn { get{ return Fields.Count == 0 ? "None" : _model.OnTurn == 1 ? "Grey" : "Black"; } set { OnTurn = value; } }
        public int PlayersLeft { get { return _model.OnTurn == 1 ? _model.Player1Fields : _model.Player2Fields; } set { PlayersLeft = value; } }

        public GameViewModel(Model m)
        {
            _model = m;
            Fields = new ObservableCollection<Field>();
            StartGameSmall = new DelegateCommand(
                (_) => OnStartGame(4)
            );
            StartGameMedium = new DelegateCommand(
                (_) => OnStartGame(6)
            );
            StartGameLarge = new DelegateCommand(
                (_) => OnStartGame(8)
            );
            _model.generateTable += new EventHandler<int>(GenerateTable);
            Pass = new DelegateCommand(
                (_) => Skip()
            );
        }

        private void Skip()
        {
            _model.Pass();
            OnPropertyChanged("OnTurn");
            OnPropertyChanged("PlayerLeft");
        }

        private void OnStartGame(int e)
        {
            if (StartGame != null)
                StartGame(this, e);
        }

        private void GenerateTable(object sender, int e)
        {
            Fields.Clear();
            for (Int32 i = 0; i < e; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < e; j++)
                {
                    Fields.Add(new Field
                    {
                        Text = String.Empty,
                        Color = _model.getMapElem(i, j) == 0 ? "White" : _model.getMapElem(i, j) == 1 ? "Gray" : _model .getMapElem(i, j) == 2 ? "Black" : "Green",
                        X = i,
                        Y = j,
                        Number = i * _model.Size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields"); 
            OnPropertyChanged("PlayersLeft");
            OnPropertyChanged("OnTurn");
        }

        private void RefreshTable() 
        {
            int e = _model.Size;
            Fields.Clear();
            for (Int32 i = 0; i < e; i++) // inicializáljuk a mezőket
            {
                for (Int32 j = 0; j < e; j++)
                {
                    Fields.Add(new Field
                    {
                        Text = String.Empty,
                        Color = _model.getMapElem(i, j) == 0 ? "White" : _model.getMapElem(i, j) == 1 ? "Gray" : _model.getMapElem(i, j) == 2 ? "Black" : "Green",
                        X = i,
                        Y = j,
                        Number = i * _model.Size + j,
                        StepCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
            OnPropertyChanged("Fields");
            OnPropertyChanged("PlayersLeft");
            OnPropertyChanged("OnTurn");
        }

        private void StepGame(Int32 index)
        {
            Field field = Fields[index];

            _model.Step(field.X, field.Y);
            RefreshTable();

            OnPropertyChanged("Fields");
            
        }

    }
}
