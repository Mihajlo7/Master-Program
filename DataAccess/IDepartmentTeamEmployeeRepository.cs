using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IDepartmentTeamEmployeeRepository
    {
        // Vrati sve timove i zaposlene po department *
        // Vrati sve timove i zaposlene po department naziv
        // Vrati sve po timove po department id
        // vrati zaposlene po id tima
        // vrati zaposlenog po Id
        // vrati sve departmenta sa timovima koji su u status Active i sort po nazivu
        // vrati sve department sa timovima u kojima je sef mladji od 35 g i imaju Engineer
        // vrati department i broj zaposlenih koji su izmedju 30 i 40 godina
        // vrati za svaki departemnt vrati timove koje broje zaposlene koji su Engineer i ima vise od 40 i sort
    }
}
