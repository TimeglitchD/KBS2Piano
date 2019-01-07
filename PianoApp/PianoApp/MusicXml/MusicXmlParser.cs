using MusicXml.Domain;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Encoding = MusicXml.Domain.Encoding;

namespace MusicXml
{
    public static class MusicXmlParser
    {
        public static Score GetScore(string filename)
        {
            var document = GetXmlDocument(filename);

            var score = new Score();
            score.Systems = 0;

            var movementTitleNode = document.SelectSingleNode("score-partwise/movement-title");
            if (movementTitleNode != null)
                score.MovementTitle = movementTitleNode.InnerText;

            score.Identification = GetIdentification(document);

            var partNodes = document.SelectNodes("score-partwise/part-list/score-part");


            var layoutNode = document.SelectSingleNode("score-partwise/defaults/page-layout");
            var marginNode = layoutNode.SelectSingleNode("page-margins");
            double width = Convert.ToDouble(layoutNode.SelectSingleNode("page-width").InnerText);
            var marginlistLeft = marginNode.SelectNodes("left-margin");
            var marginlistRight = marginNode.SelectNodes("right-margin");
            double marginLeft = Convert.ToDouble(marginlistLeft.Item(0).InnerText);
            double marginRight = Convert.ToDouble(marginlistLeft.Item(0).InnerText);

            score.Scale = (width - marginLeft - marginRight) / 1055;

            if (partNodes != null)
            {
                foreach (XmlNode partNode in partNodes)
                {
                    var part = new Part();
                    score.Parts.Add(part);

                    if (partNode.Attributes != null)
                        part.Id = partNode.Attributes["id"].InnerText;

                    var partNameNode = partNode.SelectSingleNode("part-name");

                    if (partNameNode != null)
                        part.Name = partNameNode.InnerText;

                    var measuresXpath = string.Format("//part[@id='{0}']/measure", part.Id);

                    var measureNodes = partNode.SelectNodes(measuresXpath);

                    if (measureNodes != null)
                    {
                        foreach (XmlNode measureNode in measureNodes)
                        {
                            var measure = new Measure();

                            if (measureNode.Attributes != null)
                            {
                                var measureWidthAttribute = measureNode.Attributes["width"];
                                decimal w;
                                if (measureWidthAttribute != null && decimal.TryParse(measureWidthAttribute.InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out w))
                                    measure.Width = w / (decimal)score.Scale;
                            }
                            if (measureNode.Attributes != null)
                            {
                                var measureWidthAttribute = measureNode.Attributes["width"];
                                decimal w;
                                if (measureWidthAttribute != null && decimal.TryParse(measureWidthAttribute.InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out w))
                                    measure.Width = w;

                                var measureNumberAttribute = measureNode.Attributes["number"];

                                int n;
                                if (measureNumberAttribute != null && int.TryParse(measureNumberAttribute.InnerText, out n))
                                    measure.Number = n;

                            }


                            if (measureNode.SelectSingleNode("print") != null)
                            {
                                score.Systems++;
                                measure.NewSystem = true;
                            }


                            var attributesNode = measureNode.SelectSingleNode("attributes");

                            if (attributesNode != null)
                            {
                                measure.Attributes = new MeasureAttributes();

                                var divisionsNode = attributesNode.SelectSingleNode("divisions");
                                if (divisionsNode != null)
                                    measure.Attributes.Divisions = Convert.ToInt32(divisionsNode.InnerText);

                                var keyNode = attributesNode.SelectSingleNode("key");

                                if (keyNode != null)
                                {
                                    measure.Attributes.Key = new Key();

                                    var fifthsNode = keyNode.SelectSingleNode("fifths");
                                    if (fifthsNode != null)
                                        measure.Attributes.Key.Fifths = Convert.ToInt32(fifthsNode.InnerText);

                                    var modeNode = keyNode.SelectSingleNode("mode");
                                    if (modeNode != null)
                                        measure.Attributes.Key.Mode = modeNode.InnerText;
                                }

                                measure.Attributes.Time = GetTime(attributesNode);

                                measure.Attributes.Clef = GetClef(attributesNode);
                            }

                            var childNodes = measureNode.ChildNodes;

                            int voice1 = 0;
                            int voice2 = 0;

                            foreach (XmlNode node in childNodes)
                            {
                                MeasureElement measureElement = null;
                                

                                if (node.Name == "note")
                                {

                                    var voiceNode = node.SelectSingleNode("voice");
                                    if (voiceNode != null)
                                    {
                                        var voice = voiceNode.InnerText;

                                        var staffNode = node.SelectSingleNode("staff");
                                        if (staffNode != null)
                                        {
                                            int staff = Convert.ToInt32(staffNode.InnerText);

                                            if (staff == 1)
                                            {
                                                if (voice1 == 0)
                                                {
                                                    voice1 = Convert.ToInt32(voice);
                                                }
                                            }else if (staff == 2)
                                            {
                                                if (voice2 == 0)
                                                {
                                                    voice2 = Convert.ToInt32(voice);
                                                }
                                            }
                                        }
                                    }


                                    int currentvoice = Convert.ToInt32(node.SelectSingleNode("voice").InnerText);

                                    if (node.SelectSingleNode("grace") == null && (currentvoice.Equals(voice1) || currentvoice.Equals(voice2)))
                                    {
                                        var newNote = GetNote(node);
                                        measureElement = new MeasureElement { Type = MeasureElementType.Note, Element = newNote };

                                        var note = (Note)measureElement.Element;
                                        note.MeasureNumber = measure.Number;
                                    }

                                }
                                else if (node.Name == "backup")
                                {
                                    measureElement = new MeasureElement { Type = MeasureElementType.Backup, Element = GetBackupElement(node) };
                                }
                                else if (node.Name == "forward")
                                {
                                    measureElement = new MeasureElement { Type = MeasureElementType.Forward, Element = GetForwardElement(node) };
                                }

                                if (measureElement != null)
                                    measure.MeasureElements.Add(measureElement);
                            }

                            part.Measures.Add(measure);
                        }
                    }
                }
            }

            return score;
        }

        private static Forward GetForwardElement(XmlNode node)
        {
            var forward = new Forward();

            var forwardNode = node.SelectSingleNode("duration");

            if (forwardNode != null)
            {
                forward.Duration = Convert.ToInt32(forwardNode.InnerText);
            }

            return forward;
        }

        private static Backup GetBackupElement(XmlNode node)
        {
            var backup = new Backup();

            var backupNode = node.SelectSingleNode("duration");

            if (backupNode != null)
            {
                backup.Duration = Convert.ToInt32(backupNode.InnerText);
            }

            return backup;
        }

        private static Note GetNote(XmlNode noteNode)
        {
            var note = new Note();

            var typeNode = noteNode.SelectSingleNode("type");
            if (typeNode != null)
                note.Type = typeNode.InnerText;

            var voiceNode = noteNode.SelectSingleNode("voice");
            if (voiceNode != null)
                note.Voice = Convert.ToInt32(voiceNode.InnerText);

            var durationNode = noteNode.SelectSingleNode("duration");
            if (durationNode != null)
                note.Duration = Convert.ToInt32(durationNode.InnerText);

            var accidental = noteNode.SelectSingleNode("accidental");
            if (accidental != null)
                note.Accidental = accidental.InnerText;

            var notations = noteNode.SelectSingleNode("notations");
            if(notations != null)
            {
                var technical = notations.SelectSingleNode("technical");
                if(technical != null)
                {
                    var fingering = technical.SelectSingleNode("fingering");
                    if(fingering != null)
                    {
                        note.FingerNum = Convert.ToInt32(fingering.InnerText);
                    }
                }
            }

            var xPos = noteNode.Attributes["default-x"];
            if (xPos != null)
            {
                note.XPos = float.Parse(xPos.Value, CultureInfo.InvariantCulture.NumberFormat);

            }

            var rest = noteNode.SelectSingleNode("rest");
            if (rest == null)
            {

                note.IsRest = false;
            }
            else
            {
                note.IsRest = true;
            }

            var dot = noteNode.SelectSingleNode("dot");
            if (dot == null)
            {

                note.Dot = false;
            }
            else
            {
                note.Dot = true;
            }

            note.Lyric = GetLyric(noteNode);

            note.Pitch = GetPitch(noteNode);

            var staffNode = noteNode.SelectSingleNode("staff");
            if (staffNode != null)
            {
                note.Staff = Convert.ToInt32(staffNode.InnerText);
            }

            var stemNode = noteNode.SelectSingleNode("stem");
            if (stemNode != null)
            {
                note.Stem = stemNode.InnerText;
            }

            var chordNode = noteNode.SelectSingleNode("chord");
            if (chordNode != null)
                note.IsChordTone = true;

            return note;
        }

        private static Pitch GetPitch(XmlNode noteNode)
        {
            var pitch = new Pitch();
            var pitchNode = noteNode.SelectSingleNode("pitch");
            if (pitchNode != null)
            {
                var stepNode = pitchNode.SelectSingleNode("step");
                if (stepNode != null)
                    pitch.Step = stepNode.InnerText[0];

                var alterNode = pitchNode.SelectSingleNode("alter");
                if (alterNode != null)
                    pitch.Alter = Convert.ToInt32(alterNode.InnerText);

                var octaveNode = pitchNode.SelectSingleNode("octave");
                if (octaveNode != null)
                    pitch.Octave = Convert.ToInt32(octaveNode.InnerText);
            }
            else
            {
                pitch.Step = 'R';
            }

            return pitch;
        }

        private static Lyric GetLyric(XmlNode noteNode)
        {
            var lyric = new Lyric();

            var lyricNode = noteNode.SelectSingleNode("lyric");
            if (lyricNode != null)
            {
                var syllabicNode = lyricNode.SelectSingleNode("syllabic");

                var syllabicText = string.Empty;

                if (syllabicNode != null)
                    syllabicText = syllabicNode.InnerText;

                switch (syllabicText)
                {
                    case "":
                        lyric.Syllabic = Syllabic.None;
                        break;
                    case "begin":
                        lyric.Syllabic = Syllabic.Begin;
                        break;
                    case "single":
                        lyric.Syllabic = Syllabic.Single;
                        break;
                    case "end":
                        lyric.Syllabic = Syllabic.End;
                        break;
                    case "middle":
                        lyric.Syllabic = Syllabic.Middle;
                        break;
                }

                var textNode = lyricNode.SelectSingleNode("text");
                if (textNode != null)
                    lyric.Text = textNode.InnerText;
            }
            return lyric;
        }

        private static Clef GetClef(XmlNode attributesNode)
        {
            var clef = new Clef();

            var clefNode = attributesNode.SelectSingleNode("clef");

            if (clefNode != null)
            {
                var lineNode = clefNode.SelectSingleNode("line");
                if (lineNode != null)
                    clef.Line = Convert.ToInt32(lineNode.InnerText);

                var signNode = clefNode.SelectSingleNode("sign");
                if (signNode != null)
                    clef.Sign = signNode.InnerText;
            }
            return clef;
        }

        private static Time GetTime(XmlNode attributesNode)
        {
            var time = new Time();

            var timeNode = attributesNode.SelectSingleNode("time");
            if (timeNode != null)
            {
                var beatsNode = timeNode.SelectSingleNode("beats");

                if (beatsNode != null)
                    time.Beats = Convert.ToInt32(beatsNode.InnerText);

                var beatTypeNode = timeNode.SelectSingleNode("beat-type");

                if (beatTypeNode != null)
                    time.Mode = beatTypeNode.InnerText;

                var symbol = TimeSymbol.Normal;

                if (timeNode.Attributes != null)
                {
                    var symbolAttribute = timeNode.Attributes["symbol"];

                    if (symbolAttribute != null)
                    {
                        switch (symbolAttribute.InnerText)
                        {
                            case "common":
                                symbol = TimeSymbol.Common;
                                break;
                            case "cut":
                                symbol = TimeSymbol.Cut;
                                break;
                            case "single-number":
                                symbol = TimeSymbol.SingleNumber;
                                break;
                        }
                    }
                }

                time.Symbol = symbol;
            }
            return time;
        }

        private static Identification GetIdentification(XmlNode document)
        {
            var identificationNode = document.SelectSingleNode("score-partwise/identification");

            if (identificationNode != null)
            {
                var identification = new Identification();

                var composerNode = identificationNode.SelectSingleNode("creator[@type='composer']");
                identification.Composer = composerNode != null ? composerNode.InnerText : string.Empty;

                var rightsNode = identificationNode.SelectSingleNode("rights");
                identification.Rights = rightsNode != null ? rightsNode.InnerText : string.Empty;

                identification.Encoding = GetEncoding(identificationNode);

                return identification;
            }

            return null;
        }

        private static Encoding GetEncoding(XmlNode identificationNode)
        {
            var encodingNode = identificationNode.SelectSingleNode("encoding");

            var encoding = new Encoding();

            if (encodingNode != null)
            {
                encoding.Software = GetInnerTextOfChildTag(encodingNode, "software");

                encoding.Description = GetInnerTextOfChildTag(encodingNode, "encoding-description");

                var encodingDate = encodingNode.SelectSingleNode("encoding-date");
                if (encodingDate != null)
                    encoding.EncodingDate = Convert.ToDateTime(encodingDate.InnerText);
            }

            return encoding;
        }

        private static string GetInnerTextOfChildTag(XmlNode encodingNode, string tagName)
        {
            var softwareStringBuilder = new StringBuilder();

            var encodingSoftwareNodes = encodingNode.SelectNodes(tagName);

            if (encodingSoftwareNodes != null)
            {
                foreach (XmlNode node in encodingSoftwareNodes)
                {
                    softwareStringBuilder.AppendLine(node.InnerText);
                }
            }

            return softwareStringBuilder.ToString();
        }

        private static XmlDocument GetXmlDocument(string filename)
        {
            var document = new XmlDocument();

            var xml = GetFileContents(filename);
            document.XmlResolver = null;
            document.LoadXml(xml);

            return document;
        }

        private static string GetFileContents(string filename)
        {
            using (var fileStream = new FileStream(filename, FileMode.Open))
            using (var streamReader = new StreamReader(fileStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}