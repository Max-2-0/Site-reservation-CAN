using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class sae_11
{
    struct reservation
    {
        public string nom;
        public int idLiaison;
        public string date;
        public string heure;
        public string horodatage;

        //Constructeur réservation

        public reservation(string nm, int id, string d, string h, string horo)
        {
            nom = nm;
            idLiaison = id;
            date = d;
            heure = h;
            horodatage = horo;
        }
    }

    struct passager
    {
        public string nom;
        public string prenom;
        public string codeCategorie;

        //Constructeur passager

        public passager(string nm, string pn, string cc)
        {
            nom = nm;
            prenom = pn;
            codeCategorie = cc;
        }
    }

    struct vehicule
    {
        public string codeCategorie;
        public int quantite;

        //Constructeur vehicule

        public vehicule(string cc, int q)
        {
            codeCategorie = cc;
            quantite = q;
        }
    }



    public static void Main()
    {
        List<passager> lesPassagers;
        List<vehicule> lesVehicules;
        
        lesPassagers = new List<passager>();
        lesVehicules = new List<vehicule>();


    }
}

