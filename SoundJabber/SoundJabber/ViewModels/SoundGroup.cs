using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundJabber.ViewModels
{
    public class SoundGroup
    {
        public SoundGroup()
        {
            Items = new List<SoundData>();
        }
        public string Title { get; set; }

        public List<SoundData> Items { get; set; }
    }
}
