using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ComputerTheory
{
    public class Tape : IEnumerable<char>
    {
        public const char Blank = 'B';

        public string Name { get; } 
        
        /* Using two stacks give the ability for the tape be infinite in any direction. I am not sure if any paper
         explores it, but I think it's pretty elegant since I don't need to specify an upper boundary */
        private readonly InfiniteStack<char> leftStack = new(Blank);
        
        private readonly InfiniteStack<char> rightStack = new(Blank);
        
        private char current;
        
        public Tape(string tape, string name)
        {
            Name = name;
            switch (tape.Length)
            {
                case 0:
                    current = Blank;
                    return;
                case 1:
                    current = tape.Single();
                    return;
                default:
                    current = tape.First();
                    break;
            }

            foreach (char c in tape.Substring(1).Reverse())
            {
                rightStack.Push(c);
            }
        }

        public char Read()
        {
            return current;
        }

        public void Write(char c)
        {
            current = c;
        }

        public void ShiftLeft()
        {
            rightStack.Push(current);
            current = leftStack.Pop();
        }

        public void ShiftRight()
        {
            leftStack.Push(current);
            current = rightStack.Pop();
        }
        
        public override string ToString()
        {
            return $"{Name}: {RemoveBorderBlanks(string.Concat(this))}";
        }

        private static string RemoveBorderBlanks(string str)
        {
            while (str.StartsWith(Blank))
            {
                str = str.Substring(1);
            }

            while (str.EndsWith(Blank))
            {
                str = str.Substring(0, str.Length - 1);
            }

            return str == "" ? "[Empty Tape]" : str;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public IEnumerator<char> GetEnumerator()
        {
            foreach (char left in leftStack.Reverse())
            {
                yield return left;
            }

            yield return '_';

            yield return current;

            foreach (char right in rightStack)
            {
                yield return right;
            }
        }

        public bool IsBlank()
        {
            return current == Blank && leftStack.All(x => x == Blank) && rightStack.All(x => x == Blank);
        }
    }
}