using MusicXml.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PianoApp.Models
{
    public class OctaveModel
    {
        public int Position { get; set; }
        public List<KeyModel> KeyModelList { get; set; } = new List<KeyModel>();
        private DockPanel _octave = new DockPanel();

        public OctaveModel(int pos)
        {
            Position = pos;
            _octave.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            //_octave.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
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

        }

        private void CreateKeys()
        {
            int val = Position * 12;
            for (int i = 0; i < 7; i++)
            {
                switch (i)
                {
                    case 0:
                        KeyModelList.Add(new WhiteKey() { Step = Step.C, KeyNumber = 1 + val});
                        KeyModelList.Add(new BlackKey() { Step = Step.C, Alter = 1, KeyNumber = 2 + val });
                        break;
                    case 1:
                        KeyModelList.Add(new WhiteKey() { Step = Step.D, KeyNumber = 3 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.D, Alter = -1, KeyNumber = 3 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.D, Alter = 1, KeyNumber = 4 + val });
                        break;
                    case 2:
                        KeyModelList.Add(new WhiteKey() { Step = Step.E, KeyNumber = 5 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.E, Alter = -1, KeyNumber = 5 + val });
                        break;
                    case 3:
                        KeyModelList.Add(new WhiteKey() { Step = Step.F, KeyNumber = 6 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.F, Alter = 1, KeyNumber = 7 + val });
                        break;
                    case 4:
                        KeyModelList.Add(new WhiteKey() { Step = Step.G, KeyNumber = 8 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.G, Alter = -1, KeyNumber = 8 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.G, Alter = 1, KeyNumber = 9 + val });
                        break;
                    case 5:
                        KeyModelList.Add(new WhiteKey() { Step = Step.A, KeyNumber = 10 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.A, Alter = -1, KeyNumber = 10 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.A, Alter = 1, KeyNumber = 11 + val });
                        break;
                    case 6:
                        KeyModelList.Add(new WhiteKey() { Step = Step.B, KeyNumber = 12 + val });
                        KeyModelList.Add(new BlackKey() { Step = Step.B, Alter = -1, KeyNumber = 12 + val });
                        break;
                }
            }
        }

        public DockPanel DrawOctave(float width)
        {
            var previous = new KeyModel();
            int index = 0;
            foreach (var key in KeyModelList)
            {
                if (key.Alter >= 0) {
                    var newKey = key.Draw(width);

                    if (previous.type == "Black")
                    {
                        newKey.Margin = new System.Windows.Thickness(-(width / 4), 0, 0, 0);
                    }
                    if (key.type == "Black")
                    { 
                    newKey.Margin = new System.Windows.Thickness(-(width / 4),0,0,0);
                        DockPanel.SetZIndex(newKey, index);
                    }
                    
                    newKey.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    
                    _octave.Children.Add(newKey);
                    previous = key;
                    index++;
                }
            }

            return _octave;
        }
    }
}
