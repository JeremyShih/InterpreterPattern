using System;
using System.Collections;
using System.Collections.Generic;

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
        //  抽象表達式                                                           
        public abstract class Expression
        {
            protected Dictionary<string, int> table = new Dictionary<string, int>(9);

            protected Expression()
            {
                table.Add("一", 1);
                table.Add("二", 2);
                table.Add("三", 3);
                table.Add("四", 4);
                table.Add("五", 5);
                table.Add("六", 6);
                table.Add("七", 7);
                table.Add("八", 8);
                table.Add("九", 9);
            }

            public virtual void Interpreter(Context context)
            {
                if (context.Statement.Length == 0)
                {
                    return;
                }

                foreach (string key in table.Keys)
                {
                    int value = table[key];

                    if (context.Statement.EndsWith(key + GetPostFix()))
                    {
                        context.Data += value * Multiplier();
                        context.Statement = context.Statement.Substring(0, context.Statement.Length - GetLength());
                    }
                    if (context.Statement.EndsWith("零"))
                    {
                        context.Statement = context.Statement.Substring(0, context.Statement.Length - 1);
                    }
                }
            }

            public abstract string GetPostFix();

            public abstract int Multiplier();

            //這個可以通用，但是對於個位數字例外，所以用虛方法                                                  
            public virtual int GetLength()
            {
                return GetPostFix().Length + 1;
            }
        }

        //個位表達式                                                                 
        public sealed class GeExpression : Expression
        {
            public override string GetPostFix()
            {
                return "";
            }

            public override int Multiplier()
            {
                return 1;
            }

            public override int GetLength()
            {
                return 1;
            }
        }

        //十位表達式                                                                 
        public sealed class ShiExpression : Expression
        {
            public override string GetPostFix()
            {
                return "十";
            }

            public override int Multiplier()
            {
                return 10;
            }
        }

        //百位表達式                                                                 
        public sealed class BaiExpression : Expression
        {
            public override string GetPostFix()
            {
                return "百";
            }

            public override int Multiplier()
            {
                return 100;
            }
        }

        //千位表達式                                                                     
        public sealed class QianExpression : Expression
        {
            public override string GetPostFix()
            {
                return "千";
            }

            public override int Multiplier()
            {
                return 1000;
            }
        }

        //萬位表達式                                                                     
        public sealed class WanExpression : Expression
        {
            public override string GetPostFix()
            {
                return "萬";
            }

            public override int Multiplier()
            {
                return 10000;
            }

            public override void Interpreter(Context context)
            {
                if (context.Statement.Length == 0)
                {
                    return;
                }

                ArrayList tree = new ArrayList();

                tree.Add(new GeExpression());
                tree.Add(new ShiExpression());
                tree.Add(new BaiExpression());
                tree.Add(new QianExpression());

                foreach (string key in table.Keys)
                {
                    if (context.Statement.EndsWith(GetPostFix()))
                    {
                        int temp = context.Data;
                        context.Data = 0;

                        context.Statement = context.Statement.Substring(0, context.Statement.Length - GetLength() + 1);

                        foreach (Expression exp in tree)
                        {
                            exp.Interpreter(context);
                        }
                        context.Data = temp + context.Data * Multiplier();
                    }
                }
            }
        }

        //億位表達式                                                                     
        public sealed class YiExpression : Expression
        {
            public override string GetPostFix()
            {
                return "億";
            }

            public override int Multiplier()
            {
                return 100000000;
            }

            public override void Interpreter(Context context)
            {
                ArrayList tree = new ArrayList();

                tree.Add(new GeExpression());
                tree.Add(new ShiExpression());
                tree.Add(new BaiExpression());
                tree.Add(new QianExpression());

                foreach (string key in table.Keys)
                {
                    if (context.Statement.EndsWith(GetPostFix()))
                    {
                        int temp = context.Data;
                        context.Data = 0;
                        context.Statement = context.Statement.Substring(0, context.Statement.Length - GetLength() + 1);

                        foreach (Expression exp in tree)
                        {
                            exp.Interpreter(context);
                        }
                        context.Data = temp + context.Data * this.Multiplier();
                    }
                }
            }
        }

        //環境上下文                                                                     
        public sealed class Context
        {
            private string _statement;
            private int _data;

            public Context(string statement)
            {
                this._statement = statement;
            }

            public string Statement
            {
                get { return this._statement; }
                set { this._statement = value; }
            }

            public int Data
            {
                get { return this._data; }
                set { this._data = value; }
            }
        }
    }
}
