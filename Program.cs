namespace Hometask1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, try calculator \nyou can try to do some math operation like:\n+ , -, *, /, & (raise to a power) etc \nyou also can use () paranthesis\n");
            var tokens = Tokenizer.Tokenize("6+7"); 
            var result = Calculator.Calculate(tokens);
            result.ToString();
            // line 54 in Tokinizer class - unknown for me exception
        }
    }
}