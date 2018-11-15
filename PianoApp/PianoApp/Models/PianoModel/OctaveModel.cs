using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PianoApp.Models
{
    class OctaveModel
    {
        public int Position { get; set; }
        public List<KeyModel> KeyModelList { get; set; } = new List<KeyModel>();

        public OctaveModel()
        {
            CreateKeys();
        }

        private void CreateKeys()
        {            
            for (int i = 0; i < 7; i++)
            {
                //Todo: Draw keybaord...
                //KeyModelList.Add(new WhiteKey());
                //or a black key...
                //KeyModelList.Add(new BlackKey());
            }
        }
    }
}
