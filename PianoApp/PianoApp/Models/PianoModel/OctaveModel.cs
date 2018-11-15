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

        public OctaveModel(int pos)
        {
            Position = pos;
            CreateKeys();
            DrawPianoInConsole();
        }

        private void DrawPianoInConsole()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var keyModel in KeyModelList)
            {
                sb.Append(keyModel.Step);
                sb.Append(Position);
                sb.Append(keyModel.Alter);
                sb.Append(" ");
            }

            Console.WriteLine(sb);
        }

        private void CreateKeys()
        {            
            for (int i = 0; i < 7; i++)
            {
                switch (i)
                {
                    case 0:
                        KeyModelList.Add(new WhiteKey() { Step = Step.C });
                        KeyModelList.Add(new BlackKey() { Step = Step.C, Alter = 1 });
                        break;
                    case 1:
                        KeyModelList.Add(new WhiteKey() { Step = Step.D });
                        KeyModelList.Add(new BlackKey() { Step = Step.D, Alter = -1 });
                        KeyModelList.Add(new BlackKey() { Step = Step.D, Alter = 1 });
                        break;
                    case 2:
                        KeyModelList.Add(new WhiteKey() { Step = Step.E });
                        KeyModelList.Add(new BlackKey() { Step = Step.E, Alter = -1 });
                        break;
                    case 3:
                        KeyModelList.Add(new WhiteKey() { Step = Step.F });
                        KeyModelList.Add(new BlackKey() { Step = Step.F, Alter = 1 });
                        break;
                    case 4:
                        KeyModelList.Add(new WhiteKey() { Step = Step.G });
                        KeyModelList.Add(new BlackKey() { Step = Step.G, Alter = -1 });
                        KeyModelList.Add(new BlackKey() { Step = Step.G, Alter = 1 });
                        break;
                    case 5:
                        KeyModelList.Add(new WhiteKey() { Step = Step.A });
                        KeyModelList.Add(new BlackKey() { Step = Step.A, Alter = 1 });
                        break;
                    case 6:
                        KeyModelList.Add(new WhiteKey() { Step = Step.B });
                        KeyModelList.Add(new BlackKey() { Step = Step.B, Alter = -1 });
                        break;
                }
            }
            Console.WriteLine($"Octave: {Position} Keys: {KeyModelList.Count}");            
        }
    }
}
