using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    // Jak Slab veza Projekat i Zadaci
    public interface IProjectTasksRepository : IRepository
    {
        void InsertOne();
        int InsertMany();
        void InsertBulk();

        // sve *
        // sve sa nazivom i join
        // sve nazivom i subquery
        // po id
        // pronadji sve koji imaju budyet neki i datum koji je poceo pre nekog dana
        // prikazi po svakom projektu koliko ima zadataka koji su otvoreni
        // prikazi order bay sve projekte i zadatke koji koji se zavrsavaju za 3 dana

        // Azuriraj zadatke po projektu i statusu, postavi status na pending
        // azuriraj zadatke tako sto cu mu produziti rok za 3 dana za neki projekat
        // povecaj budzet za one projekte koji imaju vise od 15 zadataka

    }
}
