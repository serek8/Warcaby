using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warcaby.Model;

namespace Warcaby.Controller
{
    public class PositionFixer : Model.Layout
    {
        public static Layout loadCoordinatesFromRafalAndSaveToLayout(string fileName = "plansza2.txt")
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                Layout layout = new Layout();
                List<int> list = new List<int>();
                int x = 0, y = 0;
                int colourToConvert;
                //Pawn.Colours colourAfterConversion = Pawn.Colours.White;
                char e;
               // reader.ReadChar(); 
                //Pawn.Type pawnType = Pawn.Type.Chequer;
                Pawn pawn;
                while (true)
                {
                    e = reader.ReadChar();
                    pawn = new Pawn();
                    if (e == '1' || e == '0' || e == '2' || e == '3' || e == '4')
                    {
                        colourToConvert = int.Parse(e.ToString());
                        if (colourToConvert != 0) // pionek istnieje
                        {
                            if (colourToConvert == 1)
                            {
                                //colourAfterConversion = Pawn.Colours.White;
                                pawn.type = Pawn.Type.Chequer;
                                pawn.colour = Pawn.Colours.White;
                            }
                            else if (colourToConvert == 2)
                            {
                                //colourAfterConversion = Pawn.Colours.Black;
                                pawn.type = Pawn.Type.Chequer;
                                pawn.colour = Pawn.Colours.Black;
                            }
                            else if (colourToConvert == 3)
                            {
                                //colourAfterConversion = Pawn.Colours.White;
                                pawn.colour = Pawn.Colours.White;
                                pawn.type = Pawn.Type.Queen;
                            }
                            else if (colourToConvert == 4)
                            {
                                //colourAfterConversion = Pawn.Colours.Black;
                                pawn.colour = Pawn.Colours.Black;
                                pawn.type = Pawn.Type.Queen;
                            }
                            if (y % 2 == 0)
                            {
                                pawn.position.x = 2 * x + 1;
                                pawn.position.y = y;
                                layout.pawnList.Add(pawn);
                            }
                            else
                            {
                                pawn.position.x = 2 * x;
                                pawn.position.y = y;
                                layout.pawnList.Add(pawn);
                            }
                        }
                        x++;
                        if (x == 4)
                        {
                            x = 0;
                            y++;
                        }
                        
                    }
                    if (reader.PeekChar() == -1) break;
                }

                return layout;
            }
            
        }



        public static void saveCoordinatesToRafalFormat(Board board, string fileName = "plansza1.txt")
        {
            Position poition = new Position();
            File.Delete(fileName);
            using (StreamWriter writer = new StreamWriter(File.Open(fileName, FileMode.CreateNew)))
            {
                for (int y = 0; y < 8; y++)
                {
                    for (int x = 0; x < 8; x++)
                    {
                        poition.x = x;
                        poition.y = y;
                        if (board.isPositionAllowed(poition) == false)
                        {
                            
                            continue;
                        }

                        // jezeli nie ma pionka
                        if (board.isAnyPawnOnPosition(poition) == false)
                        {
                            writer.Write(0);
                           
                        }
                        else if (board.getPawnColourByItsPosition(poition) == Pawn.Colours.White)
                        {
                            if(board.getPawnTypeByPosition(poition) == Pawn.Type.Chequer)
                                writer.Write(1);
                            else writer.Write(3);
                        }
                        else if (board.getPawnColourByItsPosition(poition) == Pawn.Colours.Black)
                        {
                            if (board.getPawnTypeByPosition(poition) == Pawn.Type.Chequer)
                                writer.Write(2);
                            else writer.Write(4);
                        }
                       // if (x == 7) writer.Write("\n");
                        if (x != 6 && x != 7) writer.Write(" ");
                    }
                    if(y!=7) writer.Write(Environment.NewLine);

                }

            }
        }


    }
}
