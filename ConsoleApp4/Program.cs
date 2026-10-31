using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;


class Program
{
    static List<Passager> tousLesPassagers = new List<Passager>();
    static List<Vehicule> tousLesVehicules = new List<Vehicule>();


    struct Reservation
    {
        //structure reservation qui aura les informations globales
        // [JsonInclude] sert a enregistrer les données dans le fichier json

        [JsonInclude] public string nom;
        [JsonInclude] public int idLiaison;
        [JsonInclude] public string date;
        [JsonInclude] public string heure;
        [JsonInclude] public string horodatage;
    }

    struct Passager
    {   
        //structure passager qui aura le nom, prénom et catégorie de chaque passager, on envera par la suite chaque passager dans une liste de passager
        [JsonInclude] public string nom;
        [JsonInclude] public string prenom;
        [JsonInclude] public string codeCategorie;
    }

    struct Vehicule
    {
        //structure passager qui aura la catégorie de chaque véhicule et sa quantité, on envera par la suite chaque véhicule dans une liste de véhicule
        [JsonInclude] public string codeCategorie;
        [JsonInclude] public int quantite;
    }
    class ReservationBloc
    {
        //la class ReservationBloc est ici pour la bonne génération du fichier Json avec dans l'ordre : un bloc pour les infos globales de la reservation constitué exclusivement de la structure reservation, puis d'un bloc contenant exclusivement la liste de tous les passagers et enfin un bloc contenant exclusivement la liste de tous les véhicules.
        [JsonInclude] public Reservation reservation;
        [JsonInclude] public List<Passager> passagers;
        [JsonInclude] public List<Vehicule> vehicules;
    }


    static void Main()
    {

        // 1) Saisie de la réservation
        Reservation reservation;                    //on crée une structure réservation
        reservation = SaisirReservation();          //appel de la fonction SaisirReservation

        // 2) Calcul du prix
        double total;
        total = CalculerPrixTotal(reservation);     //appel de la fonction CalculerPrixTotal
        AfficherRecapitulatif(reservation, total);  //appel de la procédure AfficherRecapitulatif

        // 3) Génération du JSON
        string json;
        json = GenererJson(reservation);            //appel de la fonction GenererJson pour créer le fichier Json

        Console.WriteLine();
        Console.WriteLine("=== JSON généré ===");
        Console.WriteLine(json);                    //affichage du contenu du fichier Json

        // 4) Sauvegarde dans un fichier
        System.IO.File.WriteAllText("reservation.json", json);      //on créer un fichier "reservation.json" avec le contenu de la variable json, donc ce qu'on a rempli dans la console lors de l'execution. Note : si le fichier existe déjà, il est écrasé
        Console.WriteLine();
        Console.WriteLine("Fichier reservation.json généré.");
    }


    // ---------- SAISIE ----------

    static Reservation SaisirReservation()
    {

        // dans cette fonction, on demande a l'utilisateur de saisir les informations globales de sa réservation qui seront stockés dans r qui est une structure de type Reservation
        Reservation r;

        r = new Reservation();



        Console.Write("Nom de la réservation : ");
        r.nom = Console.ReadLine();

        Console.WriteLine("Choisissez la liaison :");
        Console.WriteLine("1 - Lorient - Groix");
        Console.WriteLine("2 - Groix - Lorient");
        Console.WriteLine("3 - Quiberon - Le Palais");
        Console.WriteLine("4 - Le Palais - Quiberon");
        r.idLiaison = int.Parse(Console.ReadLine());

        
        DateTime date = DateTime.MinValue;   //on récupère le premier jour du mois de novembre (1)
        bool okDate;    //le booléen okDate sert a vérifier par la suite que la date est bien en novembre 2025

        okDate = false;

        string saisie;


        while (okDate == false)
        {
            Console.Write("Date (jj/mm/2025, novembre uniquement) : ");
            saisie = Console.ReadLine();
            if (DateTime.TryParse(saisie, out date) && date.Month == 11 && date.Year == 2025)  //on vérifie que la date est bien en novembre 2025 et que le jour est entre 00 et 31 exclu 
            {
                okDate = true;
            }
            else
            {
                Console.WriteLine("Date invalide.");
            }
        }
        r.date = date.ToString("yyyy-MM-dd"); //on enregistre la date dans r


        List<string> horaires;      //on initialise la liste des horraires
        int i;
        int choixHeure;

        horaires = ObtenirHoraires(r.idLiaison, date);     //on fait l'appel de fonction de ObtenirHoraire en fonction de la liaison choisie et de la date rentrée

        Console.WriteLine("Heure de départ :");
        for (i = 0; i <= horaires.Count - 1; i++)
        {
            Console.WriteLine((i + 1) + " - " + horaires[i]);       //on affiche les différentes horraires récupérées
        }

        choixHeure = int.Parse(Console.ReadLine());
        r.heure = horaires[choixHeure - 1];     //on stock l'horraire choisie dans r


      
        r.horodatage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");        //on fait l'horodatage en récuperant la date en temps réel

        SaisirPassagers(tousLesPassagers);      //on fait l'appel de procédure SaisirPassager 
        SaisirVehicules(tousLesVehicules, r);   //on fait l'appel de procédure SaisirVéhicule, on prends r en paramètre car un trajet précis n'accepte pas les 4 roues



        return r;
    }


    static void SaisirPassagers(List<Passager> liste)
    {
        Console.WriteLine();
        Console.WriteLine("=== Passagers ===");

        bool continuer;
        continuer = true;           //on crée un booléen continuer pour ajouter des passagers tant que l'utilisateur le souhaite

        while (continuer == true)
        {
            Passager p;
            int choix;
            string reponse;

            p = new Passager();     //on crée un passager avec la structure Passager puis on rentre ses différents attributs : nom, prénom, catégorie

            Console.Write("Nom : ");
            p.nom = Console.ReadLine();

            Console.Write("Prénom : ");
            p.prenom = Console.ReadLine();

            Console.WriteLine("Catégorie :");
            Console.WriteLine("1 - Adulte 26+ (adu26p)");
            Console.WriteLine("2 - Jeune 18-25 (jeu1825)");
            Console.WriteLine("3 - Enfant 4-17 (enf417)");
            Console.WriteLine("4 - Bébé <4 ans (bebe)");
            Console.WriteLine("5 - Animal (ancomp)");

            choix = int.Parse(Console.ReadLine());

            switch (choix)
            {
                case 1:
                    p.codeCategorie = "adu26p";
                    break;
                case 2:
                    p.codeCategorie = "jeu1825";
                    break;
                case 3:
                    p.codeCategorie = "enf417";
                    break;
                case 4:
                    p.codeCategorie = "bebe";
                    break;
                default:
                    p.codeCategorie = "ancomp";
                    break;
            }

            liste.Add(p);

            Console.Write("Ajouter un autre passager ? (o/n) : ");      //on propose a l'utilisateur d'ajouter des passagers supplémentaires
            reponse = Console.ReadLine();
            if (reponse == "o")
            {
                continuer = true;
            }
            else
            {
                continuer = false;
            }
        }
    }



    static void SaisirVehicules(List<Vehicule> liste, Reservation r)
    {
        string reponse;
        bool continuer;
        bool roues4interdit;

        Console.WriteLine();
        Console.Write("Y a-t-il des véhicules ? (o/n) : "); //on demande a l'utilisateur si il veut ajouter des véhicules ou non
        reponse = Console.ReadLine();

        if (reponse != "o")
        {
            return;     //on arrete la fonction si il n'y a pas de véhicules
        }

        if (r.idLiaison == 4 && r.date == "2025-11-06" && r.heure == "16:15")       //on vérifie si le trajet est interdit aux 4 roues ou non
        {
            roues4interdit = true;
        }
        else
        {
            roues4interdit = false;
        }

        Console.WriteLine("=== Véhicules ===");
        continuer = true;
        if (roues4interdit)
        {
            Console.WriteLine("Attention : 4 roues interdits sur ce trajet");       //si le trajet est interdit aux 4 roues, on en informe l'utilisateur
        }
        while (continuer == true)
        {
            if (roues4interdit)     //si le trajet est interdit aux 4 roues, on ne lui propose que des véhicules 2 roues
            {
                Vehicule v;
                int choix;

                v = new Vehicule();

                Console.WriteLine("Catégorie :");
                Console.WriteLine("1 - Trottinette (trot)");
                Console.WriteLine("2 - Vélo / remorque (velo)");
                Console.WriteLine("3 - Vélo électrique (velelec)");
                Console.WriteLine("4 - Vélo cargo / tandem (cartand)");
                Console.WriteLine("5 - Deux-roues <=125 (mobil)");
                Console.WriteLine("6 - Deux-roues >125 (moto)");

                choix = int.Parse(Console.ReadLine());

                switch (choix)
                {
                    case 1:
                        v.codeCategorie = "trot";
                        break;
                    case 2:
                        v.codeCategorie = "velo";
                        break;
                    case 3:
                        v.codeCategorie = "velelec";
                        break;
                    case 4:
                        v.codeCategorie = "cartand";
                        break;
                    case 5:
                        v.codeCategorie = "mobil";
                        break;
                    case 6:
                        v.codeCategorie = "moto";
                        break;
                    default:
                        v.codeCategorie = "";
                        break;
                }

                Console.Write("Quantité : ");                   //on demande a l'utilisateur de saisir la quantité du véhicule sélectionné
                v.quantite = int.Parse(Console.ReadLine());

                liste.Add(v);

                Console.Write("Ajouter un autre véhicule ? (o/n) : "); //on propose a l'utilisateur d'ajouter d'aautres véhicules pour ce trajet
                reponse = Console.ReadLine();
                if (reponse == "o")
                {
                    continuer = true;
                }
                else
                {
                    continuer = false;
                }
            }
            else      //si le trajet n'est pas interdit aux 4 roues, on lui propose tous les véhicules possible
            {
                Vehicule v;
                int choix;

                v = new Vehicule();

                Console.WriteLine("Catégorie :");
                Console.WriteLine("1 - Trottinette (trot)");
                Console.WriteLine("2 - Vélo / remorque (velo)");
                Console.WriteLine("3 - Vélo électrique (velelec)");
                Console.WriteLine("4 - Vélo cargo / tandem (cartand)");
                Console.WriteLine("5 - Deux-roues <=125 (mobil)");
                Console.WriteLine("6 - Deux-roues >125 (moto)");
                Console.WriteLine("7 - Voiture <4 m (cat1)");
                Console.WriteLine("8 - Voiture 4-4.39 (cat2)");
                Console.WriteLine("9 - Voiture 4.40-4.79 (cat3)");
                Console.WriteLine("10 - Voiture 4.80+ (cat4)");
                Console.WriteLine("11 - Camping-car >2.10 (camp)");

                choix = int.Parse(Console.ReadLine());

                switch (choix)
                {
                    case 1:
                        v.codeCategorie = "trot";
                        break;
                    case 2:
                        v.codeCategorie = "velo";
                        break;
                    case 3:
                        v.codeCategorie = "velelec";
                        break;
                    case 4:
                        v.codeCategorie = "cartand";
                        break;
                    case 5:
                        v.codeCategorie = "mobil";
                        break;
                    case 6:
                        v.codeCategorie = "moto";
                        break;
                    case 7:
                        v.codeCategorie = "cat1";
                        break;
                    case 8:
                        v.codeCategorie = "cat2";
                        break;
                    case 9:
                        v.codeCategorie = "cat3";
                        break;
                    case 10:
                        v.codeCategorie = "cat4";
                        break;
                    case 11:
                        v.codeCategorie = "camp";
                        break;
                    default:
                        v.codeCategorie = "";
                        break;
                }

                Console.Write("Quantité : ");                   //on demande a l'utilisateur de saisir la quantité du véhicule sélectionné
                v.quantite = int.Parse(Console.ReadLine());

                liste.Add(v);

                Console.Write("Ajouter un autre véhicule ? (o/n) : ");      //on propose a l'utilisateur d'ajouter d'aautres véhicules pour ce trajet
                reponse = Console.ReadLine();
                if (reponse == "o")
                {
                    continuer = true;
                }
                else
                {
                    continuer = false;
                }
            }

        }
    }


    static List<string> ObtenirHoraires(int idLiaison, DateTime date)
    {
        List<string> horaires;
        int jour;

        horaires = new List<string>();
        jour = date.Day; // 1 à 30

        //on va ajouter différents horraires a la liste d'horraire disponible en fonction de la traversée et du jour

        if (idLiaison == 1) // Lorient -> Groix
        {
            if (jour == 1 || jour == 2 || jour == 9 || jour == 11 || jour == 16 || jour == 23 || jour == 30)
            {
                horaires.Add("09:45");
                horaires.Add("12:15");
                horaires.Add("17:00");
                horaires.Add("19:30");
            }
            else
            {
                horaires.Add("08:05");
                horaires.Add("11:00");
                horaires.Add("13:45");
                horaires.Add("16:15");
                horaires.Add("18:45");
            }
        }
        else if (idLiaison == 2) // Groix -> Lorient
        {
            if (jour == 1 || jour == 2 || jour == 9 || jour == 11 || jour == 16 || jour == 23 || jour == 30)
            {
                horaires.Add("08:30");
                horaires.Add("11:00");
                horaires.Add("15:45");
                horaires.Add("18:15");
            }
            else
            {
                horaires.Add("06:50");
                horaires.Add("09:30");
                horaires.Add("12:30");
                horaires.Add("15:00");
                horaires.Add("17:30");
            }
        }
        else if (idLiaison == 3) // Quiberon -> Le Palais
        {
            if (jour == 1)
            {
                horaires.Add("08:15");
                horaires.Add("09:30");
                horaires.Add("11:15");
                horaires.Add("12:45");
                horaires.Add("14:45");
                horaires.Add("17:15");
                horaires.Add("19:00");
                horaires.Add("20:00");
            }
            else if (jour == 2)
            {
                horaires.Add("08:00");
                horaires.Add("09:30");
                horaires.Add("11:15");
                horaires.Add("12:30");
                horaires.Add("14:15");
                horaires.Add("15:45");
                horaires.Add("17:15");
                horaires.Add("19:00");
                horaires.Add("20:00");
            }
            else if (jour == 3)
            {
                horaires.Add("08:00");
                horaires.Add("09:30");
                horaires.Add("11:15");
                horaires.Add("14:30");
                horaires.Add("17:30");
                horaires.Add("19:30");
            }

            else if (jour == 4)
            {
                horaires.Add("07:30");
                horaires.Add("09:30");
                horaires.Add("10:30");
                horaires.Add("13:30");
                horaires.Add("18:15");
                horaires.Add("19:30");
            }

            else if (jour == 5)
            {
                horaires.Add("08:00");
                horaires.Add("10:30");
                horaires.Add("12:00");
                horaires.Add("17:15");
                horaires.Add("19:00");
                horaires.Add("20:00");
            }

            else if (jour == 6)
            {
                horaires.Add("07:30");
                horaires.Add("08:30");
                horaires.Add("11:45");
                horaires.Add("14:45");
                horaires.Add("18:00");
                horaires.Add("19:45");
            }

            else if (jour == 7)
            {
                horaires.Add("08:00");
                horaires.Add("09:30");
                horaires.Add("12:30");
                horaires.Add("15:30");
                horaires.Add("18:15");
                horaires.Add("20:00");
            }

            else if (jour == 8)
            {
                horaires.Add("08:15");
                horaires.Add("09:45");
                horaires.Add("11:00");
                horaires.Add("14:15");
                horaires.Add("17:15");
                horaires.Add("19:30");
            }

            else if (jour == 10)
            {
                horaires.Add("08:00");
                horaires.Add("09:30");
                horaires.Add("11:15");
                horaires.Add("14:15");
                horaires.Add("17:30");
                horaires.Add("19:30");
            }

            else if (jour == 11)
            {
                horaires.Add("08:15");
                horaires.Add("11:15");
                horaires.Add("14:15");
                horaires.Add("16:15");
                horaires.Add("17:30");
                horaires.Add("19:30");
            }

            else if (jour == 9 || jour == 16 || jour == 23 || jour == 30)
            {
                horaires.Add("08:00");
                horaires.Add("11:15");
                horaires.Add("14:15");
                horaires.Add("16:15");
                horaires.Add("17:30");
                horaires.Add("19:30");
            }

            else if (jour == 15 || jour == 22 || jour == 29)
            {
                horaires.Add("08:00");
                horaires.Add("09:30");
                horaires.Add("11:15");
                horaires.Add("14:15");
                horaires.Add("17:30");
                horaires.Add("19:30");
            }

            else if (jour == 12 || jour == 13 || jour == 17 || jour == 18 || jour == 19 || jour == 20 || jour == 24 || jour == 25 || jour == 26 || jour == 27)
            {
                horaires.Add("08:00");
                horaires.Add("09:30");
                horaires.Add("11:15");
                horaires.Add("14:15");
                horaires.Add("16:15");
                horaires.Add("17:15");
                horaires.Add("20:00");
            }

            else if (jour == 14 || jour == 21 || jour == 28)
            {
                horaires.Add("08:00");
                horaires.Add("09:30");
                horaires.Add("11:15");
                horaires.Add("14:15");
                horaires.Add("17:15");
                horaires.Add("19:15");
                horaires.Add("20:15");
            }


        }
        else if (idLiaison == 4) // Le Palais -> Quiberon
        {
            if (jour == 1)
            {
                horaires.Add("06:45");
                horaires.Add("07:45");
                horaires.Add("09:45");
                horaires.Add("11:00");
                horaires.Add("12:45");
                horaires.Add("14:45");
                horaires.Add("17:30");
                horaires.Add("18:45");
            }
            else if (jour == 2)
            {
                horaires.Add("06:30");
                horaires.Add("07:45");
                horaires.Add("09:45");
                horaires.Add("11:00");
                horaires.Add("12:45");
                horaires.Add("14:15");
                horaires.Add("15:45");
                horaires.Add("17:15");
            }
            else if (jour == 3)
            {
                horaires.Add("06:30");
                horaires.Add("07:45");
                horaires.Add("09:45");
                horaires.Add("12:45");
                horaires.Add("16:00");
                horaires.Add("18:00");
            }

            else if (jour == 4)
            {
                horaires.Add("06:00");
                horaires.Add("07:45");
                horaires.Add("09:00");
                horaires.Add("12:00");
                horaires.Add("16:45");
                horaires.Add("18:00");
            }

            else if (jour == 5)
            {
                horaires.Add("06:30");
                horaires.Add("07:45");
                horaires.Add("10:30");
                horaires.Add("13:30");
                horaires.Add("17:30");
                horaires.Add("18:30");
            }

            else if (jour == 6)
            {
                horaires.Add("06:00");
                horaires.Add("07:00");
                horaires.Add("09:00");
                horaires.Add("13:15");
                horaires.Add("16:15"); // attention pas de voiture
                horaires.Add("18:30");
            }

            else if (jour == 7)
            {
                horaires.Add("06:30");
                horaires.Add("07:45");
                horaires.Add("09:45");
                horaires.Add("14:00");
                horaires.Add("16:45");
                horaires.Add("18:45");
            }

            else if (jour == 8)
            {
                horaires.Add("07:00");
                horaires.Add("08:00");
                horaires.Add("09:45");
                horaires.Add("12:45");
                horaires.Add("15:45");
                horaires.Add("18:00");
            }

            else if (jour == 10)
            {
                horaires.Add("06:30");
                horaires.Add("07:45");
                horaires.Add("09:45");
                horaires.Add("12:45");
                horaires.Add("15:45");
                horaires.Add("18:00");
            }

            else if (jour == 11)
            {
                horaires.Add("07:00");
                horaires.Add("09:45");
                horaires.Add("12:45");
                horaires.Add("14:45");
                horaires.Add("15:45");
                horaires.Add("18:00");
            }

            else if (jour == 15 || jour == 22 || jour == 29)
            {
                horaires.Add("06:30");
                horaires.Add("07:45");
                horaires.Add("09:45");
                horaires.Add("12:45");
                horaires.Add("15:45");
                horaires.Add("18:00");
            }

            else if (jour == 9 || jour == 16 || jour == 23 || jour == 30)
            {
                horaires.Add("07:15");
                horaires.Add("09:45");
                horaires.Add("12:45");
                horaires.Add("14:45");
                horaires.Add("15:45");
                horaires.Add("17:45");
            }

            else if (jour == 12 || jour == 13 || jour == 17 || jour == 18 || jour == 19 || jour == 20 || jour == 24 || jour == 25 || jour == 26 || jour == 27)
            {
                horaires.Add("06:30");
                horaires.Add("07:45");
                horaires.Add("09:45");
                horaires.Add("12:45");
                horaires.Add("14:45");
                horaires.Add("15:45");
                horaires.Add("18:30");
            }

            else if (jour == 14 || jour == 21 || jour == 28)
            {
                horaires.Add("06:30");
                horaires.Add("07:45");
                horaires.Add("09:45");
                horaires.Add("12:45");
                horaires.Add("15:45");
                horaires.Add("17:45");
                horaires.Add("18:45");
            }
        }

        return horaires;
    }


    static double CalculerPrixTotal(Reservation r)
    {
        bool versGroix;
        Dictionary<string, double> tarifsPassagers;
        Dictionary<string, double> tarifsVehicules;

        double totalPassagers;
        double totalVehicules;
        int i;
        Passager p;
        Vehicule v;


        versGroix = (r.idLiaison == 1 || r.idLiaison == 2);     //on regarde si le trajet contient le port de l'île de Groix, car les tarifs sont différents si il s'agit des trajets 1 2 ou 3 4

        if (versGroix)      //on ajoute ensuite les tarifs correspondants
        {

            // Tarifs passagers
            tarifsPassagers = new Dictionary<string, double>();
            tarifsPassagers.Add("adu26p", 18.75);
            tarifsPassagers.Add("jeu1825", 13.80);
            tarifsPassagers.Add("enf417", 11.25);
            tarifsPassagers.Add("bebe", 0.0);
            tarifsPassagers.Add("ancomp", 3.35);

            // Tarifs véhicules
            tarifsVehicules = new Dictionary<string, double>();
            tarifsVehicules.Add("trot", 4.70);
            tarifsVehicules.Add("velo", 8.20);
            tarifsVehicules.Add("velelec", 11.00);
            tarifsVehicules.Add("cartand", 16.45);
            tarifsVehicules.Add("mobil", 23.10);
            tarifsVehicules.Add("moto", 66.05);
            tarifsVehicules.Add("cat1", 96.05);
            tarifsVehicules.Add("cat2", 114.80);
            tarifsVehicules.Add("cat3", 174.45);
            tarifsVehicules.Add("cat4", 210.90);
            tarifsVehicules.Add("camp", 330.20);
        }
        else
        {
            // Tarifs passagers
            tarifsPassagers = new Dictionary<string, double>();
            tarifsPassagers.Add("adu26p", 18.80);
            tarifsPassagers.Add("jeu1825", 14.10);
            tarifsPassagers.Add("enf417", 11.65);
            tarifsPassagers.Add("bebe", 0.0);
            tarifsPassagers.Add("ancomp", 3.35);

            // Tarifs véhicules
            tarifsVehicules = new Dictionary<string, double>();
            tarifsVehicules.Add("trot", 4.70);
            tarifsVehicules.Add("velo", 8.20);
            tarifsVehicules.Add("velelec", 11.00);
            tarifsVehicules.Add("cartand", 16.45);
            tarifsVehicules.Add("mobil", 23.35);
            tarifsVehicules.Add("moto", 66.40);
            tarifsVehicules.Add("cat1", 98.50);
            tarifsVehicules.Add("cat2", 117.20);
            tarifsVehicules.Add("cat3", 176.90);
            tarifsVehicules.Add("cat4", 213.35);
            tarifsVehicules.Add("camp", 332.70);
        }

        // on calcule le prix total des passagers
        totalPassagers = 0.0;
        for (i = 0; i <= tousLesPassagers.Count - 1; i++)
        {
            p = tousLesPassagers[i];
            totalPassagers = totalPassagers + tarifsPassagers[p.codeCategorie];
        }

        // on calcule le prix total des véhicules
        totalVehicules = 0.0;
        for (i = 0; i <= tousLesVehicules.Count - 1; i++)
        {
            v = tousLesVehicules[i];
            totalVehicules = totalVehicules + (tarifsVehicules[v.codeCategorie] * v.quantite);
        }

        return totalPassagers + totalVehicules;
    }

    static void AfficherRecapitulatif(Reservation r, double total)
    {
        //on affiche un rapide récapitulatif de la commande avec le nom de la commande, la liaison avec la date et l'heure, le nombre de passager et de véhicules et enfin le prix total
        Console.WriteLine();
        Console.WriteLine("=== Récapitulatif ===");
        Console.WriteLine("Nom : " + r.nom);
        Console.WriteLine("Liaison : " + r.idLiaison + "  Date : " + r.date + "  Heure : " + r.heure);
        Console.WriteLine("Passagers : " + tousLesPassagers.Count);
        Console.WriteLine("Véhicules : " + tousLesVehicules.Count);
        Console.WriteLine("Total : " + total.ToString("F2") + " €");
    }




    // ---------- JSON ----------

    static string GenererJson(Reservation r)
    {
        ReservationBloc bloc;
        List<ReservationBloc> liste;
        JsonSerializerOptions options;
        string resultat;
        int i;
        //on crée les différents blocs de données du fichier Json
        bloc = new ReservationBloc();
        bloc.reservation = r;
        bloc.passagers = new List<Passager>();
        bloc.vehicules = new List<Vehicule>();

        for (i = 0; i <= tousLesPassagers.Count - 1; i++)
        {
            bloc.passagers.Add(tousLesPassagers[i]);        //on ajoute les passagers au bloc passagers qui est une liste de Passager
        }

        for (i = 0; i <= tousLesVehicules.Count - 1; i++)
        {
            bloc.vehicules.Add(tousLesVehicules[i]);        //on ajoute les véhicules au bloc véhicules qui est une liste de Vehicule
        }

        liste = new List<ReservationBloc>();        //on ajoute tout ca dans une liste et a partir de cette liste on va créer le Json
        liste.Add(bloc);

        options = new JsonSerializerOptions();      //on crée ici le Json
        options.WriteIndented = true;               //on formate le Json correctement avec les indentations
        options.IncludeFields = true;               //on prends en compte tout ce qu'on a rempli plus tot ce qui est possible grace aux [JsonInclude] qu'on avait mis dans les structures
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;          //on force le tout a être en CamelCase

        resultat = JsonSerializer.Serialize(liste, options);        //et enfin on stocke le tout dans résultat
        return resultat;
    }



}
