using System;
using System.Collections;

namespace InterpreterPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            string roman = "五億七千三百零二萬六千四百五十二";
            //分解：((五)億)((七千)(三百)(零)(二)萬)                                        
            //((六千)(四百)(五十)(二))                                     

            Context context = new Context(roman);
            ArrayList tree = new ArrayList();

            tree.Add(new GeExpression());
            tree.Add(new ShiExpression());
            tree.Add(new BaiExpression());
            tree.Add(new QianExpression());
            tree.Add(new WanExpression());
            tree.Add(new YiExpression());

            foreach (Expression exp in tree)
            {
                exp.Interpreter(context);
            }

            Console.WriteLine(roman);
            Console.Write(context.Data);

            Console.ReadLine();
        }
    }
}
