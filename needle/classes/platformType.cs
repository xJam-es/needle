using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace needle.classes
{
    public class platformCollection
    {
        private List<platformSingle> _listed = new List<platformSingle>();

        public platformCollection()
        {
            _listed.Add(new platformSingle());
            _listed.Add(new platformSingle(1, "Sony PS Vita",0x81000000));
            _listed.Add(new platformSingle(2, "Sony PSP", 0x08800000));
            _listed.Add(new platformSingle(3, "Nintendo DS", 0x00200000));
        }

        public List<platformSingle> list
        {
            get { return _listed; }
        }
        public platformSingle findById( int id)
        {
            platformSingle tmp = new platformSingle();
            foreach( platformSingle pl in _listed)
            {
                if (id == pl.ID) tmp = pl;
            }
            return tmp;
        }
    }

    public class platformSingle
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public long MEMRANGE_BEGIN { get; set; }
        public string memRegionHex { get { return MEMRANGE_BEGIN.ToString("X8"); } set { try { MEMRANGE_BEGIN = Convert.ToInt32(value); } catch (Exception) { MEMRANGE_BEGIN = 0; } } }
        public platformSingle()
        {
            ID = 0;
            NAME = "Custom";
            MEMRANGE_BEGIN = 0;
        }

        public platformSingle(int id, string name, long mrb)
        {
            ID = id;
            NAME = name;
            MEMRANGE_BEGIN = mrb;
        }
    }
}
