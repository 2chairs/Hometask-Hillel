using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Hometask1
{
    internal class Calculator
    {
        static Stack<double> Numbers;
        static Stack<Operation> Operations;

        public static double Calculate (List<Token> tokens)
        {
            Numbers = new();
            Operations = new();

            foreach (var token in tokens)
            {
                switch (token)
                {

                    case Number number:
                        Numbers.Push(number.Value);
                        break;

                    case RightParenthesis:
                        CalculateParenthes();
                        break;

                    case Operation operation:
                        while (Operations.TryPeek (out var top) && top.IsPriosThan(operation))
                        {
                            Operations.Pop ();
                            Calculate(top);
                        }

                        Operations.Push(operation);
                        break;
                }
            }

            while (Operations.TryPop (out var operation))
            {
                if (operation is LeftParenthesis)
                {
                    throw new ArgumentException("extra left paranthesis");
                }
                Calculate(operation);
            }

            return Numbers.Pop ();
        }

        private static void CalculateParenthes()
        {
            while (true)
            {
                var top = Operations.Pop();
                if (top is LeftParenthesis)
                {
                    return;
                }

                Calculate (top);
            }
        }
        private static void Calculate (Operation operation)
        {
            if (operation is BinaryOperation binary)
            {
                var right = Numbers.Pop ();
                var left = Numbers.Pop ();
                var result = binary.Calculate (right, left);
                Numbers.Push (result);
            }
        }

    }
}
