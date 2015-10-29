using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentroFormacion.model
{
    public partial class CentroFormacionEntities
    {
        public CentroFormacionEntities(bool lazy)
            : base("name=CentroFormacionEntities")
        {
            Configuration.LazyLoadingEnabled = lazy;
        }
    }
}
