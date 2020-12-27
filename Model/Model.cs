using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ZH
{
    public struct Pos
    {
        public int x;
        public int y;
        public Pos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    public class Model
    {
        int size;
        int[,] map;
        Pos selectedPlayer { get; set; }
        public int OnTurn { set; get; }
        public int Player1Fields{ set; get; }
        public int Player2Fields{ set; get; }
        public int PassesInARow { set; get; }
        
        public int Size { get { return size; } set { size = value; } }
        
        public EventHandler<int> generateTable;
        public EventHandler<int> GameEnded;
        public Model() {
            Size = 0;
            map = new int[0, 0];
        }


        public void StartGame(int e)
        {
            //e == méret
            Player1Fields = e;
            Player2Fields = e;
            Size = e;
            map = new int[e, e];
            for(int i = 0; i < e; ++i)
            {
                for(int j = 0; j < e; ++j)
                {
                    map[i, j] = 0;
                }
            }

            //bábuk beállítása
            for(int i = 0; i < e; ++i)
            {
                map[0, i] = 1;
                map[e - 1, i] = 2;
            }
            selectedPlayer = new Pos(-1, -1);
            OnTurn = 1;
            OnGenerateTable(e);
            PassesInARow = 0;
        }

        private void OnGenerateTable(int e)
        {
            if (generateTable != null)
            {
                generateTable(this, e);
            }
        }

        public void Pass()
        {
            OnTurn = OnTurn == 1 ? 2 : 1;
            PassesInARow++;
            if(PassesInARow == 2)
            {
                OnGameEnded(3);
            }
        }

        public int getMapElem(int i, int j)
        {
            return map[i, j];
        }

        public void Step(int x, int y)
        {
            if (selectedPlayer.x == -1 && selectedPlayer.y == -1)
            {
                if(map[x, y] == OnTurn)
                {
                    selectedPlayer = new Pos(x, y);
                    map[x, y] = 3;
                }
                else
                {
                    MessageBox.Show("Can't select the opponent's player or blank field!");
                }
            }
            else
            {
                if((Math.Abs(selectedPlayer.x - x) == 2 && Math.Abs(selectedPlayer.y - y) == 2) ||
                    (Math.Abs(selectedPlayer.x - x) == 1 && Math.Abs(selectedPlayer.y - y) == 1))
                {
                    //case valid move
                    if(map[x, y] == 0)
                    {
                        map[selectedPlayer.x, selectedPlayer.y] = 0;
                        map[x, y] = OnTurn;
                        OnTurn = OnTurn == 1 ? 2 : 1; 
                        selectedPlayer = new Pos(-1, -1);
                        PassesInARow = 0;
                    }
                    else
                    {
                        MessageBox.Show("Invalid move!");
                    }
                }
                else
                {
                    if((Math.Abs(selectedPlayer.x - x) == 1 && Math.Abs(selectedPlayer.y - y) == 0) ||
                        (Math.Abs(selectedPlayer.x - x) == 0 && Math.Abs(selectedPlayer.y - y) == 1))
                    {
                        int opponent = OnTurn == 1 ? 2 : 1;
                        if(map[x, y] == opponent)
                        {
                            map[selectedPlayer.x, selectedPlayer.y] = 0;
                            map[x, y] = OnTurn;
                            if(OnTurn == 1)
                            {
                                Player2Fields -= 1;
                            }
                            else
                            {
                                Player1Fields -= 1;
                            }
                            OnTurn = OnTurn == 1 ? 2 : 1;
                            selectedPlayer = new Pos(-1, -1);
                            PassesInARow = 0;
                        }
                        else
                        {
                            MessageBox.Show("You can't hit that field!");
                        }
                    }
                }
            }

            if(Player2Fields == 0)
            {
                OnGameEnded(1);
            }
            else if(Player1Fields == 0)
            {
                OnGameEnded(2);
            }
        }

        private void OnGameEnded(int winner)
        {
            if(GameEnded != null)
            {
                if(winner == 1)
                {
                    GameEnded(this, 1);
                }
                else if(winner == 2)
                {
                    GameEnded(this, 2);
                }
                else
                {
                    GameEnded(this, 3);
                }
            }
        }
    }
}
