using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatvanyossagokApplication
{
    class Latvanyossag
    {
        int id;
        string nev;
        string leiras;
        int ar;
        int varosId;

        public Latvanyossag(int id, string nev, string leiras, int ar, int varosId)
        {
            this.Id = id;
            this.Nev = nev;
            this.Leiras = leiras;
            this.Ar = ar;
            this.VarosId = varosId;
        }

        public int Id { get => id; set => id = value; }
        public string Nev { get => nev; set => nev = value; }
        public string Leiras { get => leiras; set => leiras = value; }
        public int Ar { get => ar; set => ar = value; }
        public int VarosId { get => varosId; set => varosId = value; }

        public override string ToString()
        {
            return this.nev + " " + this.leiras + " " + this.ar + " ";
        }
    }
}
