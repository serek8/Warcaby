using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Warcaby.Controller;
using Warcaby.Model;
using Warcaby.Properties;

namespace Warcaby
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// Rodzaj nacisniecia myszki
        /// </summary>
        public enum Tap :int { Select = 0, Confirm = 1 };
        public enum Turn : int { Human = 0, Computer = 1 };
        int minMaxLevel;
        Board board;
        PictureBox[,] pawnImage;
        /// <summary>
        /// Po nacisnięciu myszy tutaj zapisuje jej wspolrzedne w zaleznosci od selectOrder
        /// </summary>
        Position selectedPosition;
        /// <summary>
        /// Zawiera kolejnosc klikniecia myszy,  0..1
        /// </summary>
        Tap nextClick;
        public FormMain()
        {
            InitializeComponent();
            board = new Board();
            drawPictureBoxesForPawns();
            selectedPosition = new Position();
            nextClick = Tap.Select;
        }
        Turn turn;
        private void drawPictureBoxesForPawns()
        {
            #region Add PictureBox for Pawns
            pawnImage = new PictureBox[8, 8];
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    pawnImage[x, y] = new PictureBox();
                    pawnImage[x, y].Location = new Point(board.pawnSize * x, board.pawnSize * y);
                    pawnImage[x, y].Width = board.pawnSize;
                    pawnImage[x, y].Height = board.pawnSize;
                    pawnImage[x, y].Click += new System.EventHandler(this.pictureBoxPawn_Click);
                    pawnImage[x, y].BackColor = Color.Transparent;
                    this.panelBoardBackground.Controls.Add(pawnImage[x, y]);
                }
            }
            #endregion
        }


        /// <summary>
        /// Kazde kliknięcie na bordzie jest odczytywane przez ta metode
        /// bo cały panel jest wypełniony obrazkami 
        /// </summary>
        private void pictureBoxPawn_Click(object sender, EventArgs e)
        {
            if (turn == Turn.Computer)
            {
                minMaxAlgorithm();
                //return;
            }
            
            Point point = panelBoardBackground.PointToClient(Cursor.Position);
            Position position = getPositionFromMouseCoordinates(point);
            /*labelPosition.Text = "X:" + position.x.ToString()
                               + "\n"
                               + "Y:"+position.y.ToString();*/
            // jeżeli zaznaczam
            
            if (nextClick == Tap.Select && board.isAnyPawnOnPosition(position))
            {
                //if (board.isAnyFightOnBoard() == true) MessageBox.Show("UWAGA przymusiwe bicie");
                // zapamietuje pozycje mojego zaznaczenia
                if (board.getPawnColourByItsPosition(position) == Pawn.Colours.Black) return;

                selectedPosition = position;

                enlargePawn(position);
                nextClick = Tap.Confirm;
                return;
            }
            else if (nextClick == Tap.Confirm) // Click.Confirm
            {
                // jezeli potwierdzi sie w białym polu
                if (!board.isPositionAllowed(position))
                {
                    zoomOutPawn(selectedPosition);
                    nextClick = Tap.Select;
                    //MessageBox.Show("biale pole");
                    return;
                }
                zoomOutPawn(selectedPosition);
                PositionFixer.saveCoordinatesToRafalFormat(board, "backup.txt");
                //MessageBox.Show( board.move(selectedPosition, position).ToString() );
                int moveResult = board.move(selectedPosition, position);
                nextClick = Tap.Select;
                refreshPawnsLayoutOnBoard();
                panelBoardBackground.Refresh();
                if (moveResult == 0) return;
                if (moveResult > 0)
                {
                    turn = Turn.Computer;
                  
                    
                }
                if (moveResult == 2 && board.shouldPawnOnPositionFight(position) == true)
                {
                    turn = Turn.Human;
                    if (board.shouldFightOption == false) ToolStripMenuItemEndTurn.Visible = true;
                    return;
                }
                
                if (board.isOver() == true)
                {
                    turn = Turn.Human;
                    MessageBox.Show("Koniec Gry");
                    return;
                }
                
                System.Threading.Thread.Sleep(100);
                minMaxAlgorithm();
            }
            
        }

        private void enlargePawn(Position position)
        {
            #region Powiększam pionki gdy są zaznaczone
            if (board.getPawnColourByItsPosition(position) == Pawn.Colours.White &&
                board.getPawnTypeByPosition(position) == Pawn.Type.Chequer)
                pawnImage[position.x, position.y].Image = Resources.whitePawnSelected;
            else if (board.getPawnColourByItsPosition(position) == Pawn.Colours.Black &&
                board.getPawnTypeByPosition(position) == Pawn.Type.Chequer)
                pawnImage[position.x, position.y].Image = Resources.blackPawnSelected;
            else if (board.getPawnColourByItsPosition(position) == Pawn.Colours.White &&
                board.getPawnTypeByPosition(position) == Pawn.Type.Queen)
                pawnImage[position.x, position.y].Image = Resources.whiteQueenSelected;
            else if (board.getPawnColourByItsPosition(position) == Pawn.Colours.Black &&
                  board.getPawnTypeByPosition(position) == Pawn.Type.Queen)
                pawnImage[position.x, position.y].Image = Resources.blackQueenSelected;
            #endregion
        }
        private void zoomOutPawn(Position position)
        {
            #region Pomniejzam pionki gdy są zaznaczone
            if (board.getPawnColourByItsPosition(position) == Pawn.Colours.White &&
                board.getPawnTypeByPosition(position) == Pawn.Type.Chequer)
                pawnImage[position.x, position.y].Image = Resources.whitePawn;
            else if (board.getPawnColourByItsPosition(position) == Pawn.Colours.Black &&
                board.getPawnTypeByPosition(position) == Pawn.Type.Chequer)
                pawnImage[position.x, position.y].Image = Resources.blackPawn;
            else if (board.getPawnColourByItsPosition(position) == Pawn.Colours.White &&
                board.getPawnTypeByPosition(position) == Pawn.Type.Queen)
                pawnImage[position.x, position.y].Image = Resources.whiteQueen;
            else if (board.getPawnColourByItsPosition(position) == Pawn.Colours.Black &&
                  board.getPawnTypeByPosition(position) == Pawn.Type.Chequer)
                pawnImage[position.x, position.y].Image = Resources.blackQueen;
            #endregion
        }

        #region RightMenu Click
        private void buttonNewGame_Click(object sender, EventArgs e)
        {
            newGame();
            // tu
            //board.currentLayout = PositionFixer.loadCoordinatesFromRafalAndSaveToLayout();
            //refreshPawnsLayoutOnBoard();
            //end tu
        }
        public void newGame()
        {
            board.newGame();
            board.currentLayout.setDefaultLayout();
            refreshPawnsLayoutOnBoard();
            turn = Turn.Human;
            View.FormLevelChoice level = new View.FormLevelChoice();
            level.ShowDialog();
            minMaxLevel =level.minMaxLevel;
            board.shouldFightOption = level.shouldFightOption;

            
            
        }
        #endregion
 
        /// <summary>
        /// Ustawia pionki zgodnie z ich pozycją
        /// </summary>
        public void refreshPawnsLayoutOnBoard()
        {
            clearBoard();
            List<Pawn> pawnList = board.getPawnList();
            //Przeszukuje liste wszystkich Pionków i ustawiam je na bordzie
            foreach (var item in pawnList)
            {
                pawnImage[item.position.x, item.position.y].Visible = true;
                //MessageBox.Show(item.type.ToString());
                if (item.colour == Pawn.Colours.White && item.type == Pawn.Type.Chequer)
                    pawnImage[item.position.x, item.position.y].Image = Resources.whitePawn;

                else if (item.colour == Pawn.Colours.Black && item.type == Pawn.Type.Chequer)
                    pawnImage[item.position.x, item.position.y].Image = Resources.blackPawn;

                else if (item.colour == Pawn.Colours.White && item.type == Pawn.Type.Queen)
                    pawnImage[item.position.x, item.position.y].Image = Resources.whiteQueen;

                else if (item.colour == Pawn.Colours.Black && item.type == Pawn.Type.Queen)
                    pawnImage[item.position.x, item.position.y].Image = Resources.blackQueen;

            }
        }

        public void clearBoard()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    pawnImage[x, y].Image = null;
                }
            }
        }


        public Position getPositionFromMouseCoordinates(Point point)
        {
            Position positonOnBoard = new Position();
            positonOnBoard.x = (int)(point.X / board.pawnSize);
            positonOnBoard.y = (int)(point.Y / board.pawnSize);
            return positonOnBoard;
        }


        private void buttonSaveBoard_Click(object sender, EventArgs e)
        {
            PositionFixer.saveCoordinatesToRafalFormat(this.board);
        }

        private void minMaxAlgorithm()
        {
            if (board.isOver() == true)
            {
                turn = Turn.Human;
                MessageBox.Show("Koniec Gry");
                return;
            }
            ToolStripMenuItemEndTurn.Visible = false;
            PositionFixer.saveCoordinatesToRafalFormat(this.board);
            Process proc = Process.Start("hideex.exe", "algorytm.exe plansza1.txt plansza2.txt " + minMaxLevel.ToString());
            System.Threading.Thread.Sleep(1100);
            board.currentLayout = PositionFixer.loadCoordinatesFromRafalAndSaveToLayout();
            turn = Turn.Human;
            
            refreshPawnsLayoutOnBoard();
            if (board.isOver() == true)
            {
                turn = Turn.Human;
                MessageBox.Show("Koniec Gry");
                return;
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            refreshPawnsLayoutOnBoard();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            board.currentLayout = PositionFixer.loadCoordinatesFromRafalAndSaveToLayout("backup.txt");
            refreshPawnsLayoutOnBoard();
        }

        private void nowaGraToolStripMenuItemNewGame_Click(object sender, EventArgs e)
        {
            newGame();
        }

        private void cofnijToolStripMenuItemBack_Click(object sender, EventArgs e)
        {
            board.currentLayout = PositionFixer.loadCoordinatesFromRafalAndSaveToLayout("backup.txt");
            refreshPawnsLayoutOnBoard();
        }

        private void ToolStripMenuItemEndTurn_Click(object sender, EventArgs e)
        {
            minMaxAlgorithm();
        }


    }
}
