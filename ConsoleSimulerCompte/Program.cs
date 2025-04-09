using BanqueLib;
using System.Data;
using System.Text.Json;

namespace ConsoleSimulerCompte
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            var compte = ChargerCompte();
            while (true)
            {

                Console.WriteLine(compte.Description());
                Console.WriteLine("""

                 1 - modifier détenteur
                 2 - peut déposer
                 3 - peut retirer
                 4 - peut retirer (montant)
                 5 - déposer (montant)
                 6 - retirer (montant)
                 7 - vider
                 8 - geler
                 9 - dégeler
                 Q - quitter
                 R - reset

                 Votre choix, Samuel Cuerrier ?

                """);

                switch (Console.ReadKey(true).KeyChar)
                {
                    case '1':
                        string nouveauDétenteur = $"{compte.Détenteur}{rnd.Next(1,99)}";
                        Console.WriteLine($"Détenteur modifié pour : {nouveauDétenteur}");
                        compte.SetDétenteur(nouveauDétenteur);
                        break;
                    case '2':
                        if(compte.PeutDéposer())
                        {
                            Console.WriteLine("Peut déposer? Oui");
                        }
                        else
                        {
                            Console.WriteLine("Peut déposer? Non");
                        }
                            break;
                    case '3':
                        {
                            if (compte.PeutRetirer())
                            {
                                Console.WriteLine("Peut retirer? Oui");
                            }
                            else
                            {
                                Console.WriteLine("Peut retirer? Non");
                            }
                            break;
                        }
                    case '4': 
                        {
                            decimal retrait = Math.Round((decimal)(rnd.NextDouble() * (99.99 - 0.01) + 0.01), 2);
                            if (compte.PeutRetirer(retrait))
                            {
                                Console.WriteLine($"Peut retirer {retrait}? Oui");
                            }
                            else
                            {
                                Console.WriteLine($"Peut retirer {retrait}? Non");
                            }
                                break;
                        }
                    case '5': 
                        {
                            decimal dépot = Math.Round((decimal)(rnd.NextDouble() * (99.99 - 0.01) + 0.01), 2);
                            if (!compte._estGelé)
                            {
                                compte.Déposer(dépot);
                                Console.WriteLine($"Dépot de {dépot}");
                            }
                            else
                            {
                                Console.WriteLine($"Impossible de déposer {dépot}");
                            }
                        }
                        break;
                    case '6':
                        {
                            decimal retrait = Math.Round((decimal)(rnd.NextDouble() * (99.99 - 0.01) + 0.01), 2);
                            if (compte.PeutRetirer(retrait))
                            {
                                Console.WriteLine($"Retrait de {retrait}");
                                compte.Retirer(retrait);
                            }
                            else
                            {
                                Console.WriteLine($"Impossible de retirer {retrait}");
                            }
                        }
                        break;
                    case '7': // Geler et dégeler
                        {
                            if (compte._solde <= 0 || compte._estGelé == true)
                            {
                                Console.WriteLine("Impossible de vider un compte vide ou gelé");
                            }
                            else
                            {
                                Console.WriteLine($"Retrait complet de {compte._solde}");
                                compte.Vider();
                            }
                        }
                        break;
                    case '8':
                        {
                            if (compte._estGelé)
                            {
                                Console.WriteLine("Le compte est déja gelé");
                            }
                            else
                            {
                                Console.WriteLine("Le compte a été gelé");
                                compte._estGelé = true;
                                compte._statut = StatutCompte.Gelé;
                            }
                            break;
                        }
                    case '9': 
                        {
                            if (!compte._estGelé)
                            {
                                Console.WriteLine("impossible de dégeler un compte non gelé");
                            }
                            else
                            {
                                Console.WriteLine("Le compte a été dégelé");
                                compte._estGelé = false;
                                compte._statut = StatutCompte.Ok;
                            }
                            break;
                        }

                    case 'q':
                        {
                            SauvegarderCompte(compte);
                            Environment.Exit(0);
                            break;
                        }
                    case 'r':
                        {
                            compte = new Compte(rnd.Next(100, 999), "Samuel Cuerrier");
                            Console.WriteLine("un nouveau compte a été créer");
                        }
                        break;
                    default:
                        Console.WriteLine(" Mauvais choix"); break;
                }
                    Console.WriteLine("\n Appuyer sur ENTER pour continuer...");
                Console.ReadLine();
            }
            

        }

        static void SauvegarderCompte(Compte compte)
        {
            File.WriteAllText("Compte.json", JsonSerializer.Serialize(new
            {
                compte._numéro,
                compte._estGelé,
                compte.Détenteur,
                compte._solde,
                Statut = (int)compte._statut
            }, new JsonSerializerOptions { WriteIndented = true }));
        }
        static Compte ChargerCompte()
        {
            var json = File.ReadAllText("Compte.json");
            var document = JsonDocument.Parse(json);
            var root = document.RootElement;
            int numéro = root.GetProperty("_numéro").GetInt32();
            string détenteur = root.GetProperty("Détenteur").GetString() ?? string.Empty;
            decimal solde = root.GetProperty("_solde").GetDecimal();
            StatutCompte statut = (StatutCompte)root.GetProperty("Statut").GetInt32();
            bool estGelé = root.GetProperty("_estGelé").GetBoolean();
            return new Compte(numéro, détenteur, solde, statut) { _estGelé = estGelé };
        }

    }
}