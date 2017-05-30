using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kazantseva.Nsudotnet.LinesCounter
{
    public enum LineState
    {
        Line,
        Inline,
        StartBlock,
        EndBlock,
        StartBlockInside,
        EndBlockInside
    };

    class Counter
    {
        private String _extention;
        private int _lines;

        public Counter(String extention)
        {
            this._extention = extention;
        }

        private LineState checkLine(String line, bool state)
        {
            bool commentOpened = false;
            bool commentOpenedInside = false;
            bool slash = false;
            bool asterisk = false;
            bool inline = false;
            int pos = 0;

            foreach (char c in line.ToCharArray())
            {
                if (c == '/' && slash)
                {
                    asterisk = false;
                    if (commentOpened)
                    {
                        ++pos;
                        continue;
                    }
                    else
                    {
                        if (pos == 1)
                        {
                            inline = true;
                        }
                        ++pos;
                        break;
                    }
                }

                if (c == '*' && slash)
                {
                    slash = false;
                    commentOpened = true;
                    if (pos != 1)
                    {
                        commentOpenedInside = true;
                    }
                    ++pos;
                    continue;
                }

                if (c == '/' && asterisk)
                {
                    if (commentOpened)
                    {
                        if (pos == line.Length)
                        {
                            return (state) ? LineState.EndBlock : LineState.Line;
                        }
                        else
                        {
                            return (state) ? LineState.EndBlockInside : LineState.Line;
                        }
                    }
                    else if (state)
                    {
                        return (pos == line.Length) ? LineState.EndBlock : LineState.EndBlockInside;
                    }
                    
                }

                slash = (c == '/') ? true : false;
                asterisk = (c == '*') ? true : false;
                ++pos;
            }

            if (!commentOpened)
            {
                return (inline) ? LineState.Inline : LineState.Line;
            }

            return (commentOpenedInside) ? LineState.StartBlockInside : LineState.StartBlock;
        }

        public int CountLines()
        {
            IEnumerable<String> files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*." + _extention,
                SearchOption.AllDirectories);
            foreach (String f in files)
            {
                _lines += CheckFile(f);
            }

            return _lines;
        }

        private int CheckFile(String fileName)
        {
            int linesCount = 0;
            bool commentOpened = false;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader fileReader = new StreamReader(fileStream))
                {
                    while (!fileReader.EndOfStream)
                    {
                        String currentLine = fileReader.ReadLine();

                        if (String.IsNullOrWhiteSpace(currentLine)) 
                        {
                            continue;
                        }

                        switch (checkLine(currentLine, commentOpened))
                        {
                            case LineState.Line: 
                                if (!commentOpened)
                                {
                                    ++linesCount;
                                }
                                break;

                            case LineState.Inline: 
                                break;

                            case LineState.StartBlock:
                                commentOpened = true;
                                break;

                            case LineState.StartBlockInside: 
                                if (!commentOpened)
                                {
                                    ++linesCount;
                                }
                                commentOpened = true;
                                break;

                            case LineState.EndBlock: 
                                if (commentOpened)
                                {
                                    commentOpened = false;
                                }
                                break; 
                            case LineState.EndBlockInside:
                                commentOpened = false;
                                ++linesCount;
                                break;
                        }
                    }
                }
            }
            return linesCount;
        }


    }
}
