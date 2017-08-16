using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PasswordChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[$@!%<>,;*?_'#=&])(?!.*([A-Za-z\d$@!%<>,;*?_'#=&])\1{1})[A-Za-z\d$@!%<>,;*?_'#=&]{8,15}$";
            string password = null;


            while (true)
            {
                Console.WriteLine("\nPassword: ");
                password = Console.ReadLine();

                // Prvih 5 tacaka zadatka je reseno regularnim izrazom
                Match m = Regex.Match(password, pattern, RegexOptions.Singleline);

                if (!m.Success)
                {
                    Console.WriteLine("Incorrect password. Please try again.");
                    continue;
                }
                // 6. tacka je resena uz pomoc linq izraza
                // Pobrojao sam pojavljivanje svakog karaktera, sortirao opadajuce, ako se prvi pojavljuje 3 ili vise puta, ne valja sifra, ostale ne treba ispitivati
                var result = password.GroupBy(c => c).Select(c => new { Char = c.Key, Count = c.Count() }).OrderByDescending(c => c.Count).First().Count;

                if (result > 3)
                {
                    Console.WriteLine("Incorrect password. Please try again.");
                    continue;
                }

                int length = password.Length; // kraci zapis, inace beskorisno
                bool incorrect = false;

                // Iteriram do polovine duzine i proveravam da li postoje 2 cifre koje daju tu duzinu u okviru lozinke.

                for (int i = 0; i <= length / 2; i++)
                {
                    int diff = length - i;

                    // Ukoliko je razlika veca od 9, to vise nije cifra, te se sigurno ne nalazi u lozinki, to preskacem.
                    if (diff > 9)
                        continue;

                    int index1 = password.IndexOf((char)('0' + i));
                    if (index1 == -1)
                        continue;

                    // IndexOf bi opet vratio poziciju iste cifre, te treba pretraziti sve nakon te pozicije
                    int index2 = password.Substring(index1 + 1).IndexOf((char)('0' + diff));
                    if (index2 == -1)
                        continue;

                    // sadrzi obe cifre koje daju duzinu, izadji iz petlje, ispisi gresku
                    incorrect = true;
                    break;
                }
                if (incorrect)
                {
                    Console.WriteLine("Incorrect password. Please try again.");
                    continue;
                }

                Console.WriteLine("Welcome summoner!");
                break;
            }
        }
    }
}
