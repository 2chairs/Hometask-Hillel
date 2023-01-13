namespace Hometask1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, try calculator \nyou can try to do some math operation like:\n+ , -, *, /, & (raise to a power) etc \nyou also can use () paranthesis\n");
            var tokens = Tokenizer.Tokenize("5.2+2&3-7"); // komment
            var result = Calculator.Calculate(tokens);
            result.ToString();

        }
    }
}