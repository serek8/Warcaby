using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warcaby.Model
{
    public class Pawn
    {
        public enum Colours:int { White = 1, Black = 2 };
        public enum Type { Chequer = 1, Queen = 2 };
        public Colours colour;
        public Type type;
        public Position position;
       // public bool isAlive;

        public Pawn(Colours colour, int x, int y)
        {
            this.position = new Position(x, y);
            this.colour = colour;
            this.type = Type.Chequer;
        }

        public Pawn()
        {
            this.position = new Position(0, 0);
            this.colour = Pawn.Colours.White;
            this.type = Type.Chequer;
        }

        public void upgradeToQueen()
        {
            type = Type.Queen;
        }

    }
}
