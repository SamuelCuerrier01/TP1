using System.Text.Json.Serialization;

namespace BanqueLib
{
    public enum StatutCompte {Ok, Gelé}

    public class Compte
    {
        #region -----champs-----
        public readonly int _numéro;
        public string _détenteur;
        public decimal _solde = 0;
        public StatutCompte _statut = StatutCompte.Ok;
        public bool _estGelé = false;
        #endregion

        #region -----constructeurs-----
        [JsonConstructor]
        public Compte(int numéro, string détenteur, decimal solde,StatutCompte statut)
        {
            if (numéro < 0)
                throw new ArgumentOutOfRangeException("Le numéro de compte doit être positif");
            _numéro = numéro;
            SetDétenteur(détenteur);
            if(solde < 0 || decimal.Round(Convert.ToDecimal(solde), 2) != Convert.ToDecimal(solde))
                throw new ArgumentOutOfRangeException("le solde est négatif ou est trop précis");
            _solde = solde;
            _statut = statut;
            _estGelé = (_statut == StatutCompte.Gelé);
        }
        public Compte(int numéro, string détenteur, decimal solde)
        {
            if (numéro < 0)
                throw new ArgumentOutOfRangeException("Le numéro de compte doit être positif");
            _numéro = numéro;
            SetDétenteur(détenteur);
            if (solde < 0 || decimal.Round(Convert.ToDecimal(solde), 2) != Convert.ToDecimal(solde))
                throw new ArgumentOutOfRangeException("le solde est négatif ou est trop précis");
            _solde = solde;
            _estGelé = (_statut == StatutCompte.Gelé);
            if (_estGelé)
            {
                _statut = StatutCompte.Gelé;
            }
            else
            {
                _statut = StatutCompte.Ok;
            }

        }
        public Compte(int numéro, string détenteur)
        {
            if (numéro < 0)
                throw new ArgumentOutOfRangeException("Le numéro de compte doit être positif");
            _numéro = numéro;
            SetDétenteur(détenteur);
            _solde = 0;
            _estGelé = (_statut == StatutCompte.Gelé);
            if (_estGelé)
            {
                _statut = StatutCompte.Gelé;
            }
            else
            {
                _statut = StatutCompte.Ok;
            }
        }
        #endregion

        #region -----Getters/Setters-----
        public string Détenteur => _détenteur;
        public void SetDétenteur(string nomDétenteur)
        {
            if(string.IsNullOrWhiteSpace(nomDétenteur))
                throw new ArgumentNullException("veuillez saisir le nom du détenteur");
            _détenteur = nomDétenteur.Trim();
        }
        #endregion

        #region -----Méthodes modifiantes-----
        public string Description()
        {
            return $@"
**************************************
*                                    *
*     COMPTE : {_numéro,-22}*
*     Solde : {_solde,-23:C2}*
*     De : {Détenteur,-24}  *
*     STATUT : {_statut,-22}*
*                                    *
**************************************
";
        }
        public bool PeutDéposer(decimal montant = 1)
        {
            if (montant <= 0 || decimal.Round(Convert.ToDecimal(montant), 2) != Convert.ToDecimal(montant))
                throw new ArgumentOutOfRangeException("veuillez entrer un nombre plus grand que 0 et a deux décimals");
            else
            {
                if (_estGelé)
                    return false;
                else return true;
            }
        }
        public bool PeutRetirer(decimal montant = 1)
        {
            if (montant <= 0 || decimal.Round(Convert.ToDecimal(montant), 2) != Convert.ToDecimal(montant))
                throw new ArgumentOutOfRangeException("veuillez entrer un nombre plus grand que 0 et a deux décimals");
            else
            {
                if (_estGelé)
                    return false;
                else if(montant < _solde)
                    return true;
                else return false;
            }
        }
        public decimal Vider()
        {
            if (_solde > 0 && _statut != StatutCompte.Gelé)
            {
                decimal montant = _solde;
                _solde = 0;
                return montant;
            }
            else
            {
                throw new InvalidOperationException("Le compte est déjà vidé ou il est gelé.");
            }
        }
        public void Geler()
        {
            if (_estGelé == false)
            {
                _estGelé = true;
                _statut = StatutCompte.Gelé;
            }
            else
            {
                throw new InvalidOperationException("le compte est déja gelé");
            }
        }
        public void Dégeler()
        {
            if (_estGelé == true)
            {
                _estGelé = false;
                _statut = StatutCompte.Ok;
            }
            else
            {
                throw new InvalidOperationException("le compte est déja dégelé");
            }
        }
        #endregion

        #region -----Méthodes calculantes-----
        public void Déposer(decimal montant)
        {
            if (!PeutDéposer(montant))
                throw new InvalidOperationException("Le dépôt est refusé.");

            _solde += montant;
        }
        public void Retirer(decimal montant)
        {
            if (PeutRetirer(montant))
                {
                    _solde -= montant;
                }
        }
        #endregion



    }
}
