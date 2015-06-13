using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warcaby.Model;

namespace Warcaby.Controller
{
    public class Board
    {
        public Layout currentLayout;
        public int boardSize;
        public int pawnSize;
        public bool shouldFightOption;
        

        public Board()
        {
            boardSize = 600;
            pawnSize = 600/8; //75
            shouldFightOption = false;
            newGame();
        }

        public void newGame()
        {
            currentLayout = new Layout();
        }

        /// <summary>
        /// Zwraca liste pionków na planszy
        /// </summary>
        public List<Pawn> getPawnList()
        {
            return currentLayout.PawnList;
        }

        #region Ruch
        /// <summary>
        /// Sprawdza ruch użytkownika, który ten chce wykonać
        /// i nasteęonie decyduje czy to bedzie chodzenie czy bitwa 
        /// i woluje do tego odpowiednią funkcję
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <returns>0 - Wybrany ruch jest nieprawidlowy</returns>
        /// <returns>1 - Pionek zostal ruszony</returns>
        /// <returns>2 - Pionek został zbity</returns>
        public int move(Position fromPosition, Position toPosition)
        {
            // sprawdzam czy pionek mial przymusowe bicie
            //if(isAnyFightForColourOnBoard(getPawnColourByItsPosition(fromPosition)) == false)
            //if(isAnyFightOnBoard() == false && shouldPawnOnPositionFight(fromPosition) == false)
            bool czy = isAnyFightForColourOnBoard(getPawnColourByItsPosition(fromPosition));
            #region Ruch pionka
            if (getPawnTypeByPosition(fromPosition) == Pawn.Type.Chequer)
            {
                if (czy == false || !shouldFightOption)
                {
                    // Jezeli mamy podejsc o jedno pole
                    if (fromPosition.x - toPosition.x == 1 || fromPosition.x - toPosition.x == -1)
                    {
                        if (fromPosition.y - toPosition.y == 1 || fromPosition.y - toPosition.y == -1)
                        {
                            if (walk(fromPosition, toPosition) == true)
                            {
                                // zamieniam na dame na koncu planszy
                                if (toPosition.y == 0 || toPosition.y == 7)
                                {
                                    upgradePawnByPosition(toPosition); //sprawdzam wewnatrz funckji czy na pewno pionek jest odpowiedniego koloru
                                }
                                return 1;
                            }
                            else return 0;
                            //return walk(fromPosition, toPosition);
                        }
                    }
                }
                // Jezeli mamy bić
                if (fromPosition.x - toPosition.x == 2 || fromPosition.x - toPosition.x == -2)
                {
                    if (fromPosition.y - toPosition.y == 2 || fromPosition.y - toPosition.y == -2)
                    {
                        //return fight(fromPosition, toPosition);
                        if (fight(fromPosition, toPosition) == true) return 2;
                        else return 0;

                    }
                }
            }
            #endregion
            #region Ruch damki
                // damka
            else if (getPawnTypeByPosition(fromPosition) == Pawn.Type.Queen)
            {
                
                if (isAnyPawnOnPosition(toPosition) == true) return 0;
                int rateX = 0, rateY = 0;
                Position fromPositionTmp = new Position(fromPosition.x, fromPosition.y);
                Position fromPositionTmpWithRate = new Position(fromPosition.x, fromPosition.y);

                //szukam wspolczynnika chodzenia
                if (fromPosition.x < toPosition.x && fromPosition.y > toPosition.y) // cw I
                { rateX = 1; rateY = -1; }
                else if(fromPosition.x > toPosition.x && fromPosition.y > toPosition.y) // cw II
                { rateX = -1; rateY = -1; }
                else if (fromPosition.x > toPosition.x && fromPosition.y < toPosition.y) // cw III
                { rateX = -1; rateY = 1; }
                else if (fromPosition.x < toPosition.x && fromPosition.y < toPosition.y) // cw IV
                { rateX = 1; rateY = 1; }
                bool wasFight = false;

                    while (true)
                    {
                        if (fromPositionTmp.x == toPosition.x && fromPositionTmp.y == toPosition.y)
                        {
                            if (wasFight == false) return 1;
                            else return 2;
                        }
                        fromPositionTmpWithRate.x = fromPositionTmp.x + rateX;
                        fromPositionTmpWithRate.y = fromPositionTmp.y + rateY;
                        if (walkQueen(fromPositionTmp, fromPositionTmpWithRate) == true)
                        {
                            fromPositionTmp.x = fromPositionTmpWithRate.x;
                            fromPositionTmp.y = fromPositionTmpWithRate.y;
                            continue;
                        }
                        fromPositionTmpWithRate.x = fromPositionTmp.x + 2 * rateX;
                        fromPositionTmpWithRate.y = fromPositionTmp.y + 2 * rateY;
                        if (fight(fromPositionTmp, fromPositionTmpWithRate) == true)
                        {
                            fromPositionTmp.x = fromPositionTmpWithRate.x;
                            fromPositionTmp.y = fromPositionTmpWithRate.y;
                            wasFight = true;
                            continue;
                        }
                        else return 0;

                    }

            }
#endregion
            return 0;
        }
        #endregion

        #region Bicie na planszy
        public bool isAnyFightOnBoard()
        {
            foreach (var item in getPawnList())
            {
                if (item.type==Pawn.Type.Chequer && shouldPawnOnPositionFight(item.position) == true) return true;
                if (item.type == Pawn.Type.Queen && shouldQueenOnPositionFight(item.position) == true) return true;
                
            }
            return false;
        }

        public bool isAnyFightForColourOnBoard(Pawn.Colours colour)
        {
            foreach (var item in getPawnList())
            {
                bool czy = shouldPawnOnPositionFight(item.position);
                if (item.type == Pawn.Type.Chequer && 
                    czy == true &&
                    item.colour == colour) return true;

                if (item.type == Pawn.Type.Queen 
                    && shouldQueenOnPositionFight(item.position) == true &&
                    item.colour == colour) return true;

            }
            return false;
        }


        public bool shouldPawnOnPositionFight(Position position)
        {
            //if (shouldFightOption == false) return false;
            if (getPawnTypeByPosition(position) == Pawn.Type.Queen)
            {
                return shouldQueenOnPositionFight(position);    
            }
            Pawn.Colours thisColour = getPawnColourByItsPosition(position);
            Position positionTmp = new Position(position.x, position.y);
            positionTmp.x = position.x + 1;
            positionTmp.y = position.y + 1;
            if (isPositionAllowed(positionTmp) == true)
                if (isAnyPawnOnPosition(positionTmp) == true)
                    if (getPawnColourByItsPosition(positionTmp) != thisColour) // kolory sa inne
                        if (isPositionAllowed(new Position(position.x + 2, position.y + 2)) == true)
                            if (isAnyPawnOnPosition(new Position(position.x + 2, position.y +2)) == false)
                                 return true; // musi walczyc

            positionTmp.x = position.x - 1;
            positionTmp.y = position.y + 1;
            if (isPositionAllowed(positionTmp) == true)
                if (isAnyPawnOnPosition(positionTmp) == true)
                 if (getPawnColourByItsPosition(positionTmp) != thisColour) // kolory sa inne
                     if (isPositionAllowed(new Position(position.x - 2, position.y + 2)) == true)
                        if (isAnyPawnOnPosition(new Position(position.x - 2, position.y + 2)) == false)
                    return true; // musi walczyc

            positionTmp.x = position.x + 1;
            positionTmp.y = position.y - 1;
            if (isPositionAllowed(positionTmp) == true)
                if (isAnyPawnOnPosition(positionTmp) == true)
                if (getPawnColourByItsPosition(positionTmp) != thisColour) // kolory sa inne
                    if (isPositionAllowed(new Position(position.x + 2, position.y - 2)) == true)
                        if (isAnyPawnOnPosition(new Position(position.x + 2, position.y - 2)) == false)
                    return true; // musi walczyc

            positionTmp.x = position.x - 1;
            positionTmp.y = position.y - 1;
            if (isPositionAllowed(positionTmp) == true)
                if (isAnyPawnOnPosition(positionTmp) == true)
                if (getPawnColourByItsPosition(positionTmp) != thisColour) // kolory sa inne
                    if (isPositionAllowed(new Position(position.x - 2, position.y - 2)) == true)
                        if (isAnyPawnOnPosition(new Position(position.x - 2, position.y - 2)) == false)
                             return true; // musi walczyc

            return false;
        }


        public bool shouldQueenOnPositionFight(Position position)
        {
            //if (shouldFightOption == false) return false;
            for (int k = 1; k < 8; k++)
            {
                Pawn.Colours thisColour = getPawnColourByItsPosition(position);
                Position positionTmp = new Position(position.x, position.y);
                positionTmp.x = position.x + k;
                positionTmp.y = position.y + k;
                if (isPositionAllowed(positionTmp) == true)
                    if (isAnyPawnOnPosition(positionTmp) == true)
                        if (getPawnColourByItsPosition(positionTmp) != thisColour) // kolory sa inne
                        {
                            positionTmp.x = position.x + k+1;
                            positionTmp.y = position.y + k+1;
                            if (isPositionAllowed(positionTmp) == true)
                                if (isAnyPawnOnPosition(positionTmp) == false)
                                      return true; // musi walczyc
                        }

                positionTmp.x = position.x - k;
                positionTmp.y = position.y + k;
                if (isPositionAllowed(positionTmp) == true)
                    if (isAnyPawnOnPosition(positionTmp) == true)
                        if (getPawnColourByItsPosition(positionTmp) != thisColour) // kolory sa inne
                        {
                            positionTmp.x = position.x - k - 1;
                            positionTmp.y = position.y + k + 1;
                            if (isPositionAllowed(positionTmp) == true)
                                if (isAnyPawnOnPosition(positionTmp) == false)
                                    return true; // musi walczyc
                        }

                positionTmp.x = position.x + k;
                positionTmp.y = position.y - k;
                if (isPositionAllowed(positionTmp) == true)
                    if (isAnyPawnOnPosition(positionTmp) == true)
                        if (getPawnColourByItsPosition(positionTmp) != thisColour) // kolory sa inne
                        {
                            positionTmp.x = position.x + k + 1;
                            positionTmp.y = position.y - k - 1;
                            if (isPositionAllowed(positionTmp) == true)
                                if (isAnyPawnOnPosition(positionTmp) == false)
                                    return true; // musi walczyc
                        }

                positionTmp.x = position.x - k;
                positionTmp.y = position.y - k;
                if (isPositionAllowed(positionTmp) == true)
                    if (isAnyPawnOnPosition(positionTmp) == true)
                        if (getPawnColourByItsPosition(positionTmp) != thisColour) // kolory sa inne
                        {
                            positionTmp.x = position.x - k - 1;
                            positionTmp.y = position.y - k - 1;
                            if (isPositionAllowed(positionTmp) == true)
                                if (isAnyPawnOnPosition(positionTmp) == false)
                                    return true; // musi walczyc
                        }
            }
            return false;
        }



        private bool fight(Position fromPosition, Position toPosition)
        {
            Pawn.Colours fromColour = getPawnColourByItsPosition(fromPosition);

            if (fromPosition.x < toPosition.x) // bijemy na prawo
            {
                if (fromPosition.y < toPosition.y) // bijemy na dol
                {
                    if (isAnyPawnOnPosition(new Position(fromPosition.x + 1, fromPosition.y + 1)))
                    {
                        if (!isAnyPawnOnPosition(new Position(fromPosition.x + 2, fromPosition.y + 2)))
                        {
                            if (getPawnColourByItsPosition(new Position(fromPosition.x + 1, fromPosition.y + 1)) != fromColour) //  pionek bije pionka innego koloru
                            {
                                //usuwam zbitego pionka
                                currentLayout.PawnList.Remove(
                                        currentLayout.PawnList.Find
                                        (pos => pos.position.x == fromPosition.x + 1 &&
                                        pos.position.y == fromPosition.y + 1));
                                //przestawiam zwycieskiego pionka
                                currentLayout.PawnList.Find
                                      (pos => pos.position.x == fromPosition.x &&
                                       pos.position.y == fromPosition.y).position = new Position(toPosition.x, toPosition.y);
                                upgradePawnByPosition(toPosition);
                                return true;
                            }
                        }
                    }
                }
                else if (fromPosition.y > toPosition.y)// bijemy do gory
                {
                    if (isAnyPawnOnPosition(new Position(fromPosition.x + 1, fromPosition.y - 1)))
                    {
                        if (!isAnyPawnOnPosition(new Position(fromPosition.x + 2, fromPosition.y - 2)))
                        {
                            if (getPawnColourByItsPosition(new Position(fromPosition.x + 1, fromPosition.y - 1)) != fromColour) //  pionek bije pionka innego koloru
                            {
                                //usuwam zbitego pionka
                                currentLayout.PawnList.Remove(
                                currentLayout.PawnList.Find
                                      (pos => pos.position.x == fromPosition.x + 1 &&
                                       pos.position.y == fromPosition.y - 1));
                                //przestawiam zwycieskiego pionka
                                currentLayout.PawnList.Find
                                      (pos => pos.position.x == fromPosition.x &&
                                       pos.position.y == fromPosition.y).position = new Position(toPosition.x, toPosition.y);
                                upgradePawnByPosition(toPosition);
                                return true;
                            }
                        }
                    }
                }
            }
            else if (fromPosition.x > toPosition.x) // bije w lewo
            {
                if (fromPosition.y < toPosition.y) // bijemy na dol
                {
                    if (isAnyPawnOnPosition(new Position(fromPosition.x - 1, fromPosition.y + 1)))
                    {
                        if (!isAnyPawnOnPosition(new Position(fromPosition.x - 2, fromPosition.y + 2)))
                        {
                            if (getPawnColourByItsPosition(new Position(fromPosition.x - 1, fromPosition.y + 1)) != fromColour) // pionek bije pionka innego koloru
                            {
                                //usuwam zbitego pionka
                                currentLayout.PawnList.Remove(
                                currentLayout.PawnList.Find
                                     (pos => pos.position.x == fromPosition.x - 1 &&
                                      pos.position.y == fromPosition.y + 1));

                                currentLayout.PawnList.Find
                                      (pos => pos.position.x == fromPosition.x &&
                                       pos.position.y == fromPosition.y).position = new Position(toPosition.x, toPosition.y);
                                upgradePawnByPosition(toPosition);
                                return true;
                            }
                        }
                    }
                }
                else if (fromPosition.y > toPosition.y) // bijemy na gore
                {
                    if (isAnyPawnOnPosition(new Position(fromPosition.x - 1, fromPosition.y - 1)))
                    {
                        if (!isAnyPawnOnPosition(new Position(fromPosition.x - 2, fromPosition.y - 2)))
                        {
                            if (getPawnColourByItsPosition(new Position(fromPosition.x - 1, fromPosition.y - 1)) != fromColour) // pionek bije pionka innego koloru
                            {
                                //usuwam zbitego pionka
                                currentLayout.PawnList.Remove(
                                currentLayout.PawnList.Find
                                     (pos => pos.position.x == fromPosition.x - 1 &&
                                      pos.position.y == fromPosition.y - 1));

                                currentLayout.PawnList.Find
                                      (pos => pos.position.x == fromPosition.x &&
                                       pos.position.y == fromPosition.y).position = new Position(toPosition.x, toPosition.y);
                                upgradePawnByPosition(toPosition);
                                return true;
                            }
                        }
                    }
                }
            }


            return false;
        }
        #endregion

        #region Szczegolowe chodzenie po planszy - nie bicie
        /// <summary>
        /// Metoda odpowiada za ruch pionka o jedno pole
        /// </summary>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <returns></returns>
        private bool walk(Position fromPosition, Position toPosition)
        {
            Pawn.Colours colour = getPawnColourByItsPosition(fromPosition);

            if (isAnyPawnOnPosition(toPosition)) return false;

            if (colour == Pawn.Colours.White)
                if (fromPosition.y > toPosition.y) return false;

            if (colour == Pawn.Colours.Black)
                if (fromPosition.y < toPosition.y) return false;

            


            currentLayout.PawnList.Find
                (pos => pos.position.x == fromPosition.x &&
                        pos.position.y == fromPosition.y).position
                      = new Position(toPosition.x, toPosition.y);
            return true;
        }
        private bool walkQueen(Position fromPosition, Position toPosition)
        {
            Pawn.Colours colour = getPawnColourByItsPosition(fromPosition);

            if (isAnyPawnOnPosition(toPosition) == true) return false;

            currentLayout.PawnList.Find
                (pos => pos.position.x == fromPosition.x &&
                        pos.position.y == fromPosition.y).position
                      = new Position(toPosition.x, toPosition.y);
            return true;
        }
        #endregion

        /// <summary>
        /// Pobiera kolor pionka z danej pozycji
        /// </summary>
        public Pawn.Colours getPawnColourByItsPosition(Position position)
        {
            return (from c in currentLayout.PawnList
                    where c.position.x == position.x
                    where c.position.y == position.y
                    select c.colour).ToList().First();
        }

        /// <summary>
        /// Sprawdza czy na danej pozycji jest jakis pionek
        /// </summary>
        public bool isAnyPawnOnPosition(Position position)
        {
            var query = ((from c in currentLayout.PawnList
                          where c.position.x == position.x && c.position.y == position.y
                          select c).ToList());
            if(query.Count == 0) return false;
            return true;
        }

        /// <summary>
        /// Sprawdza czy pole jest białe (na bialych nic nie moze stac)
        /// </summary>
        public bool isPositionAllowed(Position position)
        {
            if (position.x < 0 || position.x > 7) return false;
            if (position.y < 0 || position.y > 7) return false;

            if ((position.x + position.y) % 2 == 1) return true;
            
            return false;
        }
        public Pawn.Type getPawnTypeByPosition(Position position)
        {
            return (from c in currentLayout.PawnList
                    where c.position.x == position.x
                    where c.position.y == position.y
                    select c.type).ToList().First();
        }
        public void upgradePawnByPosition(Position position)
        {
            if (
                (position.y == 7 && getPawnColourByItsPosition(position) == Pawn.Colours.White) ||
                (position.y == 0 && getPawnColourByItsPosition(position) == Pawn.Colours.Black)
                )
            {
                var query = (from c in currentLayout.PawnList
                             where c.position.x == position.x
                             where c.position.y == position.y
                             select c).ToList().First();
                query.type = Pawn.Type.Queen;
            }
        }

        public bool isOver()
        {
            if ((from c in currentLayout.pawnList
                 where c.colour == Pawn.Colours.White
                 select c).ToList().Count == 0) return true;
            if ((from c in currentLayout.pawnList
                 where c.colour == Pawn.Colours.Black
                 select c).ToList().Count == 0) return true;
           
            return false;
        }


    }
}
